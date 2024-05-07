using UnityEngine;

public class Saw : Tools {
    private int maxStackSize = 1;
    private double weight = 2;
    public int amount;

    public override int MaxStackSize { get => maxStackSize; }

    public override double Weight { get => weight; }

    public override int Amount { get => amount; set => amount = value; }

    void Awake(){
        imageInventory = Resources.Load<Texture>("UITextures/Items/Tools/saw");
    }
}
