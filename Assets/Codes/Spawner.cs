using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;

    public SpawnData[] spawnData;

    public float levelTime;

    int level;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {
        if (GameManager.instance.isLive == false)
        {
            return;
        }
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        // 270초(4분 30초)부터는 강제로 마지막 적 등장(5분으로 잡은 기준으로 만약 다른 시간이라면 변경해야 함)
        if (GameManager.instance.gameTime >= 270f)
        {
            level = spawnData.Length - 1;  // 마지막 데이터 (spriteType 3)
        }

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
