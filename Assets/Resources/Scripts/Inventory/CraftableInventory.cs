using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;

public class CraftableInventory : MonoBehaviour
{
    public static CraftableInventory Instance { get; private set; }  // ����ʵ��

    private Inventory inventory;
    public bool bonfireCraft;
    public bool wallCraft;
    private GameObject prefabToSpawn;
    private GameObject craftableInventoryUI;
    private GameObject inventoryUI;
    private Inputs inputs;
    private InventoryInput inventoryinput;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // ����ʵ����������
        }
        else
        {
            Destroy(gameObject);  // �������ʵ�����������¶���
        }
    }

    void Start()
    {
        Inventory[] inventories = FindObjectsOfType<Inventory>();
        foreach (Inventory inventory in inventories)
        {
            inputs = inventory.GetComponentInChildren<Inputs>();
            if (inputs != null){
                this.inventory = inventory;
                inventoryinput = inventory.GetComponentInChildren<InventoryInput>();
                break;
            }
        }
        craftableInventoryUI = gameObject;
        inventoryUI = Parent.FindChild(GameObject.FindWithTag("Canvas"), "Inventory")?.gameObject;

        if (inventoryUI == null)
            Debug.Log("inventoryUI==null");
        if (inventory == null)
        {
            Debug.LogError("No Inventory component found in the scene");
        }
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => ButtonClicked(button.gameObject.name));
        }

    }
    private void ButtonClicked(string buttonName)
    {
        bool LifePotionButton = false;
        switch (buttonName)
        {

            case "BonfireButton":
                foreach (var i in inventory.Type)
                    if (i.ItemName == "Wood" && i.Amount >= 1) 
                    {
                        //Debug.Log("We have " + i.Amount + "woods");

                        //prefabToSpawn = Resources.Load<GameObject>("Prefabs/Items/Ladder");
                        bonfireCraft = true;
                        inventoryinput.CloseInventory();
                        inventory.Remove("Wood", 1);
                        Cursor.visible = true;

                    }
                break;
            case "WallButton":
                foreach (var i in inventory.Type)
                    if (i.ItemName == "Stone" && i.Amount >= 1)  //temporary i.Amount >= 3 because its amount seems to be 3 in the beginning
                    {
                        //Debug.Log("We have "+i.Amount+"stones");

                        //prefabToSpawn = Resources.Load<GameObject>("Assets/Resources/Prefabs/Items/Wall with Stakes");
                        wallCraft = true;
                        inventoryinput.CloseInventory();
                        inventory.Remove("Stone", 1);
                        Cursor.visible = true;

                    }
                break;

            case "LifePotionButton":
                foreach (var i in inventory.Type)
                    if (i.ItemName == "Herb" && i.Amount >= 0)  //temporary i.Amount >= 3 because its amount seems to be 3 in the beginning
                    {
                        LifePotionButton = true;
                    }
                break;

        }
        if(LifePotionButton)
        {
            /*
            prefabToSpawn = Resources.Load<GameObject>("Prefabs/Items/Ladder");
            ladderCraft = true;*/
            GameObject lifeobject = new GameObject("LifePotion");
            LifePotion lifePotion = lifeobject.AddComponent<LifePotion>();
            if (lifePotion == null)
                Debug.LogError("lifepotion==null");
            inventory.Add(lifePotion.GetType().ToString(), lifePotion, 1);
            inventory.Remove("Herb", 2);
            LifePotionButton = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}
