using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MoveToManagerRoom : MonoBehaviourPunCallbacks
{
    public string sceneName;
    public GameObject Arrow;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (LoadPhoton.playerScript != null && LoadPhoton.afterGotGameObject == true)
        {
            float dist = Vector3.Distance(Arrow.transform.position, LoadPhoton.playerScript.transform.position);
            if (dist < 1.7f)
            {
                LoadPhoton.playerScript.GetComponent<playerController>().ResetTargetPos();
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene(sceneName);
                OnSceneSpawn.accountGotClothes = false;
            }
        }
    }

    public void OnMouseDown()
    {
        if(OnSceneSpawn.isFocus == false)
        {
            LoadPhoton.playerScript.GetComponent<playerController>().SetTargetPos();
        }
    }
}
