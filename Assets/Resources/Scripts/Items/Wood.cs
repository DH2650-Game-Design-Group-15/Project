using UnityEngine;

public class Wood : Item {
    public override void UseInInventory() {
        // TODO Player needs maybe water and it can refill players water
    }

    void Awake(){
        maxStackSize = 20;
        weight = 0.7;
        Debug.LogWarning("Prefab missing for " + GetType().ToString());
        imageInventory = Resources.Load<Texture>("UITextures/Items/wood");
    }
}
