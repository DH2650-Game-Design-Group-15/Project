using System;
using UnityEngine;

public class Usage : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    public Inventory Inventory { get => inventory; private set => inventory = value; }

    public enum UsableItems{
        LifePotion
    }

    /// <summary> Uses the item in the inventory if it's a usable item. </summary>
    /// <param name="itemName"> The name of the used item </param>
    /// <return> The amount of items used of this type. Returns 0 if the item isn't usable. </return>
    public int UseItem(string itemName){
        if (Enum.TryParse(itemName, out UsableItems item)){
            switch (item){
                case UsableItems.LifePotion:
                    int usedAmount = 3;
                    if (inventory.Amount(itemName) >= usedAmount){
                        UseLifePotion();
                        return usedAmount;
                    }
                    break;
                default:
                    Debug.LogWarning(itemName + " should be usable but has no function");
                    break;
            }
        }
        return 0;
    }

    /// <summary> Adds a maximum of 30 hp to the players life </summary>
    private void UseLifePotion(){
        PlayerHealth health = Parent.FindParent(gameObject, typeof(PlayerHealth))?.GetComponent<PlayerHealth>();
        health.PlayerHit(-30);
    }

    // Start is called before the first frame update
    void Start()
    {
        Inventory = GetComponent<Inventory>();
    }
}
