using UnityEngine;

public class Waterbottle : Bottle {
    private int maxStackSize = 20;
    private double weight = 0.7;
    public int amount;

    public override int MaxStackSize { get => maxStackSize; }

    public override double Weight { get => weight; }

    public override int Amount { get => amount; set => amount = value; }

    void Awake(){
        imageInventory = Resources.Load<Texture>("UITextures/Items/Bottle/plastic-bottle");
    }
}
