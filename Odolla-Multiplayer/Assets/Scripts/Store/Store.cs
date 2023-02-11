using Firebase.Auth;
using Firebase.Firestore;
using Google.MiniJSON;
using Mono.Cecil.Cil;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;

public class Store : MonoBehaviour
{
    // Reference to the Firebase Firestore
    private FirebaseFirestore db;

    // Selected item details
    public string selected_item;
    public int selected_item_price;
    public string selected_item_heb;

    // players property
    List<string> MyHairList = new List<string>();
    List<string> MyHatsList = new List<string>();
    List<string> MyShirtsList = new List<string>();
    List<string> MyPantsList = new List<string>();
    List<string> MyHandsList = new List<string>(); 
    List<string> MyBackgroundList = new List<string>();

    // store property
    List<Item> HairList = new List<Item>();
    List<Item> HatsList = new List<Item>();
    List<Item> ShirtsList = new List<Item>();
    List<Item> PantsList = new List<Item>();
    List<Item> HandsList = new List<Item>();
    List<Item> BackgroundList = new List<Item>();

    public GameObject confirm;
    public GameObject popup;
    public TMP_Text popup_text;
    public TMP_Text page_text;
    public Button arrow_right;
    public Button arrow_left;
    public int playerCoins = -1;
    public Text money;
    public string store = "store_1";
    public GameObject canvas;
    public GameObject[] buttons;
    public TMP_Text[] price_tag;
    public GameObject[] buttons_images;
    public int page = 0;
    public string category = "";

    // property tabs
    public GameObject hair_tab;
    public GameObject hat_tab;
    public GameObject shirt_tab;
    public GameObject pants_tab;
    public GameObject hand_tab;
    public GameObject background_tab;

    // property tabs images
    public GameObject newHat;
    public GameObject newHair;
    public GameObject newShirt;
    public GameObject newPants;
    public GameObject newHand;
    public GameObject newBackground;

