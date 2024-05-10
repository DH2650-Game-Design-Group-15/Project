using UnityEngine;

public class Waterbottle : Bottle {
    public override void UseInInventory() {
        Debug.Log("Drink");
    }

    void Awake(){
        maxStackSize = 20;
        weight = 0.7;
        Debug.LogWarning("Prefab missing for " + GetType().ToString());
        imageInventory = Resources.Load<Texture>("UITextures/Items/Bottle/plastic-bottle");
    }
}
