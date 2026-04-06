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

   public void setButton()
    {
        image.enabled = true;

        questName.text = quest.questName;
        mainObjective.text = quest.mainObj;

        button.onClick.AddListener(displayQuest);
    }

    public void clearButton()
    {
        questName.text = "";
        mainObjective.text = "";

        button.onClick.RemoveAllListeners();

        image.enabled = false;

        quest = null;
    }

    public void displayQuest()
    {
        QuestManager.instance.questName.text = quest.questName;
        QuestManager.instance.questGiver.text = quest.questGiver;
        QuestManager.instance.questLocation.text = quest.location;
        QuestManager.instance.questDescription.text = quest.description;        
    }
}
