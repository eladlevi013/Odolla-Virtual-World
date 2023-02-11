using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LoadPhoton : MonoBehaviourPunCallbacks
{
    public GameObject loading;

    public static bool afterGotGameObject = false;

    public static GameObject playerScript;

    public GameObject player;

    public string roomName = "";
    public int MaxPlayers = 20;
    public RoomOptions roomOptions = new RoomOptions();

    // Start is called before the first frame update
    void Start()
    {
        OnSceneSpawn.isFocus = true;
        loading.SetActive(true);

        afterGotGameObject = false;
        PhotonNetwork.ConnectUsingSettings();
        print("connecting");
    }

    public override void OnConnectedToMaster()
    {
        roomOptions.MaxPlayers = (byte)MaxPlayers;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        print("connected");
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        print("Connected to room");
        playerScript = PhotonNetwork.Instantiate(player.name, new Vector3(Vector3.zero.x, Vector3.zero.y - 2f, 0) , Quaternion.identity);
        afterGotGameObject = true;
    }

    void Update()
    {
        if (OnSceneSpawn.accountGotClothes && loading.active)
        {
            loading.SetActive(false);
            OnSceneSpawn.isFocus = false;
        }
    }
}