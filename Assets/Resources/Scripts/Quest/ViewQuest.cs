using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewQuest : MonoBehaviour 
{

    private int currentQuest;
    public Quest[] questArray;
    public GameObject questUI;
    public GameObject questTitle;
    public GameObject questDescription;
    public GameObject rewardText;
    public GameObject completionLevel;

    private void Start () {
        questUI.SetActive(false);
        currentQuest = 0;
    }

    public void OpenQuestUI (InputAction.CallbackContext context) {
        if (context.started) {
            if (!questUI.activeSelf) {
                if (questArray != null && questArray.Length > 0) {
                    UpdateQuest(questArray[currentQuest]);
                } else {
                    NoQuests();
                }
                questUI.SetActive(true);
            } else {
                CloseQuestUI();
            }
        }
    }
    
    public void CloseQuestUI () {
        questUI.SetActive(false);
    }

    public void UpdateQuest (Quest q) {
        questTitle.GetComponent<TextMeshProUGUI>().text = q.title;
        questDescription.GetComponent<TextMeshProUGUI>().text = q.description;
        rewardText.GetComponent<TextMeshProUGUI>().text = generateRewardText(q);
        completionLevel.GetComponent<TextMeshProUGUI>().text = 
            "Completion: " + q.objective.GetCurrentAmount().ToString() + "/" 
            + q.objective.objectiveAmount.ToString();
    }

    public void NextQuest () {
        if (questArray != null && questArray.Length > 0){
            currentQuest++;
            currentQuest %= questArray.Length;
            UpdateQuest(questArray[currentQuest]);
        } else {
            CloseQuestUI();
        }
    }

    private void NoQuests () {
        questTitle.GetComponent<TextMeshPro>().text = "Quests";
        questDescription.GetComponent<TextMeshPro>().text = "No quests availible";
        completionLevel.GetComponent<TextMeshPro>().text = "";
        rewardText.GetComponent<TextMeshPro>().text = "";
    }

    private string generateRewardText (Quest q) {
        string rep = q.reputationChange.ToString();
        string item = q.itemReward.ToString();
        return "Reputation change: " + rep + "\nItem(s): " + item;
    }

}
