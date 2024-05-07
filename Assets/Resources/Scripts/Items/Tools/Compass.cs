using UnityEngine;

public class Compass : Tools {
    private int maxStackSize = 64;
    private double weight = 0.5;
    public int amount;

    void Awake(){
        imageInventory = Resources.Load<Texture>("UITextures/Items/Tools/compass");
    }

    public override int MaxStackSize { get => maxStackSize; }
    public override double Weight { get => weight; }
    public override int Amount { get => amount; set => amount = value; }
}
