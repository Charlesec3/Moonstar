using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.Linq;

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
}


public class CustomException : Exception
{
    public CustomException() : base() { }
    public CustomException(string message) : base(message) { }
    public CustomException(string message, Exception inner) : base(message, inner) { }
}
