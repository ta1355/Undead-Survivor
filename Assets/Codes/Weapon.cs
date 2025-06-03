using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private Transform bulletRotator;

    float timer;

    void Start()
    {
        // 플레이어 위치에 무기를 고정
        transform.position = GameManager.instance.player.transform.position;

        // 회전용 빈 오브젝트 생성
        bulletRotator = new GameObject("BulletRotator").transform;
        bulletRotator.parent = transform;
        bulletRotator.localPosition = Vector3.zero;

        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                // Weapon이 아니라 BulletRotator만 회전
                bulletRotator.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    Fire();
                    timer = 0f;
                }
                break;
        }

        // 무기를 플레이어에 고정 (움직이는 플레이어일 경우)
        transform.position = GameManager.instance.player.transform.position;

        // 테스트용: 점프 키로 LevelUp 테스트
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, count + 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count = count;

        if (id == 0)
        {
            Batch();
        }
    }

    public void Init()
    {
        switch (id)
        {
            // 근접무기
            case 0:
                speed = 150;
                Batch();
                break;
            default:
                speed = 0.3f;
                break;
        }
    }

    void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;

            if (i < bulletRotator.childCount)
            {
                bullet = bulletRotator.GetChild(i);
                bullet.gameObject.SetActive(true); // 재사용 시 비활성화 상태면 다시 켜기
            }
            else
            {
                bullet = GameManager.instance.pool.GetObject(prefabId).transform;
                bullet.parent = bulletRotator;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            float angle = 360f * i / count;
            bullet.Rotate(Vector3.forward * angle);
            bullet.Translate(Vector3.up * 1.5f, Space.Self);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }

        // 🔧 수정: 남은 총알 비활성화 (LevelUp 시 총알 수 감소 대비)
        for (int i = count; i < bulletRotator.childCount; i++)
        {
            bulletRotator.GetChild(i).gameObject.SetActive(false);
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
        {
            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;

        Vector3 dir = targetPos - transform.position;


        dir = dir.normalized; // normalized : 현재 백터의 방향은 유지하고 크기를 1로 변환된 속성

        Transform bullet = GameManager.instance.pool.GetObject(prefabId).transform;
        bullet.position = transform.position;

        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
