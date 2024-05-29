using UnityEngine;

public class MetalPlate : Item {
    public override void UseInInventory() {
        // TODO
    }

    void Awake(){
        maxStackSize = 15;
        weight = 0.7;
        prefab = Resources.Load<GameObject>("Prefabs/Items/MetalPlate");
        imageInventory = Resources.Load<Texture>("UITextures/Items/MetalPlate");
    }
}
