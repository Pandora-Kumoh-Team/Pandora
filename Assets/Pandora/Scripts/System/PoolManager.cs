using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //...오브젝트 풀링을 이용한 몬스터 스폰 

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

        // 비활성화 된 오브젝트 접근
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
            //새롭게 생성
            numObject++;
            select = Instantiate(prefabs[index], transform);
            select.SetActive(true);
            pools[index].Add(select);
        }

        return select;
    }

}
