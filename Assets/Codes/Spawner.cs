using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;

    public SpawnData[] spawnData;

    int level;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (GameManager.instance.isLive == false)
        {
            return;
        }
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1) ;

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.GetObject(0);
        // 자식 오브젝트만 실행되어야하기에 1부터 시작
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

        // GetComponent로 바로 가져오는 거 가능함
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}


// 직렬화: 개체를 저장 혹은 전송하기 위해 변환
[System.Serializable]

public class SpawnData
{
    public int spriteType;

    public float spawnTime;

    public int health;

    public float speed;


}
