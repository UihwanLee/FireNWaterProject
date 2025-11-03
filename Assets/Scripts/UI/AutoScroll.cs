using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.05f;

    private void Start()
    {
        if (scrollRect == null)
            scrollRect = GetComponent<ScrollRect>();

        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void Update()
    {
        if (scrollRect.verticalNormalizedPosition > 0f)
        {
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;
        }
        else
        {
            scrollRect.verticalNormalizedPosition = 0f;
            enabled = false;
            Debug.Log("스크롤 종료");
        }
    }
}