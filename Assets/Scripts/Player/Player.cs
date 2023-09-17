using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    public SpriteRenderer sp;
    public GameObject bubble;
    public GameObject text;
    public GameObject shadow;

    public float speed;
    public Vector3 targePos;
    public bool isMoving = false;

    public Animator anim;

    private void Start()
    {
        speed = 4f;
    }

    void FixedUpdate()
    {
        bubble.transform.position = new Vector3(transform.position.x + 1.3f, transform.position.y + 1.3f, transform.position.z);
        text.transform.position = new Vector3(transform.position.x, transform.position.y - 1.1f, transform.position.z);
        shadow.transform.position = new Vector3(transform.position.x, transform.position.y - 0.9f, transform.position.z);

        if (isMoving)
        {
            move();
        }

        anim.SetBool("walk", isMoving);
    }

    public void SetTargetPos()
    {
        targePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targePos.z = transform.position.z;
        isMoving = true;
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
        }
    }

    [PunRPC]
    private void FlipTrue()
    {
        sp.flipX = true;
    }

    [PunRPC]
    private void FlipFalse()
    {
        sp.flipX = false;
    }
}
