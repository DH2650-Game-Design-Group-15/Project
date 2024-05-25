using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    
    public string title;
    public string description;
    public int reputationChange;
    public int rewardAmount;
    public string itemReward;
    public Item questItem;
    public Item item;
    public bool isActive;
    public bool isCompleted;
    public QuestObjective objective;
    public Fraction fraction;

}
