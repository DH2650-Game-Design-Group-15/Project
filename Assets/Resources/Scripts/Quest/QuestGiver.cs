using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour 
{

    public Quest[] questArray;
    private GameObject player;
    private ViewQuest viewQuestScript;

    private void Start () {
        player = GameObject.FindWithTag("Player");
        viewQuestScript = player.GetComponent<ViewQuest>();
    }

    private void GiveQuest (int index) {
        if (questArray.Length > 0) {
            Quest quest = questArray[index];
            viewQuestScript.AddQuest(quest);
        }
    }

    public void GiveKillQuest () {
        for (int i = 0; i < questArray.Length; i++) {
            if (questArray[i].title == "TO WAR!") {
                GiveQuest(i);
            }
        }
    }

    public void GiveGatherQuest () {
        for (int i = 0; i < questArray.Length; i++) {
            if (questArray[i].title == "Resource Shortage") {
                GiveQuest(i);
            }
        }
    }

}