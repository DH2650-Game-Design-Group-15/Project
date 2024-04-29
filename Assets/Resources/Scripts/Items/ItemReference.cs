using UnityEngine;

public class ItemReference : MonoBehaviour {
    public Item item;
    public Item Item { get => item; set => item = value; }
}
