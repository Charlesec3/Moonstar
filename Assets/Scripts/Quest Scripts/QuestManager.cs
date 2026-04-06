using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public delegate void OnEnemyKilled(EnemyBase enemy);
    public static event OnEnemyKilled onEnemyKilled;

    public List<Quest> quests;
    public List<Quest> activeQuest;

    public List<string> questSaveData;
    public string questSaveDataString;

    [SerializeField] private QuestLoader[] loaders;

    public TextMeshProUGUI questName;
    public TextMeshProUGUI questGiver;
    public TextMeshProUGUI questLocation;

    public TextMeshProUGUI questDescription;

    public TextMeshProUGUI pgDisplay;

    public int currentPg;
    [SerializeField] private int totalPg;


    
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


    public void openQuestMenu()
    {
        for (int i = 0; i < loaders.Length; i++)
        {
            loaders[i].clearButton();
        }

        questName.text = "";
        questGiver.text = "";
        questLocation.text = "";
        questDescription.text = "";


        currentPg = 1;

        if(activeQuest.Count > loaders.Length)
        {
            totalPg = activeQuest.Count / loaders.Length;

            if (activeQuest.Count % loaders.Length != 0)
            {
                ++totalPg;
            }
        }
        else
        {
            totalPg = 1;
        }

        pgDisplay.text = currentPg.ToString() + " / " + totalPg.ToString();

        setLoaders();
    }

    public void setLoaders()
    {
        if(activeQuest.Count > 0)
        {
            for (int i = 0; i < loaders.Length; i++)
            {
                loaders[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < loaders.Length; i++)
            {
                loaders[i].clearButton();
            }

            int startIndex = (currentPg - 1) * loaders.Length;
            int loadNum = 0;

            for (int i = startIndex; loadNum < loaders.Length; i++)
            {
                if (i > activeQuest.Count - 1)
                {
                    break;
                }
                else
                {
                    loaders[loadNum].quest = activeQuest[i];
                    loaders[loadNum].setButton();
                    ++loadNum;
                }
            }  
        }

        for (int i = 0; i < loaders.Length; i++)
        {
            if(loaders[i].quest == null)
            { 
                loaders[i].gameObject.SetActive(false);
            }
        }  
    } 

    public void pressRightBtn()
    {
        if(currentPg < totalPg)
        {
            ++currentPg;

            setLoaders();

            pgDisplay.text = currentPg + " / " + totalPg;
        }
    }

    public void pressLeftBtn()
    {
        if (currentPg > 1)
        {
            --currentPg;

            setLoaders();

            pgDisplay.text = currentPg + " / " + totalPg;
        }
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

            foreach(Quest value in quests)
            {
                if(value.state == Quest.QuestState.Accepted)
                {
                    activeQuest.Add(value);
                }
            }
        }
    }
}
