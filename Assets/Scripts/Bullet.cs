using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   public float damage;
   public int per; // 관통

   Rigidbody2D rigid;

   private void Awake() {
      rigid = GetComponent<Rigidbody2D>();
   }

   public void Init(float damage, int per, Vector3 dir){
        this.damage = damage;
        this.per = per;

        // 관통이 -1(무한)보다 큰 경우에만 속도 적용
        if(per > -1){
         rigid.velocity = dir * 15f;
        }
   }

   private void OnTriggerEnter2D(Collider2D other) {
      // per == -1 은 근접 무기를 의미
      if (!other.CompareTag("Enemy") || per == -1)
         return;

      per = per -1;

      // per = 0 이였던 관통이 -1이 될 때
      if(per == -1){
         rigid.velocity = Vector2.zero;
         gameObject.SetActive(false);
      }
   }
}
