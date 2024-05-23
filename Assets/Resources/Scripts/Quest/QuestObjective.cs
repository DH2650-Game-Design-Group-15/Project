using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObjective
{

    public ObjectiveType objectiveType;
    public int objectiveAmount;
    public int currentAmount;

    public void IncreaseCurrentAmount (int amount) {
        currentAmount += amount;
    }
    
    public int GetCurrentAmount () {
        return currentAmount;
    }

    public bool questCompleted () {
        return objectiveAmount <= currentAmount;
    }

}

public enum ObjectiveType {
    Kill,
    Gather
}
