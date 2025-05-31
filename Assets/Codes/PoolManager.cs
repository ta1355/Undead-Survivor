using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    // 프리펩들을 보관할 변수
    public GameObject[] prefabs;

    // 풀 담당을 하는 리스트들
    List<GameObject>[] pools;


    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }

        Debug.Log(pools.Length + ": 풀 길이");
    }

    public GameObject GetObject(int i)
    {
        GameObject select = null;

        // 선택한 풀의 놀고 있는(비활성화) 게임오브젝트 접근함
        // 발견하면 select 변수에 할당

        foreach (GameObject item in pools[i])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 없다면? 새롭게 생성하고 select 변수에 할당당

        if (select == null)
        {
            select = Instantiate(prefabs[i], transform);

            pools[i].Add(select);
            
            Debug.Log("새로운 오브젝트 생성: " + select.name);
        }

        return select;
    }

}
