using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
using Photon.Pun;
using Firebase.Auth;
using Firebase.Firestore;
using System.Threading.Tasks;
using System.Linq;
using Mono.Cecil;

public class GameCardFunctionality : MonoBehaviourPun
{
    // Requests Vars
    public bool gotData = false;
    public int lastRequestTime;

    // Texts and settings buttons
    public TMP_InputField inputFieldGO;
    public GameObject specialInputFieldEverybody;
    public GameObject managerGO;
    public GameObject manager_btn;
    public TMP_Text manager_menu;
    public TMP_Text sound_text;
    public AudioSource audio;
    public GameObject settings;
    public TMP_InputField everyone;
    public TMP_InputField text;
    public TMP_Text page_text;

    // Arrows
    public Button arrow_right;
    public Button arrow_left;

    // Tabs Buttons
    public GameObject hair_tab;
    public GameObject hat_tab;
    public GameObject shirt_tab;
    public GameObject pants_tab;
    public GameObject hand_tab;
    public GameObject background_tab;

    // Lists of the properties
    public List<string> HairList = new List<string>();
    public List<string> HatsList = new List<string>();
    public List<string> ShirtsList = new List<string>();
    public List<string> PantsList = new List<string>();
    public List<string> HandsList = new List<string>();
    public List<string> BackgroundList = new List<string>();

    // UI Elements
    public int page = 0;
    public string category = OnSceneSpawn.HAIR_CATEGORY;
    public TMP_InputField typingArea;
    public GameObject[] buttons;
    public GameObject[] buttons_images;
    public GameObject gameCardCanvas;
    public GameObject newHat;
    public GameObject newHair;
    public GameObject newShirt;
    public GameObject newPants;
    public GameObject newHand;
    public GameObject newBackground;
    public Text username_card;
    public Text money;
    public Text level;

    // Firebase Vars
    public FirebaseAuth auth;
    private FirebaseFirestore db;

    public void showEveryone()
    {
        LoadPhoton.playerScript.GetComponent<playerController>().showMessage(everyone.text);
        everyone.text = "";
    }

    public void PlayerDisconnet()
    {
        // game logout changes
        OnSceneSpawn.sound = true;
        OnSceneSpawn.isManager = false;
        OnSceneSpawn.ManagerMenuIsShown = false;
        OnSceneSpawn.isFocus = false;

        // Photon sign out
        LoadPhoton.playerScript.GetComponent<playerController>().DisconnectFromServer();
        
        // Firebase sign out
        Debug.LogFormat("Account {0} has disconnected successfully", auth.CurrentUser.DisplayName);
        auth.SignOut();
    }

