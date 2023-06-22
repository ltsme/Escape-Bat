using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D collid2D;

    private void Awake() {
        collid2D = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision) {
        // Area 태그가 달린 Area와 충돌 시
        if(collision.CompareTag("Area")){
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 myPos = transform.position;

            // 플레이어의 (0.0) 기준 거리 구하기
            float posX = playerPos.x - myPos.x;
            float posY = playerPos.y - myPos.y;            
            
            // X, Y 거리를 비교해 맵 이동시 수직, 수평 방향 정하기
            // Mathf.Abs 형식을 통해 절대값(무조건 양수)로 값 반환     

            float diffX = Mathf.Abs(posX);
            float diffY = Mathf.Abs(posY);

            // X, Y 거리를 비교해 맵 이동시 +, - 방향 정하기
            float dirX = posX > 0 ? 1 : -1;
            float dirY = posY > 0 ? 1 : -1;
     
            // Ground인 Tilemap과 Enemy인 Enemy 각각에 스크립트 적용, 케이스 구분
            switch (transform.tag) {
                case "Ground" :
                    // 거리 차이가 X축이 Y축보다 크면 수평 이동
                    if(diffX > diffY) {
                        // 각각 20이므로, 총 40을 이동시켜야 정중앙에 위치한다.
                        transform.Translate(Vector3.right * dirX * 40);
                    }// 거리 차이가 Y축이 X축보다 크면 수직 이동
                    else if (diffX < diffY)
                    {
                        // 각각 20이므로, 총 40을 이동시켜야 정중앙에 위치한다.
                        transform.Translate(Vector3.up * dirY * 40);
                    }
                break;

                case "Enemy":
                    // Enemy 태그의 개체의 Capsule Collider 2D가 enable일 때만
                    if(collid2D.enabled){
                        // 플레이어의 이동 방향에 따라 맞은 편에서 나타남
                        if (diffX > diffY)
                        {
                            transform.Translate(Vector3.right * dirX * 20 + new Vector3(Random.Range(-3f, 3f),Random.Range(-3f, 3f),0f));
                        }
                        else if (diffX < diffY)
                        {
                            transform.Translate(Vector3.up * dirY * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                        }
                        
                    }
                    break;
            }
        }
    }
}
