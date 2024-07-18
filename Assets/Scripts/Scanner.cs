using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    //스캔 범위, 레이어, 스캔 결과(배열), 가장 가까운 목표를 담을 변수
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearTarget;

    private void FixedUpdate() {
        // 원형의 캐스트를 쏘고, 모든 결과 반환
        // 캐스팅 시작 위치, 스캔 반지름, 캐스팅 방향, 캐스팅 범위, 대상 레이어 
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        nearTarget = GetNearTarget();
    }

    Transform GetNearTarget() {
        Transform result = null;
        float diff = 100.0f;

        // foreach 문으로 캐스팅 결과 오브젝트를 하나씩 접근
        foreach(RaycastHit2D target in targets){
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float newDiff = Vector3.Distance(myPos, targetPos);
            
            // foreach 문을 돌며 가져온 거리가 저장된 거리보다 작으면 교체
            if(newDiff < diff){
                diff = newDiff;
                result = target.transform;
            } else {}
        }
        return result;
    }
}
