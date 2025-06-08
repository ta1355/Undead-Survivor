using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public Vector2 inputVector;

    public Rigidbody2D rigid;

    public float speed = 3.5f;

    SpriteRenderer spriteRenderer;

    Animator animator;

    public Scanner scanner;

    public Hand[] hands;


    void Awake()
    {
        // 컴포넌트 초기화
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); // true는 비활성화된 오브젝트도 포함해서 찾음
    }

    void FixedUpdate()
    {
        // 대각선 이동용
        Vector2 nextVector = inputVector * speed * Time.fixedDeltaTime;

        // 위치 이동
        rigid.MovePosition(rigid.position + nextVector);
    }

    void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        // 애니메이션 속도 설정정
        // magnitude는 백터의 크기를 말함함
        animator.SetFloat("Speed", inputVector.magnitude);
        if (inputVector.x != 0)
        {
            // 방향 전환 양수 음수로 true false 
            spriteRenderer.flipX = inputVector.x < 0;
        }
    }
}
