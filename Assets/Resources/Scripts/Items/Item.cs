using UnityEngine;

/// <summary>
/// Abstract class for all items a player can pick up. 
/// It's only a data class without functions
/// </summary>
public abstract class Item : MonoBehaviour {
    protected Texture imageInventory;

    public Texture ImageInventory { get => imageInventory; }
    public abstract int MaxStackSize { get; }
    public abstract double Weight { get; }
    public abstract int Amount { get; set; }

    /// <summary>
    /// Returns the difference between the maximal amount and the amount now
    /// </summary>
    /// <returns> Returns the available space
    public int SpaceAvailable(){
        return MaxStackSize - Amount;
    }
}
