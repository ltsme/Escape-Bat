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
    SpriteRenderer sprite;
    Animator animator;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {

        // 살아있을때만 추적하도록
        // if (!isLive) { return; }
        if (isLive) {
            // 타겟 위치 - 나의 위치 = 위치 차이
            Vector2 vecDir = target.position - rigid.position;
            Vector2 nextVec = vecDir.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            // movePosition의 경우 순간이동과 같이 위치의 이동
            // Rigidbody의 velocity를 0으로 함으로써 물리 속도가 이동에 영향을 주지 않도록
            rigid.velocity = Vector2.zero; // Vector2(0,0)
        }        
    }

    private void LateUpdate() {
        // 목표의 X축 값과 자신의 X축 값을 비교해 작으면 (플레이어가 왼쪽에 위치하면) Flip true
        sprite.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable() {
         target = GameManager.instance.player.GetComponent<Rigidbody2D>();
         isLive = true;
         health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = rAnimatorCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }
}
