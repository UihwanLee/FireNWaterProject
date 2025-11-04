using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [Header("튜토리얼 패널들 순서대로 넣기")]
    [SerializeField] private GameObject[] guides;

    [Header("게임 시작하면 1번부터 보여줄까?")]
    [SerializeField] private bool showFirstOnStart = true;

    private int _currentIndex = -1;
    private bool _isRunning = false;

    private void Awake()
    {
        // 혹시 켜져있을 수 있으니 전부 꺼두기
        HideAll();
    }
    private void Start()
    {
        // 씬이 시작되자마자 1번은 무조건 보이게 하고 싶을 때
        if (showFirstOnStart)
        {
            _isRunning = true;
            _currentIndex = 0;
            ShowCurrent();
        }
    }

    private void HideAll()
    {
        if (guides == null) return;
        for (int i = 0; i < guides.Length; i++)
        {
            if (guides[i] != null)
                guides[i].SetActive(false);
        }
    }

    /// <summary>
    /// StageManager에서 호출해서 튜토리얼을 시작하게 할 메서드
    /// </summary>
    public void StartTutorial()
    {
        if (guides == null || guides.Length == 0)
            return;

        _isRunning = true;
        _currentIndex = 0;
        ShowCurrent();
    }

    /// <summary>
    /// 튜토리얼 패널을 클릭했을 때 Button OnClick에 이걸 연결하면 됨
    /// </summary>
    public void OnClickNextGuide()
    {
        if (!_isRunning) return;

        _currentIndex++;

        if (_currentIndex >= guides.Length)
        {
            // 다 보여줬으면 종료
            HideAll();
            _isRunning = false;
            return;
        }

        ShowCurrent();
    }

    private void ShowCurrent()
    {
        // 전부 끄고
        HideAll();

        // 현재 것만 켜기
        if (_currentIndex >= 0 && _currentIndex < guides.Length)
        {
            if (guides[_currentIndex] != null)
                guides[_currentIndex].SetActive(true);
        }
    }
}