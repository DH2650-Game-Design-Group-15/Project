using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public int amount;
    public bool add;
    public bool remove;

    // Update is called once per frame
    void Update()
    {
        if (add && amount > 0){
            gameObject.AddComponent(typeof(Wood));
            Item item = GetComponent<Wood>();
            Debug.Log(item.GetType().ToString());
            item.Amount = amount;
            GetComponent<Inventory>().Add(item.GetType().ToString(), item, amount);
            Destroy(item);
            add = false;
        }
        if (remove && amount > 0){
            remove = false;
            bool removed = GetComponent<Inventory>().Remove("Wood", amount);
            if (!removed){
                Debug.Log("Can't remove item");
            }
        }
    }
}
