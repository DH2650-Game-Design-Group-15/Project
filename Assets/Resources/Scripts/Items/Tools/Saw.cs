using UnityEngine;

public class Saw : Tools {
    public override void UseInInventory() {
        // TODO Player needs maybe water and it can refill players water
    }

    void Awake(){
        maxStackSize = 5;
        weight = 2;
        prefab = Resources.Load<GameObject>("Prefabs/Items/Items/Tools/Buck-saw_i");
        // TODO prefab is always the same -> changes style after pick up and throwing away
        imageInventory = Resources.Load<Texture>("UITextures/Items/Tools/saw");
    }
}
