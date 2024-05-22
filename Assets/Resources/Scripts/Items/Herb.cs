using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herb : Item
{
    // Start is called before the first frame update
    public override void UseInInventory()
    {
        // TODO Player needs maybe water and it can refill players water
    }

    void Awake()
    {
        maxStackSize = 20;
        weight = 0.7;
        prefab = Resources.Load<GameObject>("Prefabs/Items/Herb");
        imageInventory = Resources.Load<Texture>("UITextures/Items/Grass1");
    }
}
