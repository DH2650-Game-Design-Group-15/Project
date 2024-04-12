using System.Collections.Generic;
using UnityEngine;

public class InventoryItemHelper {
    private int amount;
    private readonly List<ItemSlot> itemSlots;
    private readonly string itemName;
    private readonly PlayerInventory inventory;

    public InventoryItemHelper(Item item, PlayerInventory inventory){
        amount = 0;
        itemSlots = new List<ItemSlot>();
        itemName = item.GetType().Name;
        this.inventory = inventory;
    }

    public int Add(int amount, Item item){
        foreach (ItemSlot itemSlot in itemSlots) {
            if (!itemSlot.IsFull){
                amount = itemSlot.Add(amount);
            }
        }
        while (amount > 0 && inventory.HasFreeSlots()) {
            itemSlots.Add(new ItemSlot(inventory, item));
            amount = itemSlots[^1].Add(amount);
            inventory.UsedSlots++;
        }
        UpdateAmount();
        return amount;
    }

    private void UpdateAmount() {
        amount = 0;
        foreach (ItemSlot item in itemSlots) {
            amount += item.Amount;
        }
    }

    public string ItemName { get => itemName; }
    public List<ItemSlot> ItemSlots { get => itemSlots; }
    public int Amount { get => amount; }
    public PlayerInventory Inventory { get => inventory; }



    /*private int amount;
    private ArrayList item;
    private string itemName;

    public InventoryItemHelper(GameObject gameObject, string name){
        this.item = new ArrayList(){gameObject};
        this.amount = 1;
        this .itemName = name;
    }

    public int Amount { get => amount; }
    public ArrayList Item { get => item; }
    public string ItemName { get => itemName; }

    public void Add(GameObject gameObject){
        if (gameObject.GetComponent<Item>().GetType() == Item[0].GetType()){
            Item.Add(gameObject);
            amount = Item.Count;
        }
    }

    public GameObject Remove(){
        GameObject res = (GameObject)Item[0];
        Item.Remove(res);
        amount = Item.Count;
        return res;
    }*/
}
