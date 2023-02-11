using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Firebase.Auth;

public class OnSceneSpawn : MonoBehaviour
{
    // Constant values
    public const string HAIR_CATEGORY = "hair";
    public const string HATS_CATEGORY = "hats";
    public const string SHIRTS_CATEGORY = "shirts";
    public const string PANTS_CATEGORY = "pants";
    public const string HANDS_CATEGORY = "hands";
    public const string BACKGROUND_CATEGORY = "background";

    // Global variables
    public static string player_name = "name";
    public static bool accountGotClothes = false;
    public static bool isMoving = false;
    public static bool isFocus = false;
    public static bool settingsOpen = false;
    public static bool sound = true;
    public static bool isManager = false;
    public static bool ManagerMenuIsShown = false;

    public GameObject specialInputFieldEverybody;
    public GameObject managerGO;
    public AudioSource audio_source;
    public int counter = 0;

    void Start()
    {
        if (sound == true)
            audio_source.mute = false;
        else
            audio_source.mute = true;

        // player is manager, show manager menu
        if (ManagerMenuIsShown)
        {
            managerGO.SetActive(true);
            specialInputFieldEverybody.SetActive(true);
        }
    }

    async void Update()
    {
        if (LoadPhoton.afterGotGameObject && counter == 0)
        {
            counter++;
            string outfitJson = (await Main.Instance.FirebaseHelper.GetOutfit(FirebaseAuth.DefaultInstance.CurrentUser.DisplayName)).ToString();
            PutClothesOn(outfitJson);
            isManager = await Main.Instance.FirebaseHelper.GetManagerState();
        }
    }

    public void PutClothesOn(string json)
    {
        accountGotClothes = true;
        User user = JsonUtility.FromJson<User>(json);

        if (user == null)
            return;
        if (user.GetHair() != "" && LoadPhoton.afterGotGameObject)
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(1, user.GetHair());
        if (user.GetHat() != "" && LoadPhoton.afterGotGameObject)
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(0, user.GetHat());
        if (user.GetShirt() != "" && LoadPhoton.afterGotGameObject)
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(2, user.GetShirt());
        if (user.GetPants() != "" && LoadPhoton.afterGotGameObject)
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(5, user.GetPants());
        if (user.GetHand() != "")
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(3, user.GetHand());
        if (user.GetBackgronud() != "")
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(4, user.GetBackgronud());
        else
            LoadPhoton.playerScript.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("blank_bg");
    }
}