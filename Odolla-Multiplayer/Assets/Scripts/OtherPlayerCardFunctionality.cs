using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OtherPlayerCardFunctionality : MonoBehaviour
{
    // selectedPlayer updated on another script
    public static string selectedPlayer = "";

    // isOpen updated on another script when its get the username
    public static bool gotSelectedUsername = false;

    // canvas is the canvas of the card
    public GameObject canvas;
    public Text username;

    // all the clothes of the player
    public GameObject Hair;
    public GameObject Hats;
    public GameObject Shirt;
    public GameObject Pants;
    public GameObject Hand;
    public GameObject Background;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    async void Update()
    {
        if(gotSelectedUsername == true && LoadPhoton.afterGotGameObject)
        {
            if (OnSceneSpawn.isFocus == false)
            {
                canvas.SetActive(true);
                username.text = selectedPlayer;
                string selectedPlayerOutfit = await Main.Instance.FirebaseHelper.GetOutfit(selectedPlayer);
                PutClothesOn(selectedPlayerOutfit);
                gotSelectedUsername = false;
                selectedPlayer = "";
                OnSceneSpawn.isFocus = true;
            }
        }
    }

    public void closeCanvas()
    {
        canvas.SetActive(false);
        gotSelectedUsername = false;
        selectedPlayer = "";
        OnSceneSpawn.isFocus = false;
    }

    public void PutClothesOn(string json)
    {
        User user = JsonUtility.FromJson<User>(json);

        if (user.GetHair() != "")
            Hair.GetComponent<Image>().sprite = Resources.Load<Sprite>(user.GetHair());
        if (user.GetHat() != "")
            Hats.GetComponent<Image>().sprite = Resources.Load<Sprite>(user.GetHat());
        if (user.GetShirt() != "")
            Shirt.GetComponent<Image>().sprite = Resources.Load<Sprite>(user.GetShirt());
        if (user.GetPants() != "")
            Pants.GetComponent<Image>().sprite = Resources.Load<Sprite>(user.GetPants());
        if (user.GetHand() != "") Resources.Load<Sprite>(user.GetHand());
            Hand.GetComponent<Image>().sprite = Resources.Load<Sprite>(user.GetHand());
        if (user.GetBackgronud() != "")
            Background.GetComponent<Image>().sprite = Resources.Load<Sprite>(user.GetBackgronud());
        else
            Background.GetComponent<Image>().sprite = Resources.Load<Sprite>("blank_bg");
    }
}