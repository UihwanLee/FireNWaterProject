using TMPro;
using UnityEngine;


public class StageMapUI : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _buttonParent;
    [SerializeField] private float _spacing;

    private void Start()
    {
        int colums = 5;
        Vector2 startPos = new Vector2(-700f, 0f);

        for (int i = 0; i < GameManager.STAGE_NUM; i++)
        {
            GameObject btnObj = Instantiate(_buttonPrefab, _buttonParent);
            RectTransform rect = btnObj.GetComponent<RectTransform>();

            int row = i / colums;
            int col = i % colums;

            rect.anchoredPosition = new Vector2(
                startPos.x + (col * _spacing),
                startPos.y - (row * _spacing)
             );

            btnObj.name = $"StageButton_{i + 1}";
            UnityEngine.UI.Button btn = btnObj.GetComponent<UnityEngine.UI.Button>();

            TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = (i + 1).ToString();

            btn.onClick.AddListener(() => OnClickButtoon(i));
        }
    }

    private void OnClickButtoon(int stageNum)
    {
        Debug.Log($"{stageNum}번째 스테이지 선택");
        GameManager.Instance.SelectStage(stageNum);
    }
}
