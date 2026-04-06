using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public delegate void OnEnemyKilled(EnemyBase enemy);
    public static event OnEnemyKilled onEnemyKilled;

    public List<Quest> quests;
    public List<Quest> activeQuest;

    public List<string> questSaveData;
    public string questSaveDataString;

    
    public Dictionary<string, int> enemyIds = new Dictionary<string, int>
    {
        ["Glider"] = 111,
        ["Bomber"] = 121,
        ["Diver"] = 122,
        ["Spitter"] = 123,
        ["Tracer"] = 131,
        ["Cannon"] = 211,
        ["Shooter"] = 212,
        ["Hopper"] = 221,
        ["Roller"] = 222,
        ["Hound"] = 231,
        ["Knight"] = 232,
        ["Skellyman"] = 233
    };

    public Dictionary<string, Quest> questIds = new Dictionary<string, Quest>();

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void enemyDied(EnemyBase e)
    {
        if(onEnemyKilled != null)
        {
            onEnemyKilled(e);
        }
    }



    private class QuestManagerData
    {
        public List<string> questSaveData;

        public QuestManagerData(List<string> questSaveData)
        {
            this.questSaveData = questSaveData;
        }

        public void setData(ref List<string> questSaveData)
        {
            questSaveData = this.questSaveData;
        }
    }

    public void saveQuestData()
    {
        questSaveData.Clear();
        for(int i = 0; i < quests.Count; i++)
        {
            questSaveData.Add(quests[i].saveQuest());
        }

        QuestManagerData q1 = new QuestManagerData(questSaveData);
        questSaveDataString = JsonUtility.ToJson(q1);

        File.WriteAllText(Application.persistentDataPath + "/QuestData.json", questSaveDataString);
    }

    public void loadQuestData()
    {
        if (File.Exists(Application.persistentDataPath + "/QuestData.json") == true)
        {
            questSaveDataString = File.ReadAllText(Application.persistentDataPath + "/QuestData.json");
            QuestManagerData q1 = JsonUtility.FromJson<QuestManagerData>(questSaveDataString);

            q1.setData(ref questSaveData);

            for(int i = 0; i < questSaveData.Count; i++)
            {
                QuestData qd1 = JsonUtility.FromJson<QuestData>(questSaveData[i]);
                questIds[qd1.questName].loadQuest(qd1);
            }
        }
    }
}
