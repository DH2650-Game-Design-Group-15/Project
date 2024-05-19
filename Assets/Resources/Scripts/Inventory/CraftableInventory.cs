using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;

public class CraftableInventory : MonoBehaviour
{
    public static CraftableInventory Instance { get; private set; }  // 单例实例

    private Inventory inventory;
    public bool ladderCraft;
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
        switch (buttonName)
        {

            case "LadderButton":
                foreach (var i in inventory.Type)
                    if (i.ItemName == "Wood" && i.Amount >= 0)  //temporary i.Amount >= 3 because its amount seems to be 3 in the beginning
                    {
                        Debug.Log(i.Amount);

                        prefabToSpawn = Resources.Load<GameObject>("Prefabs/Items/Ladder");
                        // inventory.Remove(i.ItemName, 1);
                        ladderCraft = true;
                        inventoryinput.CloseInventory();
                        //inventoryinput.SetCursor(true);
                    }
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}
