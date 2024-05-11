using UnityEngine;

public class Waterbottle : Bottle {
    public override void UseInInventory() {
        Debug.Log("Drink");
    }

    void Awake(){
        maxStackSize = 20;
        weight = 0.7;
        prefab = Resources.Load<GameObject>("UITextures/Items/Items/Bottle/Waterbottle_i");
        imageInventory = Resources.Load<Texture>("UITextures/Items/Bottle/plastic-bottle");
    }
}
