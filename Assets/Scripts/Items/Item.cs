using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour {
    public Texture pictureInventory;
    private int amount;

    public Texture PictureInventory { get => pictureInventory; }
    public abstract int MaxStackSize { get; }
    public abstract double Weight { get; }
    public int Amount { get => amount; set => amount = value; }
}
