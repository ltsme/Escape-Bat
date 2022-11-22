using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 5;
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 시작할 때 한번만 실행되는 생명주기 Awake
    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     // GetAxis -> GetAxisRaw 미끄러지지 않는 조작감
    //     inputVec.x = Input.GetAxisRaw("Horizontal");
    //     inputVec.y = Input.GetAxisRaw("Vertical");
    // }


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
}
