using UnityEngine;

public class Wood : Item {
    public override void UseInInventory() {
        // TODO Player needs maybe water and it can refill players water
    }

    void Awake(){
        maxStackSize = 20;
        weight = 0.7;
        prefab = Resources.Load<GameObject>("Prefabs/Items/Wood");
        imageInventory = Resources.Load<Texture>("UITextures/Items/wood");
    }
}
