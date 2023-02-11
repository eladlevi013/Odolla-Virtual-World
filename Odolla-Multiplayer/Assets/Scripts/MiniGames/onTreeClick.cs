using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class onTreeClick : MonoBehaviour
{
    public string game_name;
    public GameObject doYouWantToPlayCavnas;

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
        if (OnSceneSpawn.isFocus == false)
        {
            OnSceneSpawn.isFocus = true;
            doYouWantToPlayCavnas.SetActive(true);
        }
    }

    public void closeWindow()
    {
        doYouWantToPlayCavnas.SetActive(false);
        OnSceneSpawn.isFocus = false;
    }

    public void openTheGame()
    {
        PhotonNetwork.Disconnect();
        LoadPhoton.playerScript.GetComponent<playerController>().ResetTargetPos();
        SceneManager.LoadScene(game_name);
        OnSceneSpawn.accountGotClothes = false;
    }
}