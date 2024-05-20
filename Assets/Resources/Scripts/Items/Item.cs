using UnityEngine;

/// <summary>
/// Abstract class for all items a player can pick up. 
/// It's only a data class without functions
/// </summary>
public abstract class Item : MonoBehaviour {
    protected Texture imageInventory;
    public GameObject prefab;
    protected int maxStackSize;
    protected double weight;
    [SerializeField] protected int amount;

    public Texture ImageInventory { get => imageInventory; }
    public int MaxStackSize { get => maxStackSize; }
    public double Weight { get => weight; }
    public int Amount { get => amount; set => amount = value; }
    public GameObject Prefab { get => prefab; }

    /// <summary> Abstract method what happens if the player uses the item in the inventory </summary>
    public abstract void UseInInventory();

    /// <summary>
    /// Returns the difference between the maximal amount and the amount now
    /// </summary>
    /// <returns> Returns the available space
    public int SpaceAvailable(){
        return MaxStackSize - Amount;
    }
}
