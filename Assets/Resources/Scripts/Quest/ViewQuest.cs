using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewQuest : MonoBehaviour 
{

    public Inventory playerInventory;
    public InventoryInput inventoryInput;
    public Quest[] questArray;
    public GameObject questUI;
    public GameObject questTitle;
    public GameObject questDescription;
    public GameObject rewardText;
    public GameObject completionLevel;
    public GameObject questNext;
    public GameObject questComplete;
    private int currentQuest;

    private void Start () {
        questUI.SetActive(false);
        currentQuest = 0;
    }

    private void Update () {
        if (questArray != null && questArray.Length > 0) {
            UpdateQuest(questArray[currentQuest]);
        }
    }

    public void OpenQuestUI (InputAction.CallbackContext context) {
        if (context.started) {
            if (!questUI.activeSelf) {
                inventoryInput.SetCursor(true);
                if (questArray == null || questArray.Length <= 0) {
                    NoQuests();
                }
                questUI.SetActive(true);
            } else {
                inventoryInput.SetCursor(false);
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
        if (q.objective.objectiveType == ObjectiveType.GatherWood) {
            int woodAmount = playerInventory.Amount("Wood");
            q.objective.IncreaseCurrentAmount(woodAmount - q.objective.GetCurrentAmount());
        }
        completionLevel.GetComponent<TextMeshProUGUI>().text = q.objective.GetCurrentAmount().ToString()
            + "/" + q.objective.objectiveAmount.ToString();
        if (q.objective.questCompleted()) {
            questNext.SetActive(false);
            questComplete.SetActive(true);
        } else {
            questNext.SetActive(true);
            questComplete.SetActive(false);
        }
    }

    public void NextQuest () {
        if (questArray != null && questArray.Length > 0){
            currentQuest++;
            currentQuest %= questArray.Length;
        } else {
            CloseQuestUI();
        }
    }

    public void CompleteQuest () {
        Quest q = questArray[currentQuest];
        if (q.objective.questCompleted()) {
            GetComponent<Fractions>().SetReputation(q.fraction, q.reputationChange);
            if (q.questItem != null) {
                playerInventory.Remove(q.questItem, q.objective.objectiveAmount);
            }
            playerInventory.Add(q.itemReward, q.item, q.rewardAmount);
            questArray = RemoveQuest(questArray, currentQuest);
        }
    }

    public Quest[] RemoveQuest (Quest[] original, int index) {
        
        Quest[] updatedQuestArray = new Quest[original.Length - 1];
        for (int i = 0, j = 0; i < original.Length; i++) {
            if (i == index) {
                continue;
            }
            updatedQuestArray[j++] = original[i];
        }
        if (currentQuest != 0) {
            currentQuest--;
        }
        if (updatedQuestArray == null || updatedQuestArray.Length <= 0) {
            questComplete.SetActive(false);
            NoQuests();
        }
        return updatedQuestArray;
    }

    public void AddQuest (Quest q) {
        Quest[] updatedQuestArray = new Quest[questArray.Length + 1];
        for (int i = 0; i < questArray.Length; i++) {
            updatedQuestArray[i] = questArray[i];
        }
        updatedQuestArray[questArray.Length] = q;
        questArray = updatedQuestArray;
    }

    private void NoQuests () {
        questTitle.GetComponent<TextMeshProUGUI>().text = "Quests";
        questDescription.GetComponent<TextMeshProUGUI>().text = "No quests availible";
        completionLevel.GetComponent<TextMeshProUGUI>().text = "";
        rewardText.GetComponent<TextMeshProUGUI>().text = "";
    }

    private string generateRewardText (Quest q) {
        string rep = q.reputationChange.ToString();
        string item = q.itemReward;
        string amt = q.rewardAmount.ToString();
        return "Reputation change: " + rep + "\nItem(s): " + amt + " " + char.ToUpper(item[0]) + item.Substring(1).ToLower();
    }

}
