using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObjective
{

    public ObjectiveType objectiveType;
    public int objectiveAmount;
    public int currentAmount;

    public bool questCompleted () {
        return objectiveAmount <= currentAmount;
    }

}

public enum ObjectiveType {
    Kill,
    Gather
}
