using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private GameObject _star3;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _lose;

    [Header("버튼")]
    [SerializeField] private UnityEngine.UI.Button _restartButton;
    [SerializeField] private UnityEngine.UI.Button _nextButton;

    public void Activate(StageScore stageScore)
    {
        DeactivateAll();

        switch (stageScore)
        {
            case StageScore.A:
                _star3.SetActive(true);
                break;
            case StageScore.B:
                _star2.SetActive(true);
                break;
            case StageScore.C:
                _star1.SetActive(true);
                break;
            default:
                _lose.SetActive(true);
                break;
        }
    }

    public void DeactivateAll()
    {
        _star3.SetActive(false);
        _star2.SetActive(false);
        _star1.SetActive(false);
        _lose.SetActive(false);
    }
}
