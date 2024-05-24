using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour 
{

    public Quest[] questArray;
    public ViewQuest viewQuestScript;
    public GameObject player;

    public void GiveQuest (int index) {
        Quest quest = questArray[index];
        player.GetComponent<ViewQuest>().AddQuest(quest);
        questArray = viewQuestScript.RemoveQuest(questArray, index);
    }

}