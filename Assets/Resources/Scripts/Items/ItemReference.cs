using UnityEngine;

/// <summary>
/// Stores a Item Component of another objects. Helps to get the back reference from the drawn inventory to the inventory 
/// </summary>
public class ItemReference : MonoBehaviour {
    private string itemName;
    private int amount;

    public string ItemName { get => itemName; set => itemName = value; }
    public int Amount { get => amount; set => amount = value; }
}
