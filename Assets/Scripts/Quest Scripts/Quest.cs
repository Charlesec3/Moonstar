using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.Linq;
using System.IO;

[Serializable]
public class Quest : MonoBehaviour
{
    public enum QuestState {Unknown, Mentioned, Accepted, Achieved, Completed};
    public QuestState state;

    public bool botched;

    [SerializeField] protected bool mustBeTurnedIn;

    public List<QuestGoal> goals = new List<QuestGoal>();

    public string questName;
    public string description;
    public string mainObj; 
    public string location;
    public string questGiver;


    void Start()
    {
        QuestManager.instance.questIds.Add(questName, this);
    } 

    public void checkGoals()
    {
        if(goals.All(g => g.completed) == true)
        {
            if(mustBeTurnedIn == true)
            {
                state = QuestState.Achieved;
            }
            else
            {
                state = QuestState.Completed;

                //give reward
                
            }
        }
    }



    public void setState(QuestState newState)
    {
       if(state != QuestState.Completed)
        {
            if((int)newState > (int)state)
            {
                state = newState;

                if(newState == QuestState.Accepted)
                {
                    foreach(QuestGoal value in goals)
                    {
                        value.initialize();
                    }
                }
            }
            else
            {
                throw new CustomException("New state is ealier than current state!");
            }
        }
        else
        {
            throw new CustomException("Quest is completed!");
        }
    }

    public void setBotched(bool b)
    {
        if(state != QuestState.Completed)
        {
            botched = b;
        }
    }

    public string saveQuest()
    {
        QuestData q1 = new QuestData(state, botched, mustBeTurnedIn, goals, questName, description, mainObj, location, questGiver);
        return q1.save();
    }

    public void loadQuest(QuestData q1)
    {
        this.state = q1.state;
        this.botched = q1.botched;
        //this.mustBeTurnedIn = q1.mustBeTurnedIn;
        this.goals = q1.goals;
        //this.questName = q1.questName;
        //this.description = q1.description;
        //this.mainObj = q1.mainObj;
        //this.location = q1.location;
        //this.questGiver = q1.questGiver;
    }
    
}


[Serializable]
public class QuestData
{
    public Quest.QuestState state;
    public bool botched;

    public bool mustBeTurnedIn;

    public List<QuestGoal> goals = new List<QuestGoal>();

    public string questName;
    public string description;
    public string mainObj; 
    public string location;
    public string questGiver;  

    public QuestData(Quest.QuestState state, bool botched, bool mustBeTurnedIn, List<QuestGoal> goals, string questName, string description, string mainObj, string location, string questGiver)
    {
        this.state = state;
        this.botched = botched;
        this.mustBeTurnedIn = mustBeTurnedIn;
        this.goals = goals;
        this.questName = questName;
        this.description = description;
        this.mainObj = mainObj;
        this.location = location;
        this.questGiver = questGiver;
    }

    public string save()
    {
        return JsonUtility.ToJson(this);
    }
}


public class CustomException : Exception
{
    public CustomException() : base() { }
    public CustomException(string message) : base(message) { }
    public CustomException(string message, Exception inner) : base(message, inner) { }
}
