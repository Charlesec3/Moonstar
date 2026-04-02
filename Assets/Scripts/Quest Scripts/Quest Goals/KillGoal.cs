using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : QuestGoal
{
    public int enemyID;

    public KillGoal(int enemyID, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.enemyID = enemyID;
        this.description = description;
        this.completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = requiredAmount;
    }

    public override void initialize()
    {
       QuestManager.onEnemyKilled += enemyDied;
    }

    void enemyDied(EnemyBase e)
    {
        if(e.enemyID == enemyID)
        {
            increaseCurrentAmount(1);
        }
    }
}
