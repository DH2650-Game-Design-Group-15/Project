using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour 
{

    public Quest[] questArray;
    public GameObject player;
    private ViewQuest viewQuestScript;
    
    //testing
    public bool give;

    private void Start () {
        viewQuestScript = player.GetComponent<ViewQuest>();
    }

    private void Update () {
        if (give) {
            if (questArray.Length > 0) {
                GiveQuest(0);
            }
            give = false;
        }
    }

    public void GiveQuest (int index) {
        Quest quest = questArray[index];
        viewQuestScript.AddQuest(quest);
        questArray = viewQuestScript.RemoveQuest(questArray, index);
    }

}