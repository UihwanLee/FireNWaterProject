using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private GameObject avatar_ember;
    [SerializeField] private GameObject avatar_wade;

    [Header("Transform Info")]
    [SerializeField] private Transform emberColorSlotParent;
    [SerializeField] private Transform wadeColorSlotParent;

    [Header("Customzing Data")]
    [SerializeField] private List<GameObject> emberColorDataList;
    [SerializeField] private List<GameObject> wadeColorDataList;

    private void Start()
    {
        GenerateColorSlots();
    }

    private void GenerateColorSlots()
    {

    }

    public void ChoiceColor()
    {

    }

    public void PurchaseColor()
    {

    }
}
