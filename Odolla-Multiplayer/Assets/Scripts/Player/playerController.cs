using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviourPunCallbacks
{
    public SpriteRenderer sp;
    public GameObject message;
    public TMP_Text text_message;
    public GameObject SpeechBubble;
    public GameObject SpeechBubble_text;

    public float speed;
    public static Vector3 targePos;
    public bool isMoving = false;

    public Animator anim;

    public void DisconnectFromServer()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("LoginPage");
    }

    public void showMessage(string text)
    {
        photonView.RPC("showMessageEveryone", RpcTarget.Others, text);
    }

    [PunRPC]
    void showMessageEveryone(string text)
    {
        text_message.text = text;
        transform.GetChild(9).gameObject.SetActive(true);
    }

    public void closeWindow()
    {
        message.SetActive(false);
    }

    private void Start()
    {
        speed = 4f;
    }

    private void OnMouseDown()
    {
        if (!photonView.IsMine)
        {
            OtherPlayerCardFunctionality.selectedPlayer = GetComponent<PhotonView>().Owner.NickName;
            OtherPlayerCardFunctionality.gotSelectedUsername = true;
        }
    }

    public void showBubblePrivate(string text)
    {
        if (photonView.IsMine)
        {
            text_message.text = text;
            message.SetActive(true);
        }
    }

    public void showBubble(string text)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("showSpeechBubble", RpcTarget.AllBuffered, text);
        }
    }

    [PunRPC]
    void showSpeechBubble(string text)
    {
        if (text.Length < 54 && text.Length > 1)
        {
            SpeechBubble_text.GetComponent<TMP_Text>().text = text;
            SpeechBubble.SetActive(true);
            StartCoroutine(ExecuteAfterTime(4));
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SpeechBubble.SetActive(false);
    }

    public void changeClothes(int bodypart, string cloth)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("changeSprite", RpcTarget.AllBuffered, bodypart, cloth);
        }
    }

    [PunRPC]
    void changeSprite(int bodypart, string cloth)
    {
        transform.GetChild(bodypart).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(cloth);
    }

    void FixedUpdate()
    {
        if (photonView.IsMine && isMoving)
        {
            move();
        }

        if (photonView.IsMine && !isMoving)
            photonView.RPC("StopAnimation", RpcTarget.All);
    }


    public void SetTargetPos()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            photonView.RPC("StartAnimation", RpcTarget.All);
            targePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targePos.z = transform.position.z;
            isMoving = true;
        }
    }

    public void ResetTargetPos()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            targePos = Vector3.zero;
            targePos.z = transform.position.z;
            isMoving = false;
        }
    }

    void move()
    {
        if (targePos.x < transform.position.x)
        {
            photonView.RPC("FlipTrue", RpcTarget.All);
        }
        if (targePos.x > transform.position.x)
        {
            photonView.RPC("FlipFalse", RpcTarget.All);
        }

        transform.position = Vector3.MoveTowards(transform.position, targePos, speed * Time.deltaTime);
        if (transform.position == targePos)
        {
            isMoving = false;

            if(photonView.IsMine)
                photonView.RPC("StopAnimation", RpcTarget.All);
        }
    }

    [PunRPC]
    private void FlipTrue()
    {
        transform.eulerAngles = new Vector3(0, -180, 0);
        placeTheClothes();
    }

    [PunRPC]
    private void FlipFalse()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        placeTheClothes();
    }

    [PunRPC]
    private void StartAnimation()
    {
        OnSceneSpawn.isMoving = true;
        GetComponent<Animator>().Play("Base Layer.Walk", 0, 0f);
        transform.GetChild(5).gameObject.GetComponent<PantsFunctionality>().playAnime();
    }

    [PunRPC]
    private void StopAnimation()
    {
        OnSceneSpawn.isMoving = false;
        GetComponent<Animator>().Rebind();
        transform.GetChild(5).gameObject.GetComponent<PantsFunctionality>().stop();
    }

    public void placeTheClothes()
    {
        transform.GetChild(0).gameObject.transform.position = new Vector3(transform.GetChild(0).gameObject.transform.position.x, transform.GetChild(0).gameObject.transform.position.y, 10);
        transform.GetChild(1).gameObject.transform.position = new Vector3(transform.GetChild(1).gameObject.transform.position.x, transform.GetChild(1).gameObject.transform.position.y, 5);
        transform.GetChild(2).gameObject.transform.position = new Vector3(transform.GetChild(2).gameObject.transform.position.x, transform.GetChild(2).gameObject.transform.position.y, 5);
        transform.GetChild(3).gameObject.transform.position = new Vector3(transform.GetChild(3).gameObject.transform.position.x, transform.GetChild(3).gameObject.transform.position.y, 5);

    }
}