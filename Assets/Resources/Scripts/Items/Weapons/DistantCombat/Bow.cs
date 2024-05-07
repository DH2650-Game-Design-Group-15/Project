using UnityEngine;

public class Bow : DistantCombat {
    private int maxStackSize = 1;
    private double weight = 5;
    public int amount;

    void Awake(){
        imageInventory = Resources.Load<Texture>("UITextures/Items/Weapons/DistantCombat/bow");
    }

    public override int MaxStackSize { get => maxStackSize; }
    public override double Weight { get => weight; }
    public override int Amount { get => amount; set => amount = value; }
}
