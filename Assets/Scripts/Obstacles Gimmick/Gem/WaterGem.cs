using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watergem : BaseGem, InteractWithController
{
    SpriteRenderer spriteRenderer;
    private Color currentColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentColor = spriteRenderer.color;
    }

    public void Activate(BaseController wade)
    {
        //잼 먹고 
        currentColor.a = 0f;
        spriteRenderer.color =  currentColor;
        //점수 체크 
        GameManager.Instance.AddWaterGem();
        //비활성화
        gameObject.SetActive(false);
    }

}
