using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Firestore;
using System.Threading.Tasks;
using Newtonsoft.Json;
public class FirebaseHelper : MonoBehaviour
{
    // Reference to the Firebase Firestore
    private FirebaseFirestore db;

    public Text money;
    public Text level;

    async void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public async Task<int> GetCoins()
    {
        string displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        DocumentReference docRef = db.Collection("users").Document(displayName);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.ContainsField("coins"))
        {
            int coins = snapshot.GetValue<int>("coins");
            return coins;
        }
        else
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "coins", 0 }
            };
            await docRef.SetAsync(data, SetOptions.MergeAll);

            snapshot = await docRef.GetSnapshotAsync();
            int coins = snapshot.GetValue<int>("coins");
            return coins;
        }
    }

    public async Task<string> GetOutfit(string playerDisplayName)
    {
        DocumentReference docRef = db.Collection("users").Document(playerDisplayName);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.ContainsField("outfit"))
        {
            Dictionary<string, object> outfit = snapshot.GetValue<Dictionary<string, object>>("outfit");
            string json = JsonConvert.SerializeObject(outfit);
            return json;
        }
        else
        {
            Dictionary<string, object> outfit = new Dictionary<string, object>
            {
                { "hat", "empty" },
                { "hair", "empty" },
                { "shirt", "empty" },
                { "pants", "empty" },
                { "hands", "empty" },
                { "background", "blank_bg" }
            };

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "outfit", outfit }
            };
            await docRef.SetAsync(data, SetOptions.MergeAll);

            snapshot = await docRef.GetSnapshotAsync();
            outfit = snapshot.GetValue<Dictionary<string, object>>("outfit");
            string json = JsonConvert.SerializeObject(outfit);

            return json;
        }
    }

    public async Task setOutfit(User user)
    {
        // Get the current user ID
        string displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        DocumentReference docRef = db.Collection("users").Document(displayName);

        // Create the "outfit" field with default values if it doesn't exist
        Dictionary<string, object> outfitJson = new Dictionary<string, object>
        {
            { "hat", user.GetHat() },
            { "hair", user.GetHair() },
            { "shirt", user.GetShirt() },
            { "pants", user.GetPants() },
            { "hands", user.GetHand() },
            { "background", user.GetBackgronud() }
        };

        // Create the "outfit" field with default values if it doesn't exist
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "outfit", outfitJson }
        };
        await docRef.SetAsync(data, SetOptions.MergeAll);
    }

    public async Task UpdateUsersCoins(int updateAmount)
    {
        string displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        DocumentReference docRef = db.Collection("users").Document(displayName);
        int currentCoins = await GetCoins();

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "coins", currentCoins + updateAmount }
        };

        await docRef.SetAsync(updates, SetOptions.MergeAll);
    }

    public async Task<int> GetLevel()
    {
        string displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        DocumentReference docRef = db.Collection("users").Document(displayName);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.ContainsField("level"))
        {
            int level = snapshot.GetValue<int>("level");
            return level;
        }
        else
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "level", 0 }
            };
            await docRef.SetAsync(data, SetOptions.MergeAll);

            snapshot = await docRef.GetSnapshotAsync();
            int level = snapshot.GetValue<int>("level");
            return level;
        }
    }

    public async Task<bool> GetManagerState()
    {
        string displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        DocumentReference docRef = db.Collection("users").Document(displayName);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.ContainsField("manager"))
        {
            int manager = snapshot.GetValue<int>("manager");
            return !(manager == 0);
        }
        else
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "manager", 0 }
            };
            await docRef.SetAsync(data, SetOptions.MergeAll);

            snapshot = await docRef.GetSnapshotAsync();
            int manager = snapshot.GetValue<int>("manager");
            return !(manager == 0);
        }
    }
}