using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SetNewArea : MonoBehaviourPun
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        if(LoadPhoton.afterGotGameObject == true && LoadPhoton.playerScript.GetComponent<PhotonView>().IsMine && OnSceneSpawn.isFocus ==false && OnSceneSpawn.isMoving==false)
        {
            LoadPhoton.playerScript.GetComponent<playerController>().SetTargetPos();
        }
    }
}