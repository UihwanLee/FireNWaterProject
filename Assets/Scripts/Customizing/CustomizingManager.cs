using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CustomizingType
{
    Ember,
    Wade,
}

public class CustomizingManager : MonoBehaviour
{
    /*
     * Customizing UI
     * 
     * #1.SO로 Customizing Color Option 관리
     * #2. Ember / Wade Materail 속성을 변경하여 커스터마이징
     * 
     */

    [Header("GameObjects")]
    [SerializeField] private GameObject colorSlotPrefab;
    [SerializeField] private Image avatar_ember;
    [SerializeField] private Image avatar_wade;

    [Header("Transform Info")]
    [SerializeField] private Transform emberColorSlotParent;
    [SerializeField] private Transform wadeColorSlotParent;

    [Header("Customzing Data")]
    [SerializeField] private List<CustomizingData> emberColorDataList;
    [SerializeField] private List<CustomizingData> wadeColorDataList;

    [Header("CustomzingList")]
    [SerializeField] private List<CustomizingSlot> emberSlotList;
    [SerializeField] private List<CustomizingSlot> wadeSlotList;

    private CustomizingSlot currentEmberSlot;
    private CustomizingSlot currentWadeSlot;

    private void Start()
    {
        GenerateEmberNWadeSlots();
    }

    #region 커스터마이징 초기화

    private void GenerateEmberNWadeSlots()
    {
        // 커스마이징 슬롯 생성 : Ember, Wade
        GenerateColorSlots(emberColorDataList, emberColorSlotParent, emberSlotList);
        GenerateColorSlots(wadeColorDataList, wadeColorSlotParent, wadeSlotList);
    }

    private void GenerateColorSlots(List<CustomizingData> dataList, Transform slotParent, List<CustomizingSlot> colorList)
    {
        // 커스터마이징 슬롯 생성
        for (int i = 0; i < dataList.Count; i++)
        {
            int idx = i;
            CustomizingSlot slot = Instantiate(colorSlotPrefab, slotParent).GetComponent<CustomizingSlot>();
            slot.Init(idx, dataList[idx]);

            UnityEngine.UI.Button btn = slot.GetComponentInChildren<UnityEngine.UI.Button>();
            btn.onClick.AddListener(() => ChoiceColorSlot(slot));

            colorList.Add(slot);
        }
    }

    #endregion

    #region 커스터마이징 선택

    public void ChoiceColorSlot(CustomizingSlot slot)
    {
        // Slot 선택 시 Type에 따라 동작
        switch (slot.Data.type)
        {
            case CustomizingType.Ember:
                ChoiceEmberColor(slot);
                break;
            case CustomizingType.Wade:
                ChoiceWadeColor(slot);
                break;
            default:
                break;
        }
    }

    private void ChoiceEmberColor(CustomizingSlot slot)
    {
        // 이전에 선택된 슬롯이 없다면 초기화
        if (currentEmberSlot == null) currentEmberSlot = emberSlotList[slot.Data.idx];

        // 이전 슬롯 초기화
        currentEmberSlot.ResetSlot();

        // 새로운 슬롯 선택
        currentEmberSlot = emberSlotList[slot.Data.idx];
        currentEmberSlot.SelectSlot();

        // 아바타 색상 적용
        ApplyCustomizingAvatar(slot.Data);

        // 이미 산 커스터마이징 슬롯이라면 적용
        if (currentEmberSlot.Data.isPurchase)
            ApplyCustomizing(currentEmberSlot.Data);
    }

    private void ChoiceWadeColor(CustomizingSlot slot)
    {
        // 이전에 선택된 슬롯이 없다면 초기화
        if (currentWadeSlot == null) currentWadeSlot = wadeSlotList[slot.Data.idx];

        // 이전 슬롯 초기화
        currentWadeSlot.ResetSlot();

        // 새로운 슬롯 선택
        currentWadeSlot = wadeSlotList[slot.Data.idx];
        currentWadeSlot.SelectSlot();

        // 아바타 색상 적용
        ApplyCustomizingAvatar(slot.Data);

        // 새로운 슬롯 선택
        if (currentEmberSlot.Data.isPurchase)
            ApplyCustomizing(currentEmberSlot.Data);
    }

    #endregion

    #region 커스터마이징 구매

    public void PurchaseColor()
    {

    }

    #endregion

    #region 커스터마이징 적용

    private void ApplyCustomizingAvatar(CustomizingData data)
    {
        // 커스터마이징 아바타 적용
        switch (data.type)
        {
            case CustomizingType.Ember:
                avatar_ember.material = data.material;
                avatar_ember.material.SetColor("_Color", new Color(data.color.r, data.color.g, data.color.b));
                break;
            case CustomizingType.Wade:
                avatar_wade.material = data.material;
                avatar_wade.material.SetColor("_Color", new Color(data.color.r, data.color.g, data.color.b));
                break;
            default:
                break;
        }
    }

    private void ApplyCustomizing(CustomizingData data)
    {

    }

    #endregion
}
