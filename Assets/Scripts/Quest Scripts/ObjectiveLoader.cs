using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectiveLoader : MonoBehaviour
{
    public QuestGoal objective;

    public TextMeshProUGUI questObjevtive;
    
    public void setButton()
    {
        questObjevtive.text = objective.description + " " + objective.progress;
    }

    public void clearButton()
    {
        questObjevtive.text = "";

        objective = null;
    }
}
