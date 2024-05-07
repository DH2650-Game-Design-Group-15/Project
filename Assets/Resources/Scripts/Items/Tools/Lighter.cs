using UnityEngine;

public class Lighter : Tools {
    private int maxStackSize = 20;
    private double weight = 0.1;
    public int amount;

    public override int MaxStackSize { get => maxStackSize; }

    public override double Weight { get => weight; }

    public override int Amount { get => amount; set => amount = value; }

    void Awake(){
        imageInventory = Resources.Load<Texture>("UITextures/Items/Tools/lighter");
    }
}
