using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public delegate void OnEnemyKilled(EnemyBase enemy);
    public static event OnEnemyKilled onEnemyKilled;

    public List<Quest> quests;
    public List<Quest> activeQuest;

    
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
}
