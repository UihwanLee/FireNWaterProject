using TMPro;
using UnityEngine;


public class StageUI : MonoBehaviour
{
    [Header("배경")]
    [SerializeField] private GameObject _bgImg;
    [SerializeField] private GameObject _logo;
    [SerializeField] private GameObject _bgEndAnim;

    [Header("커스터마이징")]
    [SerializeField] private GameObject _customizingUI;

    [Header("타이머")]
    [SerializeField] private GameObject _timerUI;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("스테이지 결과")]
    [SerializeField] private ResultUI _resultUI;

    [Header("스테이지 선택 버튼")]
    [SerializeField] private GameObject _buttonParent;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private float _spacing;

    // todo: 이전 스테이지 클리어 되어야지만 다음 스테이지 가능

    private void Start()
    {
        ConnectButtonAndStage();
    }

    #region Stage Map UI
    public void ShowStageMapUI()
    {
        if (GameManager.Instance.MaxClearStageId == Define.STAGE_NUM - 1)
        {
            _bgEndAnim.SetActive(true);
        }
        else
        {
            _bgImg.SetActive(true);
        }
        _logo.SetActive(true);
        _buttonParent.SetActive(true);
    }

    public void CloseStageMapUI()
    {
        _bgEndAnim.SetActive(true);
        _bgImg.SetActive(false);
        _logo.SetActive(false);
        _buttonParent.SetActive(false);
    }

    private void ConnectButtonAndStage()
    {
        var btnObjs = _buttonParent.GetComponentsInChildren<UnityEngine.UI.Button>();

        foreach (var btnObj in btnObjs)
        {
            btnObj.TryGetComponent(out StageSelectButton btn);
            int buttonId = btn.buttonId;

            if (buttonId == Define.STAGE_NUM)
            {
                btnObj.onClick.AddListener(OnClickEndButton);
                return;
            }

            btnObj.onClick.AddListener(() => OnClickStageSelectButton(buttonId));
        }
    }

    private void CreateStageButtons()
    {
        int colums = 5;
        Vector2 startPos = new Vector2(-700f, 0f);

        Transform buttonParentTransform = _buttonParent.transform;

        for (int i = 0; i < Define.STAGE_NUM; i++)
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
                OnClickStageSelectButton(stageNum);
            });
        }
    }

    private void OnClickStageSelectButton(int stageNum)
    {
        Debug.Log($"{stageNum}번째 스테이지 선택");
        GameManager.Instance.SelectStage(stageNum);
    }

    private void OnClickEndButton()
    {
        Debug.Log("End 크레딧 보기");
        // todo: 엔딩 씬으로 넘어가기
    }
    #endregion

    #region Customizing UI
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
    #endregion

    #region Timer UI 
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

    public void UpdateTime(float time)
    {
        if (!_timerUI.activeSelf) return;
        _timerText.text = time.ToString("n2");
    }
    #endregion

    #region Result UI
    public void ShowResultUI(StageScore score)
    {
        _resultUI.Activate(score);
    }

    public void CloseResultUI()
    {
        _resultUI.DeactivateAll();
    }
    #endregion
}
