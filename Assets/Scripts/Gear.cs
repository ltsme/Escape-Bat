using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData itemData){
        // 기본 세팅
        name = "Gear " + itemData.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // 속성 세팅
        type = itemData.itemType;
        rate = itemData.damages[0];

        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear(); // 레벨 업 시 로직 적용
    }

    private void ApplyGear(){
        switch (type){
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
            default :
                break;
        }
    }

    private void RateUp(){
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach(Weapon weapon in weapons){
            switch (weapon.id){
                case 0:
                    // 근저리의 경우, 돌아가는 속도가 빨라짐
                    weapon.speed = 150 + (rate * 200);
                    break;
                case 1:
                    // 원거리의 경우, 연사속도가 더 빨라짐
                    weapon.speed = 0.5f * (1f - rate);
                    break;
                default:
                    break;
            }
        }
    }
    private void SpeedUp(){
        float speed = 3;
        GameManager.instance.player.speed += speed * rate;
    }
}
