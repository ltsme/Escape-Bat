using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    public Vector2 inputVec;
    public float speed = 5;
    public Scanner scanner;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // 시작할 때 한번만 실행되는 생명주기 Awake
    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     // GetAxis -> GetAxisRaw 미끄러지지 않는 조작감
    //     inputVec.x = Input.GetAxisRaw("Horizontal");
    //     inputVec.y = Input.GetAxisRaw("Vertical");
    // }

    // 새로운 시스템인 Input System 사용
    void OnMove(InputValue value){
        inputVec = value.Get<Vector2>();
    }

    // 물리 연산 프레임마다 호출되는 생명주기 함수
    private void FixedUpdate() {
        // 1. 힘을 준다.
        // rigid.AddForce(inputVec);

        // 2. 속도 제어.
        // rigid.velocity = inputVec;

        // 3. 위치 이동
        // MovePosition은 위치 이동이라 현재 위치도 더해주어야 함.
        // FixedUpdate 에서는 fixedDeltaTime을 쓴다.
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime; // normalized : 벡터 값의 크기가 1이 되도록 수정 (대각선의 경우 루트2 이므로)
        rigid.MovePosition(rigid.position + nextVec);
    }

    // 프레임이 종료 되기 전 실행되는 생명주기 함수
    private void LateUpdate() {

        animator.SetFloat("Speed", inputVec.magnitude); // 벡터의 방향을 제외한, 순수한 길이 값만 반환

        if (inputVec.x != 0){ // 입력이 없을 때는 그대로 유지, 입력이 있을 때
            spriteRenderer.flipX = inputVec.x < 0; // 비교 연산자의 결과를 바로 넣을 수 있다. 0보다 작으면 true(x를 flip 시킨다), 0보다 크면 false
        }
    }
}
