using UnityEngine;

public class Stone : Item {
    public override void UseInInventory() {
        // TODO Player needs maybe water and it can refill players water
    }

    void Awake(){
        maxStackSize = 20;
        weight = 0.7;
        prefab = Resources.Load<GameObject>("Prefabs/Items/Stone");
        imageInventory = Resources.Load<Texture>("UITextures/Items/stone");
    }
}
