using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;

    Image icon;
    Text textLevel;

    private void Awake() {
        // 자식 오브젝트의 컴포넌트가 필요
        // 0은 Button오브젝트, 1은 자식(Image)오브젝트
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        // Text는 어짜피 1개밖에 없음. [0]
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
    }

    private void LateUpdate() {
        textLevel.text = "Lv." + level;
    }

    public void OnClick(){
        switch (data.itemType) {
            // Melle와 Range는 같은 조건
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if(level == 0){
                    // 새 게임오브젝트 추가
                    GameObject newWeapon = new GameObject();
                    // AddComponent<t> : 게임 오브젝트에 T 컴포넌트 추가
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }else{
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUP(nextDamage, nextCount);
                }
                break;
            case ItemData.ItemType.Glove:
                break;
            case ItemData.ItemType.Shoe:
                break;
            case ItemData.ItemType.Heal:
                break;
        }
        level ++;
        // 5개로 설정해놓은 레벨업 범위를 넘어가지 않도록
        if (level == data.damages.Length){
            // Button 오브젝트 (Item 0, Item 1, ...)의 기능 끄기
            GetComponent<Button>().interactable = false;
        }
    }
}   
