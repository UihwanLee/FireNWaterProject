using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private GameObject _star3;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _lose;

    [Header("버튼")]
    [SerializeField] private UnityEngine.UI.Button _restartButton;
    [SerializeField] private UnityEngine.UI.Button _homeButton;
    [SerializeField] private UnityEngine.UI.Button _nextButton;

    private List<GameObject> _results;

    private void Awake()
    {
        // StageScore: [0]A, [1]B, [2]C, [3]None
        _results = new List<GameObject> { _star3, _star2, _star1, _lose };
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(() => GameManager.Instance.StartStage());
        _homeButton.onClick.AddListener(() => GameManager.Instance.ExitStage());
        _nextButton.onClick.AddListener(() => GameManager.Instance.StartNextStage());
    }

    public void Activate(StageScore stageScore)
    {
        DeactivateAll();

        _results[(int)stageScore].SetActive(true);
        _restartButton.gameObject.SetActive(true);
        _homeButton.gameObject.SetActive(true);
        if (StageScore.None == stageScore) return;
        _nextButton.gameObject.SetActive(
            GameManager.Instance.MaxClearStageId < Define.STAGE_NUM - 1
            );
    }

    public void DeactivateAll()
    {
        _star3.SetActive(false);
        _star2.SetActive(false);
        _star1.SetActive(false);
        _lose.SetActive(false);
        _restartButton.gameObject.SetActive(false);
        _homeButton.gameObject.SetActive(false);
        _nextButton.gameObject.SetActive(false);
    }
}
