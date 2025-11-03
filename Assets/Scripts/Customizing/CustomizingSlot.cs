using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingSlot : MonoBehaviour
{
    [Header("Slot Component")]
    [SerializeField] private GameObject btn_slot;
    [SerializeField] private Image img_highlight;
    [SerializeField] private Image img_pick;
    [SerializeField] private GameObject object_jem;
    [SerializeField] private TextMeshProUGUI txt_price;
    [SerializeField] private GameObject btn_purchase;
    [SerializeField] private GameObject img_purchase_icon;

    [SerializeField] CustomizingData data;

    Image img_slot;

    public void Init(int idx, CustomizingData data, CustomizingManager manager)
    {
        // Material 적용
        img_slot = btn_slot.GetComponent<Image>();
        Material mat = new Material(img_slot.material);
        img_slot.material = mat;

        // 색상 적용
        mat.color = new Color(data.color.r, data.color.g, data.color.b);

        // Idx 적용
        data.idx = idx;

        // 가격 적용
        txt_price.text = data.price.ToString();

        // 슬롯 버튼 적용
        UnityEngine.UI.Button btn = btn_slot.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(() => manager.ChoiceColorSlot(this));

        // 구매 버튼 적용
        btn = btn_purchase.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(() => manager.TryPurchaseColor(data));

        this.data = data;

        ResetSlot();
    }

    public void ResetSlot()
    {
        // 선택 여부
        img_highlight.transform.gameObject.SetActive(false);

        // 구매 여부
        object_jem.SetActive(!data.isPurchase);
        img_purchase_icon.SetActive(data.isPurchase);
        txt_price.transform.gameObject.SetActive(!data.isPurchase);
        btn_purchase.transform.gameObject.SetActive(false);

        // 착용 여부
        img_pick.transform.gameObject.SetActive(data.isPick);
    }

    public void SelectSlot()
    {
        img_highlight.transform.gameObject.SetActive(true);
        btn_purchase.transform.gameObject.SetActive(!data.isPurchase);
    }

    public CustomizingData Data { get { return data; } set { data = value; } }
}
