using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class QuestGoal
{
    public string description;

    public bool completed;

    public int currentAmount;
    public int requiredAmount;

    public string progress;


    public virtual void initialize()
    {
        
    }
    
    public void evaluate()
    {
        completed = currentAmount >= requiredAmount;
    }

    public void increaseCurrentAmount(int increaseBy)
    {
        currentAmount += increaseBy;

        progress = "(" + currentAmount + "/" + requiredAmount + ")";
        
        evaluate();
    }
}
