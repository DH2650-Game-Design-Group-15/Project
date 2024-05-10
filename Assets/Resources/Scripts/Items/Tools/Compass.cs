using UnityEngine;

public class Compass : Tools {
    public override void UseInInventory() {}

    void Awake(){
        maxStackSize = 64;
        weight = 0.1;
        prefab = Resources.Load<GameObject>("Prefabs/Items/Items/Tools/Compass-palm_n");
        // TODO prefab is always the same -> changes style after pick up and throwing away
        imageInventory = Resources.Load<Texture>("UITextures/Items/Tools/compass");
    }
}
