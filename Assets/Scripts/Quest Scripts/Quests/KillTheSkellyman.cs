using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTheSkellyman : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        state = QuestState.Unknown;
        botched = false;

        mustBeTurnedIn = false;

        questName = "Kill The Skellyman";
        description = "Find and defeate the dreaded Skellyman";
        mainObj = "Kill the skellyman";
        location = "Upper Section";
        questGiver = "Tom";

        goals.Add(new KillGoal(QuestManager.instance.enemyIds["Skellyman"], mainObj, false, 0, 1));

        QuestManager.instance.quests.Add(this);
        QuestManager.instance.questIds.Add(questName, this);

        setState(QuestState.Accepted);
    }
}
