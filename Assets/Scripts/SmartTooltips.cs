using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SmartTooltips : MonoBehaviour
{
    public class UserStats
    {
        public string Username;
        public long dateTime;

        public UserStats(string username, long dateTime)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            this.dateTime = dateTime;
        }

        public DateTime ReadDateTime()
        {
            return DateTime.FromFileTime(dateTime);
        }

        // Turns DateTime into Serializable Data for Json save
        public void WriteDateTime(DateTime dateTimeReadable)
        {
            dateTime = dateTimeReadable.ToFileTime();
        }
    }

    string saveFile;
    private void Awake()
    {
        saveFile = Application.dataPath + "/UserData.json";
        Debug.Log(saveFile);
    }

    // Start is called before the first frame update
    void Start()
    {
        

        UserStats testUser = new("Testomann66", DateTime.Now.ToFileTime());
        Debug.LogWarning(testUser.dateTime.ToString());
        Save(testUser);

        Load(ref testUser);
        Debug.LogWarning(testUser.ReadDateTime());
    }

    public void Save(object obj)
    {
        string jsonString = JsonUtility.ToJson(obj);
        File.WriteAllText(saveFile, jsonString);
    }

    public void Load<Type>(ref Type obj)
    {
        if (File.Exists(saveFile))
        {
            string jsonString = File.ReadAllText(saveFile);

            obj = JsonUtility.FromJson<Type>(jsonString);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
