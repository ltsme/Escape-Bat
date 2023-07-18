 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;
    public Transform[] spawnPoint;

    int level;
    float timer;

    private void Awake() {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1); // FloorToInt() : 소수 아래는 버리고 Int형으로 바꾸는 함수, 10초마다 레벨 1씩 증가
        // 반대로 CeilToInt : 소수점 아래를 올리고 Int형으로 바꾸는 함수

        if (timer > spawnData[level].spawnTime){ // level에 따라 소환 주기가 달라짐.
            Spawn();
            timer = 0;
        }       
    }

    void Spawn (){
        // Range(0, 2)는 0 또는 1을 반환한다.
        // 생성 위치를 추가하기 위해, Enemy라는 GameObject 객체를 만들어 사용하자.
        GameObject enemy = GameManager.instance.poolManager.Get(0);
        // Spawner 오브젝트가 가지고 있는 Transform 컴포넌트가 있기 때문에, 0이 아닌 1을 할당한다.
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}