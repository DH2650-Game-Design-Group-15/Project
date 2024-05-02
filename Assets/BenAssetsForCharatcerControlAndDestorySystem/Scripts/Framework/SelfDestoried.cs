using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public GameObject cubePrefab; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAndSpawn()); // ��ʼЭ��
    }

    IEnumerator DestroyAndSpawn()
    {
        yield return new WaitForSeconds(5); // �ȴ�����

        if (cubePrefab != null)
        {
            Instantiate(cubePrefab, transform.position, Quaternion.identity); // �ڵ�ǰλ��ʵ����CubeԤ����
        }
        Destroy(gameObject); // ���ٵ�ǰ��Ϸ����
    }
}
