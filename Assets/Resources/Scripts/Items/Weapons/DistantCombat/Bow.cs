using UnityEngine;

public class Bow : DistantCombat {
    public override void UseInInventory() {
        // TODO Player needs maybe water and it can refill players water
    }

    void Awake(){
        maxStackSize = 20;
        weight = 0.7;
        Debug.LogWarning("Prefab missing for " + GetType().ToString());
        // TODO prefab is always the same -> changes style after pick up and throwing away
        imageInventory = Resources.Load<Texture>("UITextures/Items/Weapons/DistantCombat/bow");
    }
}
