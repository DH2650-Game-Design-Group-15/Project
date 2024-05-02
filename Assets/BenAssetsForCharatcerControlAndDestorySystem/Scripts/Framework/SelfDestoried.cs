using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public GameObject cubePrefab; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAndSpawn()); // 开始协程
    }

    IEnumerator DestroyAndSpawn()
    {
        yield return new WaitForSeconds(5); // 等待三秒

        if (cubePrefab != null)
        {
            Instantiate(cubePrefab, transform.position, Quaternion.identity); // 在当前位置实例化Cube预制体
        }
        Destroy(gameObject); // 销毁当前游戏对象
    }
}
