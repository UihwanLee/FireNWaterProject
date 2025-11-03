using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingSlot : MonoBehaviour
{
    [Header("Slot Component")]
    [SerializeField] private Button btn_slot;
    [SerializeField] private Image btn_highlight;
    [SerializeField] private TextMeshProUGUI txt_price;
    [SerializeField] private Button btn_purchase;

    public void InitSlot()
    {

    }

}
