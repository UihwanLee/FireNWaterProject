using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCustomizingData", menuName = "Data/CustomizingData")]
public class CustomizingData : ScriptableObject
{
    // 캐릭터가 사용할 커스터마이징 데이터

    public CustomizingType type;    // 타입
    public int idx;                 // 인덱스
    public Color color;             // 변경 색상
    public int price;               // 가격
    public bool isPurchase;         // 구매 여부
    public bool isPick;             // 착용 여부
    public Material material;       // 적용 마테리얼
}
