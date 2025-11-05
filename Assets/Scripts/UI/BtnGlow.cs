using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BtnGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage;
    public Sprite normalSprite;
    public Sprite glowSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null && glowSprite != null)
            targetImage.sprite = glowSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null && normalSprite != null)
            targetImage.sprite = normalSprite;
    }
}