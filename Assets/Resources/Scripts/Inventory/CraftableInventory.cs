using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;

public class CraftableInventory : MonoBehaviour
{
    public static CraftableInventory Instance { get; private set; }  // 单例实例

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
            DontDestroyOnLoad(gameObject);  // 保持实例不被销毁
        }
        else
        {
            Destroy(gameObject);  // 如果已有实例，则销毁新对象
        }
    }

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        inputs = FindObjectOfType<Inputs>();
        inventoryinput = FindObjectOfType<InventoryInput>();

        craftableInventoryUI = GameObject.FindWithTag("CraftUI");
        Transform canvasParentTransform = transform.parent;
        Transform inventoryTransform = canvasParentTransform.Find("Inventory");
        inventoryUI = inventoryTransform.gameObject;

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

        Debug.Log(buttonName + " was clicked!");
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
