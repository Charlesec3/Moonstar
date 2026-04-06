using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestLoader : MonoBehaviour
{
    public Quest quest;

    public Button button;
    public TextMeshProUGUI questName;
    public TextMeshProUGUI mainObjective;

    public Image image;

    [SerializeField] private ObjectiveLoader[] loaders;

   public void setButton()
    {
        //image.enabled = true;

        questName.text = quest.questName;
        mainObjective.text = quest.mainObj;

        button.onClick.AddListener(displayQuest);
    }

    public void clearButton()
    {
        questName.text = "";
        mainObjective.text = "";

        button.onClick.RemoveAllListeners();

        //image.enabled = false;

        quest = null;
    }

    public void displayQuest()
    {
        for (int i = 0; i < loaders.Length; i++)
        {
            loaders[i].clearButton();
        }

        QuestManager.instance.questName.text = quest.questName;
        QuestManager.instance.questGiver.text = quest.questGiver;
        QuestManager.instance.questLocation.text = quest.location;
        QuestManager.instance.questDescription.text = quest.description;   

        setObjectiveLoaders();     
    }


    public void setObjectiveLoaders()
    {
        if(quest.goals.Count > 0)
        {
            for (int i = 0; i < loaders.Length; i++)
            {
                loaders[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < loaders.Length; i++)
            {
                loaders[i].clearButton();
            }

            //int startIndex = (currentPg - 1) * loaders.Length;
            int startIndex = 0;
            int loadNum = 0;

            for (int i = startIndex; loadNum < loaders.Length; i++)
            {
                if (i > quest.goals.Count - 1)
                {
                    break;
                }
                else
                {
                    loaders[loadNum].objective = quest.goals[i];
                    loaders[loadNum].setButton();
                    ++loadNum;
                }
            }  
        }

        for (int i = 0; i < loaders.Length; i++)
        {
            if(loaders[i].objective == null)
            { 
                loaders[i].gameObject.SetActive(false);
            }
        }  
    }
}
