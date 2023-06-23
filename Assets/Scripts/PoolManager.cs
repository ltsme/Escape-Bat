using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹을 보관할 변수
    public GameObject[] prefabs;

    // 풀 담당을 하는 리스트
    List<GameObject>[] pools;

    private void Awake() {
        // 리스트 배열 초기화 시, 길이를 프리팹 배열 길이와 같게
        pools = new List<GameObject>[prefabs.Length];
        for (int index = 0; index<pools.Length; index++){
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index){
        GameObject gameObject = null;

        // 선택한 pool에 있는 비 활성화 된 게임 오브젝트 반환

        // foreach : 배열, 리스트의 데이터를 순차적으로 순회 접근
        foreach (GameObject item in pools[index]){
            // 비 활성화된 item에 대해서만 발동
            if (!item.activeSelf){
                gameObject = item;
                gameObject.SetActive(true);
                break;
            }
        }

        // 못 찾을 경우, 새롭게 생성(prefab)에서 복제해 pool에 집어 넣는다.
         if (gameObject == null){
            gameObject = Instantiate(prefabs[index], transform);
            pools[index].Add(gameObject);
         }

        return gameObject;
    }
}
