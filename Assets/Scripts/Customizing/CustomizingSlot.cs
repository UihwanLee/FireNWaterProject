using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingSlot : MonoBehaviour
{
    [Header("Slot Component")]
    [SerializeField] private GameObject btn_slot;
    [SerializeField] private Image img_highlight;
    [SerializeField] private GameObject object_jem;
    [SerializeField] private TextMeshProUGUI txt_price;
    [SerializeField] private GameObject btn_purchase;
    [SerializeField] private GameObject img_purchase_icon;

    [SerializeField] CustomizingData data;

    Image img_slot;

    public void Init(int idx, CustomizingData data)
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

        this.data = data;

        ResetSlot();
    }

    public void ResetSlot()
    {
        // Slot 리셋
        img_purchase_icon.SetActive(data.isPurchase);
        img_highlight.transform.gameObject.SetActive(false);
        object_jem.SetActive(!data.isPurchase);
        txt_price.transform.gameObject.SetActive(!data.isPurchase);
        btn_purchase.transform.gameObject.SetActive(false);
    }

    public void SelectSlot()
    {
        img_highlight.transform.gameObject.SetActive(true);
        btn_purchase.transform.gameObject.SetActive(!data.isPurchase);
    }

    public CustomizingData Data { get { return data; } set { data = value; } }
}
