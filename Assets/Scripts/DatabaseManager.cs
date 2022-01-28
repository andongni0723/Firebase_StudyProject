using Firebase.Database;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class DatabaseManager : MonoBehaviour
{
    public TMP_InputField name_IF;
    public TMP_InputField gold_IF;

    public TextMeshProUGUI name_T;
    public TextMeshProUGUI gold_T;

    private string userID;
    private DatabaseReference databaseReference;

    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateUser()
    {
        User newUser = new User(name_IF.text, int.Parse(gold_IF.text));
        string json = JsonUtility.ToJson(newUser);

        databaseReference.Child("users").Child(userID).SetRawJsonValueAsync(json);

    }

    public void UpdateName()
    {
        databaseReference.Child("users").Child(userID).Child("name").SetValueAsync(name_IF.text);
    }

    public void UpdateGold()
    {
        databaseReference.Child("users").Child(userID).Child("gold").SetValueAsync(gold_IF.text);
    }


    ///// Get Data /////
    
    public void GetUserInfo()
    {
        StartCoroutine(GetName((string name) =>
        {
            name_T.text = "Name :" +  name;
        }));

        StartCoroutine(GetGold((int gold) =>
        {
            gold_T.text = "Gold :" +  gold.ToString();
        }));
    }
    

    public IEnumerator GetName(Action<string> onCalkback)
    {
        var userNameData = databaseReference.Child("users").Child(userID).Child("name").GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if(userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;

            onCalkback.Invoke(snapshot.Value.ToString());
        }
    }

    public IEnumerator GetGold(Action<int> onCalkback)
    {
        var userGoldData = databaseReference.Child("users").Child(userID).Child("gold").GetValueAsync();

        yield return new WaitUntil(predicate: () => userGoldData.IsCompleted);

        if (userGoldData != null)
        {
            DataSnapshot snapshot = userGoldData.Result;

            onCalkback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }
}
