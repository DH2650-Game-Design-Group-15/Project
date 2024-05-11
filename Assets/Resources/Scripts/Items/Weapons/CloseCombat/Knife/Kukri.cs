using UnityEngine;

public class Kukri:Knife {
    public override void UseInInventory() {
        // TODO Player needs maybe water and it can refill players water
    }

    void Awake(){
        maxStackSize = 20;
        weight = 0.7;
        prefab = Resources.Load<GameObject>("Prefabs/Items/Items/Weapons/CloseCombat/Knife/Kukri_i");
        imageInventory = Resources.Load<Texture>("UITextures/Items/Weapons/CloseCombat/Knife/kukri");
    }
}
