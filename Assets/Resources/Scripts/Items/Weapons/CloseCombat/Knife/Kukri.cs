using UnityEngine;

public class Kukri:Knife {
    private int maxStackSize = 10;
    private double weight = 9;
    public int amount;

    void Awake(){
        imageInventory = Resources.Load<Texture>("UITextures/Items/Weapons/CloseCombat/Knife/kukri");
    }

    public override int MaxStackSize { get => maxStackSize; }
    public override double Weight { get => weight; }
    public override int Amount { get => amount; set => amount = value; }
}
