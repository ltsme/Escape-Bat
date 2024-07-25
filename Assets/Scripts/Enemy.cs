using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;    
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] rAnimatorCon;
    public Rigidbody2D target;
    bool isLive;

    Rigidbody2D rigid;
    Collider2D collid2D;
    SpriteRenderer sprite;
    Animator animator;
    WaitForFixedUpdate waitForFU;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        collid2D = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        waitForFU = new WaitForFixedUpdate();
    }

    private void FixedUpdate() {
        // 살아있을때만 추적하도록
        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 vecDir = target.position - rigid.position;
        Vector2 nextVec = vecDir.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        // movePosition의 경우 순간이동과 같이 위치의 이동
        // Rigidbody의 velocity를 0으로 함으로써 물리 속도가 이동에 영향을 주지 않도록
        rigid.velocity = Vector2.zero; // Vector2(0,0)

        // if (isLive || !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) { // animator.SetTrigger("Hit"); 파라미터 Hit에 의해 State가 Hit인지 확인
        //     // 타겟 위치 - 나의 위치 = 위치 차이
        //     Vector2 vecDir = target.position - rigid.position;
        //     Vector2 nextVec = vecDir.normalized * speed * Time.fixedDeltaTime;
        //     rigid.MovePosition(rigid.position + nextVec);
        //     // movePosition의 경우 순간이동과 같이 위치의 이동
        //     // Rigidbody의 velocity를 0으로 함으로써 물리 속도가 이동에 영향을 주지 않도록
        //     rigid.velocity = Vector2.zero; // Vector2(0,0)
        // }        
    }

    private void LateUpdate() {
        // 목표의 X축 값과 자신의 X축 값을 비교해 작으면 (플레이어가 왼쪽에 위치하면) Flip true
        sprite.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable() {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        collid2D.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        animator.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = rAnimatorCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D other) {
        // 피격 대상이 Bullet인지 확인, 그리고 Enemy가 살아있을 때
        if(!other.CompareTag("Bullet") || !isLive)
            return;
        
        // 피격 계산
        health -= other.GetComponent<Bullet>().damage;
        StartCoroutine(HitKnockBack());
        // 남은 체력을 바탕으로, 피격과 사망으로 나누기
        // 피격
        if(health >0){
            animator.SetTrigger("Hit");  
        // 사망
        }else{
            isLive = false;
            collid2D.enabled = false;
            rigid.simulated = false;
            sprite.sortingOrder = 1;
            animator.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    // 코루틴 : 일반적인 함수와 달리 생명주기와 비동기적으로 실행되는 함수
    // 코루틴의 인터페이스
    IEnumerator HitKnockBack(){
        // 코루틴의 리턴 키워드
        //yield return null; // 1 프레임 쉬기
        //yield return new WaitForSeconds(2f); // 2초 쉬기
        yield return waitForFU; // 하나의 물리 프레임을 쉬기

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos; // 플레이어 반대 방향으로 가기
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // 방향 정규화, 순간적인 힘이므로 Impulse 속성 추가
    }

    void Dead(){
        gameObject.SetActive(false);
    }
}
