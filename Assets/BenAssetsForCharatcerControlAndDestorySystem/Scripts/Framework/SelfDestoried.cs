using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private GameObject cubePrefab; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAndSpawn()); // ��ʼЭ��
        cubePrefab = Resources.Load<GameObject>("Prefabs/Items/Cube");
        if (cubePrefab == null)
        {
            Debug.LogError("Failed to load the cube prefab!");
        }

    }

    IEnumerator DestroyAndSpawn()
    {
        yield return new WaitForSeconds(5); // �ȴ�5��

        if (cubePrefab != null)
        {
            Instantiate(cubePrefab, transform.position, Quaternion.identity); // �ڵ�ǰλ��ʵ����CubeԤ����
        }
        Destroy(gameObject); // ���ٵ�ǰ��Ϸ����
    }
}
