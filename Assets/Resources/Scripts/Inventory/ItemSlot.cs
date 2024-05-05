using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the amount of items in this item slot and on which field in the inventory it's displayed.
/// </summary>
public class ItemSlot{
    private int amount;
    private int column;
    private int row;
    private bool isFull;
    private readonly int maxAmount;
    private readonly Texture texture;
    
    public ItemSlot(PlayerInventory inventory, Item item) {
        amount = 0;
        isFull = false;
        maxAmount = item.MaxStackSize;
        texture = item.PictureInventory;
        row = 0;
        column = 0;
        bool stop = false;
        foreach (List<bool> rows in inventory.FreeSlot) {
            foreach (bool free in rows) {
                if (free) {
                    stop = true;
                    break;
                }
                column++;
            }
            if (stop){
                break;
            }
            row++;
            column = 0;
        }
    }

    public int Add(int amount) {
        this.amount += amount;
        if (this.amount > maxAmount) {
            isFull = true;
            int above = this.amount - maxAmount;
            this.amount = maxAmount;
            return above;
        } else {
            return 0;
        }
    }

    public int Amount { get => amount; }
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }
    public bool IsFull { get => isFull; }
    public int MaxAmount { get => maxAmount; }
    public Texture Texture => texture;
}