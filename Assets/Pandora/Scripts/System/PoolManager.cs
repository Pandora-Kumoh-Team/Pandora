using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //...������Ʈ Ǯ���� �̿��� ���� ���� 

    public GameObject[] prefabs;
    public int numObject;

    List<GameObject>[] pools;
    private void Awake()
    {
        numObject = 0;
        pools = new List<GameObject>[prefabs.Length];
        for(int index = 0; index < pools.Length; index++)
            pools[index] = new List<GameObject>();
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ��Ȱ��ȭ �� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if(!select)
        {
            //���Ӱ� ����
            numObject++;
            select = Instantiate(prefabs[index], transform);
            select.SetActive(true);
            pools[index].Add(select);
        }

        return select;
    }

}
