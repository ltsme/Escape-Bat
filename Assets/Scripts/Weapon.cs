using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count; // 갯수
    public float speed; // 회전 속도

    void Start() {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // 무기 id에 따라 로직을 분리
        switch (id)
        {
            case 0:
                // Vector3.back은 (0,0,-1)을 의미하며, 시계방향으로 돈다.
                transform.Rotate(Vector3.back * speed * Time.deltaTime);           
                break;
            case 1:
                break;
            default:
                break;
        }

        // TEST
        if(Input.GetButtonDown("Jump")){
            LevelUP(5, 1);
        }
    }

    public void Init()
    {
        // 무기 id에 따라 로직을 분리
        switch (id)
        {
            case 0:
                speed = 150;
                Place();
                break;
            case 1:
                break;
            default:
                break;
        }
    }

    void Place(){
        for(int i=0; i < count; i++){
            // prefabId(0, 1, 2, ...) 종류에 따라 poolManager의 Element 무기를 가져오는 과정
            Transform bullet;

            if(i < transform.childCount){
                bullet = transform.GetChild(i);
            }else{
                bullet = GameManager.instance.poolManager.Get(prefabId).transform;
                // 부모를 변경, 여기서는 poolManager 오브젝트에서 Weapon 0 오브젝트로 이동한다. transform.parent
                bullet.parent = transform;
            }

            // 무기의 위치, 회전 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 무기의 배치, Rotate를 이용해 360도를 count로 나눈만큼 회전 (z축 값 이동).
            bullet.Rotate(Vector3.forward * 360 * i / count);
            // 무기의 배치, Translate를 이용해 y축 값 이동
            bullet.Translate(bullet.transform.up * 1.5f, Space.World);

            // 근접무기는 per(관통) 수치를 -1로 한다. (무한을 의미)
            bullet.GetComponent<Bullet>().Init(damage, -1);
        }
    }

    void LevelUP(float damage, int count){
        this.damage += damage;
        this.count += count;

        // 근접 무기의 경우, 레벨 업 하면서 처음 배치를 한다.
        if (id == 0){
            Place();
        }
    }
}
