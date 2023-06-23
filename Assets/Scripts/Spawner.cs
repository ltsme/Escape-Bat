 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    float timer;

    private void Awake() {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.5f){
            Spawn();
            timer = 0;
        }       
    }

    void Spawn (){
        // Range(0, 2)는 0 또는 1을 반환한다.
        // 생성 위치를 추가하기 위해, Enemy라는 GameObject 객체를 만들어 사용하자.
        GameObject enemy = GameManager.instance.poolManager.Get(Random.Range(0, 2));
        // Spawner 오브젝트가 가지고 있는 Transform 컴포넌트가 있기 때문에, 0이 아닌 1을 할당한다.
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}
