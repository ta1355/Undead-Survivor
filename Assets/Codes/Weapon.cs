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

    float timer;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (GameManager.instance.isLive == false)
        {
            return;
        }
        switch (id)
        {
            case 0:
                //
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
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

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        name = "Weapon " + data.itemId;

        transform.parent = player.transform;

        transform.localPosition = Vector3.zero;

        id = data.itemId;

        damage = data.baseDamage;

        count = data.baseCount;

        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;
            default:
                speed = 0.4f;
                break;
        }

        // hand

        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // broadcastMessage: 특정 함수 호출을 모든 자식에게 방송하는 함수
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;

            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
                bullet.gameObject.SetActive(true); // 재사용 시 비활성화 상태면 다시 켜기
            }
            else
            {
                bullet = GameManager.instance.pool.GetObject(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            float angle = 360f * i / count;
            bullet.Rotate(Vector3.forward * angle);
            bullet.Translate(Vector3.up * 1.5f, Space.Self);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }

        // 🔧 수정: 남은 총알 비활성화 (LevelUp 시 총알 수 감소 대비)
        for (int i = count; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
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