    async void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        buttons_images = new GameObject[buttons.Length];
        for(int i=0; i<12; i++)
            buttons_images[i] = buttons[i].transform.GetChild(0).gameObject;
    }

    async void Update()
    {
        page_text.text = (page + 1) + "";
        if (page == 0)
            arrow_left.interactable = false;
        else
            arrow_left.interactable = true;

        if (category == OnSceneSpawn.HAIR_CATEGORY)
            arrowsInteractableInPlayerCard(HairList.Count);
        else if (category == OnSceneSpawn.HATS_CATEGORY)
            arrowsInteractableInPlayerCard(HatsList.Count);
        else if (category == OnSceneSpawn.SHIRTS_CATEGORY)
            arrowsInteractableInPlayerCard(ShirtsList.Count);
        else if (category == OnSceneSpawn.PANTS_CATEGORY)
            arrowsInteractableInPlayerCard(PantsList.Count);
        else if (category == OnSceneSpawn.HANDS_CATEGORY)
            arrowsInteractableInPlayerCard(HandsList.Count);
        else if (category == OnSceneSpawn.BACKGROUND_CATEGORY)
            arrowsInteractableInPlayerCard(BackgroundList.Count);
    }

    public void arrowsInteractableInPlayerCard(int ListSize)
    {
        if (page + 1 >= Math.Ceiling((double)ListSize / 12))
            arrow_right.interactable = false;
        else
            arrow_right.interactable = true;
    }

    public void show_bubble()
    {
        if (text.text.Length > 1)
        {
            inputFieldGO.interactable = false;
            LoadPhoton.playerScript.GetComponent<playerController>().showBubble(text.text);
            text.text = "";
            StartCoroutine(ExecuteAfterTime(4));
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        inputFieldGO.interactable = true;
    }

    public void SoundMute()
    {
        if(OnSceneSpawn.sound)
        {
            audio.mute = true;
            sound_text.text = "מושתק";
            OnSceneSpawn.sound = false;
        }
        else
        {
            audio.mute = false;
            sound_text.text = "שמע מופעל";
            OnSceneSpawn.sound = true;
        }
    }

    public void onButtonDisableManagerOptions()
    {
        if (OnSceneSpawn.ManagerMenuIsShown)
        {
            OnSceneSpawn.ManagerMenuIsShown = false;
            if (managerGO != null)
                managerGO.SetActive(false);
            if (specialInputFieldEverybody != null)
                specialInputFieldEverybody.SetActive(false);
            manager_menu.text = "תפריט מנהל מוסתר";
        }
        else
        {
            OnSceneSpawn.ManagerMenuIsShown = true;
            if (managerGO != null)
                managerGO.SetActive(true);
            if (specialInputFieldEverybody != null)
                specialInputFieldEverybody.SetActive(true);
            manager_menu.text = "תפריט מנהל מוצג";
        }
    }

    public void showSettings() {
        if (OnSceneSpawn.settingsOpen)
        {
            settings.SetActive(false);
            OnSceneSpawn.settingsOpen = false;
            OnSceneSpawn.isFocus = false;
        }
        else
        {
            settings.SetActive(true);
            manager_btn.SetActive(false);
            OnSceneSpawn.settingsOpen = true;
            OnSceneSpawn.isFocus = true;

            if (OnSceneSpawn.sound)
                sound_text.text = "שמע מופעל";
            else
                sound_text.text = "מושתק";

            if(OnSceneSpawn.isManager)
            {
                manager_btn.SetActive(true);
                if (OnSceneSpawn.ManagerMenuIsShown)
                    manager_menu.text = "תפריט מנהל מוצג";
                else
                    manager_menu.text = "תפריט מנהל מוסתר";
            }
        }
    }

    public void checkLetters()
    {
        for (int i = 0; i < text.text.Length; i++)
            if (text.text[i] >= 'a' && text.text[i] <= 'z' || text.text[i] >= 'A' && text.text[i] <= 'Z')
                text.text = text.text.ToString().Remove(i);
    }

    public void remove_item()
    {
        if(category == OnSceneSpawn.HATS_CATEGORY)
        {
            newHat.GetComponent<Image>().sprite = Resources.Load<Sprite>("empty");
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(0, "empty");
            onClothesChange();
        }
        else if(category == OnSceneSpawn.HAIR_CATEGORY)
        {
            newHair.GetComponent<Image>().sprite = Resources.Load<Sprite>("empty");
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(1, "empty");
            onClothesChange();
        }
        else if (category == OnSceneSpawn.SHIRTS_CATEGORY)
        {
            newShirt.GetComponent<Image>().sprite = Resources.Load<Sprite>("empty");
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(2, "empty");
            onClothesChange();
        }
        else if (category == OnSceneSpawn.PANTS_CATEGORY)
        {
            newPants.GetComponent<Image>().sprite = Resources.Load<Sprite>("empty");
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(5, "empty");
            onClothesChange();
        }
        else if (category == OnSceneSpawn.HANDS_CATEGORY)
        {
            newHand.GetComponent<Image>().sprite = Resources.Load<Sprite>("empty");
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(3, "empty");
            onClothesChange();
        }
        else if (category == OnSceneSpawn.BACKGROUND_CATEGORY)
        {
            newBackground.GetComponent<Image>().sprite = Resources.Load<Sprite>("blank_bg");
            LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(4, "blank_bg");
            onClothesChange();
        }
    }

    public void make_all_tabs_dark()
    {
        hair_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark");
        hat_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark");
        shirt_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark");
        pants_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark");
        hand_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark");
        background_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark");
    }

    public void changeCategory()
    {
        make_all_tabs_dark();
        arrow_right.interactable = true;
        page = 0;
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        lastRequestTime = cur_time;
        clearAllCells();
    }

    public void ChangeCategoryToHair()
    {
        changeCategory();
        hair_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright");
        category = OnSceneSpawn.HAIR_CATEGORY;
        orgenizeStuffOnGrid();
    }

    public void ChangeCategoryToHats()
    {
        changeCategory();
        hat_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright");
        category = OnSceneSpawn.HATS_CATEGORY;
        orgenizeStuffOnGrid();
    }

    public void ChangeCategoryToShirts()
    {
        changeCategory();
        shirt_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright");
        category = OnSceneSpawn.SHIRTS_CATEGORY;
        orgenizeStuffOnGrid();
    }

    public void ChangeCategoryToHands()
    {
        changeCategory();
        hand_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright");
        category = OnSceneSpawn.HANDS_CATEGORY;
        orgenizeStuffOnGrid();
    }

    public void ChangeCategoryToBackgrounds()
    {
        changeCategory();
        background_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright");
        category = OnSceneSpawn.BACKGROUND_CATEGORY;
        orgenizeStuffOnGrid();
    }

    public void ChangeCategoryToPants()
    {
        changeCategory();
        pants_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright");
        category = OnSceneSpawn.PANTS_CATEGORY;
        orgenizeStuffOnGrid();
    }

    public async Task orgenizeProperty()
    {
        orgenizeStuffOnGrid();

        string displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        DocumentReference docRef = db.Collection("users").Document(displayName);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.ContainsField("property"))
        {
            Dictionary<string, object> property = snapshot.GetValue<Dictionary<string, object>>("property");
            if (property.ContainsKey("hair"))
            {
                object hairValue = property["hair"];
                if (hairValue is List<object> hairList)
                {
                    HairList = hairList.Cast<string>().ToList();
                }
            }
            if (property.ContainsKey("hats"))
            {
                object hatsValue = property["hats"];
                if (hatsValue is List<object> hatsList)
                {
                    HatsList = hatsList.Cast<string>().ToList();
                }
            }
            if (property.ContainsKey("shirts"))
            {
                object shirtsValue = property["shirts"];
                if (shirtsValue is List<object> shirtsList)
                {
                    ShirtsList = shirtsList.Cast<string>().ToList();
                }
            }
            if (property.ContainsKey("pants"))
            {
                object pantsValue = property["pants"];
                if (pantsValue is List<object> pantsList)
                {
                    PantsList = pantsList.Cast<string>().ToList();
                }
            }
            if (property.ContainsKey("hands"))
            {
                object handsValue = property["hands"];
                if (handsValue is List<object> handsList)
                {
                    HandsList = handsList.Cast<string>().ToList();
                }
            }
            if (property.ContainsKey("background"))
            {
                object backgroundsValue = property["background"];
                if (backgroundsValue is List<object> backgroundsList)
                {
                    BackgroundList = backgroundsList.Cast<string>().ToList();
                }
            }
        }
        gotData = true;
    }

    public void onArrowRight()
    {
        if (gotData)
        {
            clearAllCells();
            if (category == OnSceneSpawn.HAIR_CATEGORY)
            {
                if (page + 1 >= Math.Ceiling((double)HairList.Count / 12))
                    arrow_right.interactable = false;
                else
                    page += 1;
            }
            else if (category == OnSceneSpawn.HATS_CATEGORY)
            {
                if (page + 1 >= Math.Ceiling((double)HatsList.Count / 12))
                    arrow_right.interactable = false;
                else
                    page += 1;
            }
            else if (category == OnSceneSpawn.SHIRTS_CATEGORY)
            {
                if (page + 1 >= Math.Ceiling((double)ShirtsList.Count / 12))
                    arrow_right.interactable = false;
                else
                    page += 1;
            }
            else if (category == OnSceneSpawn.PANTS_CATEGORY)
            {
                if (page + 1 >= Math.Ceiling((double)PantsList.Count / 12))
                    arrow_right.interactable = false;
                else
                    page += 1;
            }
            else if (category == OnSceneSpawn.HANDS_CATEGORY)
            {
                if (page + 1 >= Math.Ceiling((double)HandsList.Count / 12))
                    arrow_right.interactable = false;
                else
                    page += 1;
            }
            else if (category == OnSceneSpawn.BACKGROUND_CATEGORY)
            {
                if (page + 1 >= Math.Ceiling((double)BackgroundList.Count / 12))
                    arrow_right.interactable = false;
                else
                    page += 1;
            }
            orgenizeStuffOnGrid();
        }
    }

    public void orgenizeStuffOnGrid()
    {
        clearAllCells();
        if (gotData)
        {
            if (category == OnSceneSpawn.HAIR_CATEGORY)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (HairList.Count > (i + (page * 12)))
                    {
                        buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(HairList[i + (page * 12)]);
                        buttons_images[i].tag = "Hair";
                    }
                }
            }
            else if (category == OnSceneSpawn.HATS_CATEGORY)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (HatsList.Count > (i + (page * 12)))
                    {
                        buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(HatsList[i + (page * 12)]);
                        buttons_images[i].tag = "Hat";
                    }
                }
            }
            else if (category == OnSceneSpawn.SHIRTS_CATEGORY)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (ShirtsList.Count > (i + (page * 12)))
                    {
                        buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(ShirtsList[i + (page * 12)]);
                        buttons_images[i].tag = "Shirt";
                    }
                }
            }
            else if (category == OnSceneSpawn.PANTS_CATEGORY)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (PantsList.Count > (i + (page * 12)))
                    {
                        buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(PantsList[i + (page * 12)]);
                        buttons_images[i].tag = "Pants";
                    }
                }
            }
            else if (category == OnSceneSpawn.HANDS_CATEGORY)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (HandsList.Count > (i + (page * 12)))
                    {
                        buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(HandsList[i + (page * 12)]);
                        buttons_images[i].tag = "Hand";
                    }
                }
            }
            else if (category == OnSceneSpawn.BACKGROUND_CATEGORY)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (BackgroundList.Count > (i + (page * 12)))
                    {
                        buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(BackgroundList[i + (page * 12)]);
                        buttons_images[i].tag = "Background";
                    }
                }
            }
        }
    }

    public void onArrowLeft()
    {
        page -= 1;
        clearAllCells();
        arrow_right.interactable = true;
        orgenizeStuffOnGrid();
    }

    public string getTagByCategory()
    {
        String tag = "";

        if (category == OnSceneSpawn.HANDS_CATEGORY)
        {
            tag = "Hand";
        }
        else if (category == OnSceneSpawn.HAIR_CATEGORY)
        {
            tag = "Hair";
        }
        else if (category == OnSceneSpawn.HATS_CATEGORY)
        {
            tag = "Hat";
        }
        else if (category == OnSceneSpawn.SHIRTS_CATEGORY)
        {
            tag = "Shirt";
        }
        else if (category == OnSceneSpawn.PANTS_CATEGORY)
        {
            tag = "Pants";
        }
        else if (category == OnSceneSpawn.BACKGROUND_CATEGORY)
        {
            tag = "Background";
        }
        return tag;
    }

    public void clearAllCells()
    {
        for (int i = 0; i < 12; i++)
        {
            buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("empty");
            buttons_images[i].tag = getTagByCategory();
        }
    }

    public async void onBagButtonClicked()
    {
        if (OnSceneSpawn.isMoving == false)
        {
            page = 0;
            category = OnSceneSpawn.HAIR_CATEGORY;
            money.text = (await Main.Instance.FirebaseHelper.GetCoins()).ToString();
            level.text = (await Main.Instance.FirebaseHelper.GetLevel()).ToString();
            await orgenizeProperty();

            orgenizeStuffOnGrid();
            make_all_tabs_dark();
            hair_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright");
            username_card.text = OnSceneSpawn.player_name; // update the username of the gamecard
            fixClothesOnGameCard();

            OnSceneSpawn.isFocus = true;
            gameCardCanvas.SetActive(true); // Show the canvas
        }
    }

    public void fixClothesOnGameCard()
    {
        newHair.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
        newHat.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
        newShirt.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite;
        newPants.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().sprite;
        newHand.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite;

        if (LoadPhoton.playerScript.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite.name == "empty")
        {
            newBackground.GetComponent<Image>().sprite = Resources.Load<Sprite>("blank_bg");
        }
        else
        {
            newBackground.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void closeWindow()
    {
        OnSceneSpawn.isFocus = false;
        gameCardCanvas.SetActive(false);
    }
    
    public async void onClothesChange()
    {
        User user = new User();
        if (LoadPhoton.afterGotGameObject == true)
        {
            user.SetHair(LoadPhoton.playerScript.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite.name);
            user.SetHat(LoadPhoton.playerScript.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite.name);
            user.SetShirt(LoadPhoton.playerScript.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite.name);
            user.SetPants(LoadPhoton.playerScript.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().sprite.name);
            user.SetHand(LoadPhoton.playerScript.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite.name);

            if (LoadPhoton.playerScript.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite.name == "empty")
            {
                user.SetBackground("blank_bg");
            }
            else
            {
                user.SetBackground(LoadPhoton.playerScript.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite.name);
            }
        }
        await Main.Instance.FirebaseHelper.setOutfit(user);
    }

    public void onItemButtonClick(int index)
    {
        if (buttons_images[index].tag == "Hair")
        {
            newHair.GetComponent<Image>().sprite = buttons_images[index].GetComponent<Image>().sprite;
            if (LoadPhoton.afterGotGameObject)
                LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(1, buttons_images[index].GetComponent<Image>().sprite.name);
        }
        else if (buttons_images[index].tag == "Hat")
        {
            newHat.GetComponent<Image>().sprite = buttons_images[index].GetComponent<Image>().sprite;
            if (LoadPhoton.afterGotGameObject)
                LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(0, buttons_images[index].GetComponent<Image>().sprite.name);
        }

        else if (buttons_images[index].tag == "Shirt")
        {
            newShirt.GetComponent<Image>().sprite = buttons_images[index].GetComponent<Image>().sprite;
            if (LoadPhoton.afterGotGameObject)
                LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(2, buttons_images[index].GetComponent<Image>().sprite.name);
        }

        else if (buttons_images[index].tag == "Pants")
        {
            newPants.GetComponent<Image>().sprite = buttons_images[index].GetComponent<Image>().sprite;
            if (LoadPhoton.afterGotGameObject)
                LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(5, buttons_images[index].GetComponent<Image>().sprite.name);
        }

        else if (buttons_images[index].tag == "Hand")
        {
            newHand.GetComponent<Image>().sprite = buttons_images[index].GetComponent<Image>().sprite;
            if (LoadPhoton.afterGotGameObject)
                LoadPhoton.playerScript.GetComponent<playerController>().changeClothes(3, buttons_images[index].GetComponent<Image>().sprite.name);
        }

        else if (buttons_images[index].tag == "Background")
        {
            if (buttons_images[index].GetComponent<Image>().sprite.name == "empty")
            {
                newBackground.GetComponent<Image>().sprite = Resources.Load<Sprite>("blank_bg");
            }
            else
            {
                newBackground.GetComponent<Image>().sprite = buttons_images[index].GetComponent<Image>().sprite;
            }
        }
        onClothesChange();
    }

    public void onItemButtonClick0()
    {
        onItemButtonClick(0);
    }

    public void onItemButtonClick1()
    {
        onItemButtonClick(1);
    }

    public void onItemButtonClick2()
    {
        onItemButtonClick(2);
    }

    public void onItemButtonClick3()
    {
        onItemButtonClick(3);
    }

    public void onItemButtonClick4()
    {
        onItemButtonClick(4);
    }

    public void onItemButtonClick5()
    {
        onItemButtonClick(5);
    }

    public void onItemButtonClick6()
    {
        onItemButtonClick(6);
    }

    public void onItemButtonClick7()
    {
        onItemButtonClick(7);
    }

    public void onItemButtonClick8()
    {
        onItemButtonClick(8);
    }

    public void onItemButtonClick9()
    {
        onItemButtonClick(9);
    }

    public void onItemButtonClick10()
    {
        onItemButtonClick(10);
    }

    public void onItemButtonClick11()
    {
        onItemButtonClick(11);
    }
}