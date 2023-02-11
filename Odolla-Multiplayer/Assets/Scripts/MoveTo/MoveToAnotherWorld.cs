using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MoveToAnotherWorld : MonoBehaviourPunCallbacks
{
    public string sceneName;
    public GameObject Arrow;

    void Start()
    {
        Arrow.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        if (LoadPhoton.afterGotGameObject == true)
        {
            float dist = Vector3.Distance(Arrow.transform.position, LoadPhoton.playerScript.transform.position);

            if (dist < 1.7f)
            {
                LoadPhoton.playerScript.GetComponent<playerController>().ResetTargetPos();
                PhotonNetwork.Disconnect();
                OnSceneSpawn.isFocus = true;
                SceneManager.LoadScene(sceneName);
                OnSceneSpawn.accountGotClothes = false;
            }
        }
    }

    public void OnMouseEnter()
    {
        Arrow.GetComponent<Renderer>().enabled = true;
    }

    public void OnMouseExit()
    {
        Arrow.GetComponent<Renderer>().enabled = false;
    }

    public void OnMouseDown()
    {
        if(OnSceneSpawn.isFocus == false)
        {
            LoadPhoton.playerScript.GetComponent<playerController>().SetTargetPos();
        }
    }
}
