using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFollow : MonoBehaviour
{
    RectTransform rect;

    void Awake() {
        rect = GetComponent<RectTransform>();
    }

    // Player과 같이, Fixedupdate에서 이동이 이루어 져야 한다.
    void FixedUpdate() {
        // 월드 좌표와 스크린 좌표는 다르다.
        // Camera.main.WorldToScreenPoint() : 월드 상의 오브젝트 위치를 스크린 좌표로 변환
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
