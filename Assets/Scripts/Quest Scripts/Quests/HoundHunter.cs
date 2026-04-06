using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoundHunter : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        state = QuestState.Unknown;
        botched = false;

        mustBeTurnedIn = false;

        questName = "Hound Hunter";
        description = "Kill 5 hounds";
        mainObj = "Kill 5 hounds";
        questGiver = "Tom";

        goals.Add(new KillGoal(QuestManager.instance.enemyIds["Hound"], mainObj, false, 0, 5));

        QuestManager.instance.quests.Add(this);
        QuestManager.instance.questIds.Add(questName, this);

        setState(QuestState.Accepted);
    }
}
