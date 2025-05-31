using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;

    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }


    void FixedUpdate()
    {

        if (!isLive)
        {
            return;
        }

        // 플레이어의 키 입력 값을 더한 이동 == 몬스터의 방향 값을 더한 이동
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        // 물리 속도가 이동에 영향을 주지 않도록 속도 제거
        rigid.linearVelocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive)
        {
            return;
        }

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }
}
