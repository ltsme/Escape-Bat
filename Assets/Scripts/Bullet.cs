using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   public float damage;
   public int per; // 관통

   public void Init(float damage, int per){
        this.damage = damage;
        this.per = per;
   }
}
