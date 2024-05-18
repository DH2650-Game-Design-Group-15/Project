using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftableInventory : MonoBehaviour
{
    private Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
            Debug.Log("bad");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
