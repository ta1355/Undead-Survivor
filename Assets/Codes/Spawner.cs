using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;

    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.2f)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.GetObject(Random.Range(0, 2));
        // 자식 오브젝트만 실행되어야하기에 1부터 시작
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}
