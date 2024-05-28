using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using MBExtensions;

public class SmartTooltips : MonoBehaviour
{
    public static SmartTooltips Instance;

    public UserStatsList userStatsList;
    public class UserStatsList
    {
        public List<UserStats> list = new();
        //public UserStats[] listo;

        public UserStatsList()
        {
            //this.list = new UserStats[2];
            list.Add(new("ElTesto", DateTime.Now));
            //list.Add(new("popool", DateTime.Now));
            //list.Add(new("popollololo", DateTime.Now));
        }

        public bool IsNewUser(string username)
        {
            foreach(UserStats stat in list)
            {
                if (username is string && stat.Username.ToLower().Contains(username.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }
    }

    [System.Serializable]
    public class UserStats
    {
        public string Username;
        public long dateTime;

        public UserStats(string username, DateTime dateTime)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            this.dateTime = dateTime.ToFileTime();
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
        TooManyFuncts.Singletonize(ref Instance, this);
        saveFile = Application.dataPath + "/UserData.json";
        Debug.Log(saveFile);
    }

    // Start is called before the first frame update
    void Start()
    {
        UserStatsStorageInit();
        //UserStatsSaveTest();
    }

    public void UserMsgReceived(string username, string msg)
    {
        bool isNew = userStatsList.IsNewUser(username);
        Debug.Log("New User: " + isNew);
        if(isNew == false)
        {
            return;
        }
        else
        {
            var newUser = new UserStats(username, DateTime.Now);
            userStatsList.list.Add(newUser);
            Save(userStatsList);
            PrintWelcomeMsg(username);

        }
    }
    public GameObject TempReferenceToUnderlyingTooltipBox;
    public void PrintWelcomeMsg(string name)
    {
        TooManyFuncts.GetComponentInChildrenParametric<TextMeshProUGUI>(transform.GetChild(0), "Username", null, null).text = '\n' + name;
        transform.GetChild(0).gameObject.SetActive(true);
        TempReferenceToUnderlyingTooltipBox.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(this.DelayedExecution(ReenableUnderlying, 5.0f));
        void ReenableUnderlying()
        {
            TempReferenceToUnderlyingTooltipBox.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void UserStatsStorageInit()
    {
        userStatsList = new();
        Load(ref userStatsList);
        //if (userStatsList is null)
        //    userStatsList = new();
        Debug.Log(userStatsList.list.ToString());
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

            var newObj = JsonUtility.FromJson<Type>(jsonString);
            if (newObj is Type)
                obj = newObj;
        }
    }

    public void UserStatsSaveTest() {
        UserStats testUser = new("Testomann66", DateTime.Now);
        //Debug.LogWarning(testUser.dateTime.ToString());
        UserStatsList userStatsList = new();
        userStatsList.list[0] = testUser;
        Save(userStatsList);

        Load(ref userStatsList);
        Debug.LogWarning(userStatsList.list[0].ReadDateTime());
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

/*
 * using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SmartTooltips : MonoBehaviour
{
    public static SmartTooltips Instance;

    public UserStatsList userStatsList;
    [System.Serializable]
    public class UserStatsList
    {
        [SerializeReference]
        public List<UserStats> list;
        public UserStats[] saveArray;

        public UserStatsList()
        {
            //this.list = new UserStats[2];
            this.list = new();
        }

        public bool IsNewUser(string username)
        {
            foreach(UserStats stat in list)
            {
                if (username is string && stat.Username.ToLower().Contains(username.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }
    }

    [System.Serializable]
    public class UserStats
    {
        public string Username;
        public long dateTime;

        public UserStats(string username, DateTime dateTime)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            this.dateTime = dateTime.ToFileTime();
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
        TooManyFuncts.Singletonize(ref Instance, this);
        saveFile = Application.dataPath + "/UserData.json";
        Debug.Log(saveFile);
    }

    // Start is called before the first frame update
    void Start()
    {
        //UserStatsStorageInit();
        UserStatsSaveTest();
    }

    public void UserMsgReceived(string username, string msg)
    {
        bool isNew = userStatsList.IsNewUser(username);
        Debug.Log("New User: " + isNew);
        if(isNew == false)
        {
            return;
        }
        else
        {
            var newUser = new UserStats(username, DateTime.Now);
            userStatsList.list.Add(newUser);
        }
    }

    public void UserStatsStorageInit()
    {
        userStatsList = new();
        LoadUserData(ref userStatsList);
    }

    public void SaveUserData(UserStatsList userStatsList)
    {
        userStatsList.saveArray = userStatsList.list.ToArray();
        Save(userStatsList);
    }

    public void LoadUserData(ref UserStatsList userStatsList)
    {
        Load(ref userStatsList);
        userStatsList.list = new List<UserStats>(userStatsList.saveArray);
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

    public void UserStatsSaveTest() {
        UserStats testUser = new("Testomann66", DateTime.Now);
        //Debug.LogWarning(testUser.dateTime.ToString());
        UserStatsList userStatsList = new();
        userStatsList.list[0] = testUser;
        SaveUserData(userStatsList);

        LoadUserData(ref userStatsList);
        Debug.LogWarning(userStatsList.list[0].ReadDateTime());
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
*/
