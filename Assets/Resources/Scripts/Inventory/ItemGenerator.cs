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
                mousePos.z = 10; // ������ȣ����ֵȡ������ĳ������ú������λ��
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
                Debug.Log("creatlaadder");
                // ��ת������������λ������Ԥ����
                Instantiate(prefabToSpawn, worldPosition, Quaternion.identity);

                CraftableInventory.Instance.ladderCraft = false;
                Cursor.visible = false;

            }
        }

    }
}
