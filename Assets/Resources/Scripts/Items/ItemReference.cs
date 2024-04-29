using UnityEngine;

/// <summary>
/// Stores a Item Component of another objects. Helps to get the back reference from the drawn inventory to the inventory 
/// </summary>
public class ItemReference : MonoBehaviour {
    public Item item;
    public Item Item { get => item; set => item = value; }
}
