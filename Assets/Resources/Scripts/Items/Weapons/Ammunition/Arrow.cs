using UnityEngine;

public class Arrow : Ammunition {
    private int maxStackSize = 10;
    private double weight = 9;
    public int amount;

    void Awake(){
        imageInventory = Resources.Load<Texture>("UITextures/Items/Weapons/Ammunition/arrow");
    }

    public override int MaxStackSize { get => maxStackSize; }
    public override double Weight { get => weight; }
    public override int Amount { get => amount; set => amount = value; }
}
