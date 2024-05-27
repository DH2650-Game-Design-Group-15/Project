using UnityEngine;

public class Uranium : Item {
    public override void UseInInventory() {
        // TODO
    }

    void Awake(){
        maxStackSize = 10;
        weight = 0.7;
        prefab = Resources.Load<GameObject>("Prefabs/Items/Uranium");
        imageInventory = Resources.Load<Texture>("UITextures/Items/Uranium");
    }
}
