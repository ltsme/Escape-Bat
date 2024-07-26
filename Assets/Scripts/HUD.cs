using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType {exp, level, kill, time, health}
    public InfoType type;
    Text myText;
    Slider mySlider;

    private void Awake() {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate() {
        switch (type) {
            case InfoType.exp:
                float currentExp = GameManager.instance.exp;
                float nextExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySlider.value = currentExp / nextExp;
                break;
            case InfoType.level:
                // {0:F0}에서 0은 인덱스 넘버, F0은 소수 0자리까지 표시함을 의미
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.health:
                break;
        }
    }
}