    async void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        buttons_images = new GameObject[buttons.Length];
        for(int i=0; i<12; i++)
            buttons_images[i] = buttons[i].transform.GetChild(0).gameObject;
    }

    void Update()
    {
        page_text.text = (page + 1) + "";
        if (page == 0)
            arrow_left.interactable = false;
        else
            arrow_left.interactable = true;

        // Disable arrows, when it needs to be disabled
        if (category == OnSceneSpawn.HAIR_CATEGORY)
            setRightArrowInteractableStatus(HairList);
        else if (category == OnSceneSpawn.HATS_CATEGORY)
            setRightArrowInteractableStatus(HatsList);
        else if (category == OnSceneSpawn.SHIRTS_CATEGORY)
            setRightArrowInteractableStatus(ShirtsList);
        else if (category == OnSceneSpawn.PANTS_CATEGORY)
            setRightArrowInteractableStatus(PantsList);
        else if (category == OnSceneSpawn.HANDS_CATEGORY)
            setRightArrowInteractableStatus(HandsList);
        else if (category == OnSceneSpawn.BACKGROUND_CATEGORY)
            setRightArrowInteractableStatus(BackgroundList);
    }

    public void setRightArrowInteractableStatus(List<Item> list)
    {
        if (page + 1 >= Math.Ceiling((double)list.Count / 12))
            arrow_right.interactable = false;
        else
            arrow_right.interactable = true;
    }

    public void rightArrowPressed(List<Item> list)
    {
        if (page + 1 >= Math.Ceiling((double)HairList.Count / 12))
            arrow_right.interactable = false;
        else
            page += 1;
    }

    public void onArrowRight()
    {
        clearAllCells();
        if (category == OnSceneSpawn.HAIR_CATEGORY)
            rightArrowPressed(HairList);
        else if (category == OnSceneSpawn.HATS_CATEGORY)
            rightArrowPressed(HatsList);
        else if (category == OnSceneSpawn.SHIRTS_CATEGORY)
            rightArrowPressed(ShirtsList);
        else if (category == OnSceneSpawn.PANTS_CATEGORY)
            rightArrowPressed(PantsList);
        else if (category == OnSceneSpawn.HANDS_CATEGORY)
            rightArrowPressed(HandsList);
        else if (category == OnSceneSpawn.BACKGROUND_CATEGORY)
            rightArrowPressed(BackgroundList);
        orgenizeStuffOnGrid();
    }

    public void orgenizeStuffOnGrid()
    {
        if (category == OnSceneSpawn.HAIR_CATEGORY)
        {
            for (int i = 0; i < 12; i++)
            {
                if (HairList.Count > (i + (page * 12)))
                {
                    buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(HairList[i + (page * 12)].getItem_name());
                    buttons_images[i].tag = "Hair";
                    price_tag[i].text = HairList[i + (page * 12)].getItem_price().ToString();
                }
            }
        }
        else if (category == OnSceneSpawn.HATS_CATEGORY)
        {
            for (int i = 0; i < 12; i++)
            {
                if (HatsList.Count > (i + (page * 12)))
                {
                    buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(HatsList[i + (page * 12)].getItem_name());
                    buttons_images[i].tag = "Hat";
                    price_tag[i].text = HatsList[i + (page * 12)].getItem_price().ToString();
                }
            }
        }

        else if (category == OnSceneSpawn.SHIRTS_CATEGORY)
        {
            for (int i = 0; i < 12; i++)
            {
                if (ShirtsList.Count > (i + (page * 12)))
                {
                    buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(ShirtsList[i + (page * 12)].getItem_name());
                    buttons_images[i].tag = "Shirt";
                    price_tag[i].text = ShirtsList[i + (page * 12)].getItem_price().ToString();
                }
            }
        }
        else if (category == OnSceneSpawn.PANTS_CATEGORY)
        {
            for (int i = 0; i < 12; i++)
            {
                if (PantsList.Count > (i + (page * 12)))
                {
                    buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(PantsList[i + (page * 12)].getItem_name());
                    buttons_images[i].tag = "Pants";
                    price_tag[i].text = PantsList[i + (page * 12)].getItem_price().ToString();
                }
            }
        }
        else if (category == OnSceneSpawn.HANDS_CATEGORY)
        {
            for (int i = 0; i < 12; i++)
            {
                if (HandsList.Count > (i + (page * 12)))
                {
                    buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(HandsList[i + (page * 12)].getItem_name());
                    buttons_images[i].tag = "Hand";
                    price_tag[i].text = HandsList[i + (page * 12)].getItem_price().ToString();
                }
            }
        }
        else if (category == OnSceneSpawn.BACKGROUND_CATEGORY)
        {
            for (int i = 0; i < 12; i++)
            {
                if (BackgroundList.Count > (i + (page * 12)))
                {
                    buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(BackgroundList[i + (page * 12)].getItem_name());
                    buttons_images[i].tag = "Background";
                    price_tag[i].text = BackgroundList[i + (page * 12)].getItem_price().ToString();
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

    public void closeWindow()
    {
        canvas.SetActive(false);
        OnSceneSpawn.isFocus = false;
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
            buttons_images[i].tag = tag;
            cleanText();
        }
    }

    public async void onStoreClick()
    {
        if (OnSceneSpawn.isFocus == false)
        {
            // open the store window
            canvas.SetActive(true);
            fixClothesOnGameCard();
            OnSceneSpawn.isFocus = true;
            page = 0;
            category = "hair";
            cleanText();
             
            // change the tabs to default
            hair_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright_store");
            hat_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");
            shirt_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");
            pants_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");
            hand_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");
            background_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");

            // get the player's coins
            playerCoins = await Main.Instance.FirebaseHelper.GetCoins();
            money.text = playerCoins.ToString();

            // get the stores items
            await getItemsFromStore();
            orgenizeItems();

            // get the player's items
            await getPlayersProperty();
        }
    }

    public void fixClothesOnGameCard()
    {
        newHair.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
        newHat.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
        newShirt.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite;
        newHand.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite;
        newPants.GetComponent<Image>().sprite = LoadPhoton.playerScript.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().sprite;
        newBackground.GetComponent<Image>().sprite = Resources.Load<Sprite>("player_place_store");
    }

    public void changeCategoryDefault()
    {
        // change the tabs to default
        hair_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");
        hat_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");
        shirt_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");
        hand_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");
        background_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");
        pants_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_dark_store");

        // other stuff to update
        cleanText();
        arrow_right.interactable = true;
        page = 0;
        orgenizeItems();
    }

    public void ChangeCategoryToHair()
    {
        hair_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright_store");
        category = "hair";
        changeCategoryDefault();
    }

    public void ChangeCategoryToHats()
    {
        hat_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright_store");
        category = "hats";
        changeCategoryDefault();
    }

    public void ChangeCategoryToShirts()
    {
        shirt_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright_store");
        category = "shirts";
        changeCategoryDefault();
    }

    public void ChangeCategoryToHands()
    {
        hand_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright_store");
        category = "hands";
        changeCategoryDefault();
    }

    public void ChangeCategoryToBackgrounds()
    {
        background_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright_store");
        category = "background";
        changeCategoryDefault();
    }

    public void ChangeCategoryToPants()
    {
        pants_tab.GetComponent<Image>().sprite = Resources.Load<Sprite>("text_tab_inv_bright_store");
        category = "pants";
        changeCategoryDefault();
    }

    public async Task getItemsFromStore()
    {
        DocumentReference docRef = db.Collection("stores_items").Document("store_1");
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
         
        // get the items from the store
        HairList = snapshot.GetValue<Dictionary<string, Item>>("hair").Values.ToList();
        HatsList = snapshot.GetValue<Dictionary<string, Item>>("hats").Values.ToList();
        ShirtsList = snapshot.GetValue<Dictionary<string, Item>>("shirts").Values.ToList();
        PantsList = snapshot.GetValue<Dictionary<string, Item>>("pants").Values.ToList();
        HandsList = snapshot.GetValue<Dictionary<string, Item>>("hands").Values.ToList();
        BackgroundList = snapshot.GetValue<Dictionary<string, Item>>("backgrounds").Values.ToList();
    }

    public void orgenizeItems()
    {
        clearAllCells();
        String tag = getTagByCategory();
        List<Item> items = new List<Item>();

        if (category == OnSceneSpawn.HANDS_CATEGORY)
        {
            items = HandsList;
        }
        else if (category == OnSceneSpawn.HAIR_CATEGORY)
        {
            items = HairList;
        }
        else if (category == OnSceneSpawn.HATS_CATEGORY)
        {
            items = HatsList;
        }
        else if (category == OnSceneSpawn.SHIRTS_CATEGORY)
        {
            items = ShirtsList;
        }
        else if (category == OnSceneSpawn.PANTS_CATEGORY)
        {
            items = PantsList;
        }
        else if (category == OnSceneSpawn.BACKGROUND_CATEGORY)
        {
            items = BackgroundList;
        }

        int index = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if (index <= 11)
            {
                buttons_images[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(items[i].getItem_name());
                buttons_images[i].tag = tag;
                price_tag[i].text = items[i].getItem_price().ToString();
                index++;
            }
        }
    }

    public void cleanText()
    {
        for (int i = 0; i < 12; i++)
        {
            price_tag[i].text = "";
        }
    }

    public Boolean isInList(String selected_item, List<string> list)
    {
        bool flag = false;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == selected_item)
            {
                flag = true;
            }
        }

        return flag;
    }

    public void gettingItem(int i)
    {
        if(category == "hats")
        {
            if (HatsList.Count > ((page * 12) + i))
            {
                selected_item = HatsList[page * 12 + i].getItem_name();
                selected_item_price = int.Parse(HatsList[page * 12 + i].getItem_price());
                selected_item_heb = HatsList[page * 12 + i].getHebrew_name();

                if (!isInList(selected_item, MyHatsList))
                    showAlert();
                else
                    showAlertIfItemExist();
            }
        } 
        else if(category == "hair")
        {
            if (HairList.Count > ((page * 12) + i)) {
                selected_item = HairList[page * 12 + i].getItem_name();
                selected_item_price = int.Parse(HairList[page * 12 + i].getItem_price());
                selected_item_heb = HairList[page * 12 + i].getHebrew_name();

                if (!isInList(selected_item, MyHairList))
                    showAlert();
                else
                    showAlertIfItemExist();
            }
        }
        else if (category == "hands")
        {
            if (HandsList.Count > ((page * 12) + i))
            {
                selected_item = HandsList[page * 12 + i].getItem_name();
                selected_item_price = int.Parse(HandsList[page * 12 + i].getItem_price());
                selected_item_heb = HandsList[page * 12 + i].getHebrew_name();

                if (!isInList(selected_item, MyHandsList))
                    showAlert();
                else
                    showAlertIfItemExist();
            }
        }
        else if (category == "shirts")
        {
            if (ShirtsList.Count > ((page * 12) + i))
            {
                selected_item = ShirtsList[page * 12 + i].getItem_name();
                selected_item_price = int.Parse(ShirtsList[page * 12 + i].getItem_price());
                selected_item_heb = ShirtsList[page * 12 + i].getHebrew_name();

                if (!isInList(selected_item, MyShirtsList))
                    showAlert();
                else
                    showAlertIfItemExist();
            }
        }
        else if (category == "pants")
        {
            if (PantsList.Count > ((page * 12) + i))
            {
                selected_item = PantsList[page * 12 + i].getItem_name();
                selected_item_price = int.Parse(PantsList[page * 12 + i].getItem_price());
                selected_item_heb = PantsList[page * 12 + i].getHebrew_name();

                if (!isInList(selected_item, MyPantsList))
                    showAlert();
                else
                    showAlertIfItemExist();
            }
        }
        else if (category == "background")
        {
            if (BackgroundList.Count > ((page * 12) + i))
            {
                selected_item = BackgroundList[page * 12 + i].getItem_name();
                selected_item_price = int.Parse(BackgroundList[page * 12 + i].getItem_price());
                selected_item_heb = BackgroundList[page * 12 + i].getHebrew_name();

                if (!isInList(selected_item, MyBackgroundList))
                    showAlert();
                else
                    showAlertIfItemExist();
            }
        }
    }

    public void showAlert() {
        confirm.SetActive(true);
        popup_text.text = "האם ברצונך לרכוש את הפריט: " + selected_item_heb + " תמורת " + Reverse(selected_item_price.ToString()) + " מטבעות אודולר";
        popup.SetActive(true);
    }

    public void showAlertIfItemExist()
    {
        confirm.SetActive(false);
        popup_text.text = "יש ברשותך את הפריט";
        popup.SetActive(true);
    }

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public void closePopup()
    {
        popup.SetActive(false);
        selected_item = "";
        selected_item_price = 0;
        selected_item_heb = "";
    }

    public void onItemButtonClick0()
    {
        gettingItem(0);
    }

    public void onItemButtonClick1()
    {
        gettingItem(1);
    }

    public void onItemButtonClick2()
    {
        gettingItem(2);
    }

    public void onItemButtonClick3()
    {
        gettingItem(3);
    }

    public void onItemButtonClick4()
    {
        gettingItem(4);
    }
    public void onItemButtonClick5()
    {
        gettingItem(5);
    }
    public void onItemButtonClick6()
    {
        gettingItem(6);
    }

    public void onItemButtonClick7()
    {
        gettingItem(7);
    }

    public void onItemButtonClick8()
    {
        gettingItem(8);
    }

    public void onItemButtonClick9()
    {
        gettingItem(9);
    }

    public void onItemButtonClick10()
    {
        gettingItem(10);
    }

    public void onItemButtonClick11()
    {
        gettingItem(11);
    }

    public void popup_confirm()
    {
        if (playerCoins < selected_item_price) {
            popup_text.text = "אין ברשותך מספיק מטבעות על מנת לבצע את הרכישה";
        }
        else
        {
            purchaseItem();
        }
    }

    public async void purchaseItem()
    {
        // Get the current user ID
        string displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        DocumentReference docRef = db.Collection("users").Document(displayName);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        playerCoins = await Main.Instance.FirebaseHelper.GetCoins();
        if (playerCoins - selected_item_price >= 0)
        {
            // Update the user's money
            playerCoins -= selected_item_price;
            money.text = playerCoins.ToString();

            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "coins", playerCoins }
            };
            await docRef.UpdateAsync(user);

            // Update the user's property to default if not exists
            if(!snapshot.ContainsField("property"))
            {
                Dictionary<string, object> property = new Dictionary<string, object>
                {
                    { "hats", new List<string>() },
                    { "hands", new List<string>() },
                    { "hair", new List<string>() },
                    { "shirts", new List<string>() },
                    { "pants", new List<string>() },
                    { "background", new List<string>() }
                };

                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "property", property }
                };
                await docRef.SetAsync(data, SetOptions.MergeAll);
            }

            // Add item to property
            Dictionary<string, object> propertyUpdate = new Dictionary<string, object>
            {
               { "property." + category, FieldValue.ArrayUnion(selected_item) }
            };
            await docRef.UpdateAsync(propertyUpdate);
            closePopup();
        }
        else
        {
            popup_text.text = "אין ברשותך מספיק מטבעות על מנת לבצע את הרכישה";
        }

        await getPlayersProperty();
        await Main.Instance.FirebaseHelper.GetCoins();
    }

    public async Task getPlayersProperty()
    {
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
                    MyHairList = hairList.Cast<string>().ToList();
                }
            }
            if (property.ContainsKey("hats"))
            {
                object hatsValue = property["hats"];
                if (hatsValue is List<object> hatsList)
                {
                    MyHatsList = hatsList.Cast<string>().ToList();
                }
            }
            if (property.ContainsKey("shirts"))
            {
                object shirtsValue = property["shirts"];
                if (shirtsValue is List<object> shirtsList)
                {
                    MyShirtsList = shirtsList.Cast<string>().ToList();
                }
            }
            if (property.ContainsKey("pants"))
            {
                object pantsValue = property["pants"];
                if (pantsValue is List<object> pantsList)
                {
                    MyPantsList = pantsList.Cast<string>().ToList();
                }
            }
            if (property.ContainsKey("hands"))
            {
                object handsValue = property["hands"];
                if (handsValue is List<object> handsList)
                {
                    MyHandsList = handsList.Cast<string>().ToList();
                }
            }
            if (property.ContainsKey("backgrounds"))
            {
                object backgroundsValue = property["backgrounds"];
                if (backgroundsValue is List<object> backgroundsList)
                {
                    MyBackgroundList = backgroundsList.Cast<string>().ToList();
                }
            }
        }
    }
}