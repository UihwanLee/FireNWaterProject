using TMPro;
using UnityEngine;


public class StageUI : MonoBehaviour
{
    [Header("자식 오브젝트")]
    [SerializeField] private GameObject _bgImg;
    [SerializeField] private GameObject _buttonParent;
    [SerializeField] private GameObject _customizingUI;
    [SerializeField] private GameObject _timerUI;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("스테이지 선택 버튼")]
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private float _spacing;

    // todo: 이전 스테이지 클리어 되어야지만 다음 스테이지 가능

    private void Start()
    {
        CreateStageButtons();
    }

    #region UI Show/Close
    public void ShowStageMapUI()
    {
        _bgImg.SetActive(true);
        _buttonParent.SetActive(true);
    }

    public void CloseStageMapUI()
    {
        _bgImg.SetActive(false);
        _buttonParent.SetActive(false);
    }

    public void ShowCustomizingUI()
    {
        if (_customizingUI.activeSelf) return;
        _customizingUI.SetActive(true);
    }

    public void CloseCustomizingUI()
    {
        if (!_customizingUI.activeSelf) return;
        _customizingUI.SetActive(false);
    }

    public void ShowTimerUI()
    {
        if (_timerUI.activeSelf) return;
        _timerUI.SetActive(true);
    }

    public void CloseTimeUI()
    {
        if (!_timerUI.activeSelf) return;
        _timerUI.SetActive(false);
    }
    #endregion

    public void UpdateTime(float time)
    {
        if (!_timerUI.activeSelf) return;
        _timerText.text = time.ToString("n2");
    }

    private void CreateStageButtons()
    {
        int colums = 5;
        Vector2 startPos = new Vector2(-700f, 0f);

        Transform buttonParentTransform = _buttonParent.transform;

        for (int i = 0; i < GameManager.STAGE_NUM; i++)
        {
            GameObject btnObj = Instantiate(_buttonPrefab, buttonParentTransform);
            RectTransform rect = btnObj.GetComponent<RectTransform>();

            int row = i / colums;
            int col = i % colums;

            rect.anchoredPosition = new Vector2(
                startPos.x + (col * _spacing),
                startPos.y - (row * _spacing)
             );

            btnObj.name = $"StageButton_{i + 1}";
            UnityEngine.UI.Button btn = btnObj.GetComponent<UnityEngine.UI.Button>();

            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = (i + 1).ToString();

            int stageNum = i;
            btn.onClick.AddListener(() =>
            {
                Logger.Log($"{btn.name} 선택");
                OnClickButtoon(stageNum);
            });
        }
    }

    private void OnClickButtoon(int stageNum)
    {
        CloseStageMapUI();
        Debug.Log($"{stageNum}번째 스테이지 선택");
        GameManager.Instance.SelectStage(stageNum);
    }
}
