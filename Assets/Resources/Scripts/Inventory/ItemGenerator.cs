using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator: MonoBehaviour
{
    private GameObject prefabToSpawn;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (CraftableInventory.Instance.ladderCraft)
        {
            prefabToSpawn = Resources.Load<GameObject>("Prefabs/Items/Ladder");

            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 10; // 设置深度，这个值取决于你的场景设置和摄像机位置
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
                Debug.Log("creatlaadder");
                // 在转换的世界坐标位置生成预制体
                Instantiate(prefabToSpawn, worldPosition, Quaternion.identity);

                CraftableInventory.Instance.ladderCraft = false;
                Cursor.visible = false;

            }
        }

    }
}
