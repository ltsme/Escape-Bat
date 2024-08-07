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


    float timer; // 원거리 무기를 위한 타이머
    Player player;

    private void Awake() {
        player = GameManager.instance.player;
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
            default:
                // 프레임 마다, timer를 연산 
                timer += Time.deltaTime;
                if(timer > speed){
                    timer = 0f;
                    Fire();
                }
                break;
        }

        // // Space 키 입력 시, 레벨 업
        // if(Input.GetButtonDown("Jump")){
        //     LevelUP(5, 1);
        // } 
    }

    public void Init(ItemData itemData)
    {   
        // 기본 세팅
        name = "Weapon " + itemData.itemId;
        transform.parent = player.transform; // player의 자식 오브젝트로 생성
        transform.localPosition = Vector3.zero; // player 안에서 위치를 0.0.0으로 설정

        // 속성 세팅    
        id = itemData.itemId;
        damage = itemData.baseDamage;
        count = itemData.baseCount;

        // Scriptable Object의 독립성을 위해, 단순 PoolManager의 index가 아닌 Projectile의 속성에 Prefab을 설정한다.
        for(int index = 0; index < GameManager.instance.poolManager.prefabs.Length; index++){
            if(itemData.projectile == GameManager.instance.poolManager.prefabs[index]){
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150;
                Place();
                break;
            default:
                // speed = Fire()의 연사속도
                speed = 0.3f;
                break;
        }
        // Gear가 생성 또는 레벨업, Weapon이 처음 생성 또는 레벨업을 할 때
        // BroadcastMessage() : 특정 함수 호출을 모든 child에게 알림
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }
    }

    public void LevelUP(float damage, int count){
        this.damage += damage;
        this.count += count;

        // 근접 무기의 경우, 레벨 업 하면서 처음 배치를 한다.
        if (id == 0){
            Place();
        }
        // Gear가 생성 또는 레벨업, Weapon이 처음 생성 또는 레벨업을 할 때
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Fire(){
        // 스캔된 가장 가까운 Enemy의 존재 여부 확인
        // 조건이 없는 경우(NULL), return;
        if(!player.scanner.nearTarget)
            return;

        Vector3 targetPos = player.scanner.nearTarget.position;
        // 목표 방향 = 목표 위치 - 내 위치
        Vector3 dir = targetPos - transform.position;
        // 벡터의 방향은 유지하고, 크기는 1로 변환
        dir = dir.normalized; 

        Transform bullet = GameManager.instance.poolManager.Get(prefabId).transform;
        bullet.position = transform.position;
        // FromToRotation() 지정한 축을 중심으로 목표를 향해 회전 
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        // 원거리는 
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
