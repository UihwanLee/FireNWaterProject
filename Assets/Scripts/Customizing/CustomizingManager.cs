using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI fireJemCount;
    [SerializeField] private TextMeshProUGUI waterJemCount;

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

    [Header("Test")]
    [SerializeField] private SpriteRenderer ember;
    [SerializeField] private SpriteRenderer wade;
    [SerializeField] private int JemCount = 10000; // Test 용

    private void Start()
    {
        InitColorDataList();
        GenerateEmberNWadeSlots();
        UpdateJem();
    }

    #region 커스터마이징 초기화

    private void InitColorDataList()
    {
        for (int i = 0; i < emberColorDataList.Count; i++)
        {
            emberColorDataList[i].isPurchase = (i == 0);
            emberColorDataList[i].isPick = (i == 0);
        }
        for (int i = 0; i < wadeColorDataList.Count; i++)
        {
            wadeColorDataList[i].isPurchase = (i == 0);
            wadeColorDataList[i].isPick = (i == 0);
        }
    }

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
            slot.Init(idx, dataList[idx], this);

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

        // 이미 산 커스터마이징 슬롯이라면 적용
        if (slot.Data.isPurchase)
        {
            currentEmberSlot.Data.isPick = false;
            ApplyCustomizing(currentEmberSlot.Data);
        }

        // 이전 슬롯 초기화
        currentEmberSlot.ResetSlot();

        // 새로운 슬롯 선택
        currentEmberSlot = emberSlotList[slot.Data.idx];
        currentEmberSlot.SelectSlot();

        // 아바타 색상 적용
        ApplyCustomizingAvatar(slot.Data);
    }

    private void ChoiceWadeColor(CustomizingSlot slot)
    {
        // 이전에 선택된 슬롯이 없다면 초기화
        if (currentWadeSlot == null) currentWadeSlot = wadeSlotList[slot.Data.idx];

        // 이미 산 커스터마이징 슬롯이라면 적용
        if (slot.Data.isPurchase)
        {
            currentWadeSlot.Data.isPick = false;
            ApplyCustomizing(currentWadeSlot.Data);
        }

        // 이전 슬롯 초기화
        currentWadeSlot.ResetSlot();

        // 새로운 슬롯 선택
        currentWadeSlot = wadeSlotList[slot.Data.idx];
        currentWadeSlot.SelectSlot();

        // 아바타 색상 적용
        ApplyCustomizingAvatar(slot.Data);
    }

    #endregion

    #region 커스터마이징 구매

    public void TryPurchaseColor(CustomizingData data)
    {
        // 잼 체크
        if (data.type == CustomizingType.Ember)
        {
            // fire Jem Count 비교
            if (GameManager.Instance.GetFireGemCount() <= data.price)
            {
                Debug.Log("Fire Jem이 부족합니다");
                return;
            }
        }
        else
        {
            // fire Jem Count 비교
            if (GameManager.Instance.GetWaterGemCount() <= data.price)
            {
                Debug.Log("Water Jem이 부족합니다");
                return;
            }
        }

        PurchaseColor(data);
    }

    private void PurchaseColor(CustomizingData data)
    {
        if (data.type == CustomizingType.Ember)
        {
            GameManager.Instance.UseFireGem(data.price);
        }
        else
        {
            GameManager.Instance.UseWaterGem(data.price);
        }
        JemCount -= data.price;

        // Jem 적용
        UpdateJem();

        // 데이터 구매 적용
        data.isPurchase = true;

        // 색상 적용
        ApplyCustomizing(data);
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
        // 커스터마이징 캐릭터 적용
        switch (data.type)
        {
            case CustomizingType.Ember:
                if (ember != null)
                {
                    ember.material = data.material;
                    ember.material.SetColor("_Color", new Color(data.color.r, data.color.g, data.color.b));
                }
                ResetPickSlot(data);
                break;
            case CustomizingType.Wade:
                if (wade != null)
                {
                    wade.material = data.material;
                    wade.material.SetColor("_Color", new Color(data.color.r, data.color.g, data.color.b));
                }
                ResetPickSlot(data);
                break;
            default:
                break;
        }
    }

    #endregion

    #region 커스터마이징 업데이트

    public void UpdateJem()
    {
        fireJemCount.text = GameManager.Instance.GetFireGemCount().ToString();
        waterJemCount.text = GameManager.Instance.GetWaterGemCount().ToString();
    }

    private void ResetPickSlot(CustomizingData data)
    {
        switch (data.type)
        {
            case CustomizingType.Ember:
                {
                    for (int i = 0; i < emberSlotList.Count; i++)
                    {
                        bool isPick = (i == data.idx);
                        emberSlotList[i].Data.isPick = isPick;
                        emberSlotList[i].ResetSlot();
                    }
                }
                break;
            case CustomizingType.Wade:
                for (int i = 0; i < wadeSlotList.Count; i++)
                {
                    bool isPick = (i == data.idx);
                    wadeSlotList[i].Data.isPick = isPick;
                    wadeSlotList[i].ResetSlot();
                }
                break;
            default:
                break;
        }
    }

    #endregion
}