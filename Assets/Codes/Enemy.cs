using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed;

    public float health;

    public float maxHealth;

    public RuntimeAnimatorController[] animCon;

    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;

    Animator animator;

    SpriteRenderer spriter;

    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }


    void FixedUpdate()
    {

        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))  // GetCurrentAnimatorStateInfo : 현재 에니메이션 상태
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

        isLive = true;

        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = animCon[data.spriteType];

        speed = data.speed;

        maxHealth = data.health;

        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
        {
            return;
        }

        health -= collision.GetComponent<Bullet>().damage;

        // getcurrentAnimatorstatelnfo : 현재 상태 정보를 가져오는 함수
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            Dead();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;    // 다음 물리 프레임 딜레이

        Vector3 playerPos = GameManager.instance.player.transform.position;

        Vector3 dirVec = transform.position - playerPos;

        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
