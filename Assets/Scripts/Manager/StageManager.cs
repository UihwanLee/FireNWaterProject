using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("캐릭터")]
    [SerializeField] private GameObject _ember;
    [SerializeField] private GameObject _wade;

    private GameManager _gameManager;
    private Dictionary<int, StageController> _stages = new();
    private StageController _currentStage;

    // 측정할 정보
    private float _timer = 0f;
    public float Timer
    {
        get => _timer;
        private set
        {
            if (_currentStage.LimitTime < value)
            {
                // todo: limit time이랑 비교하기
            }
            _timer = value;
        }
    }

    private void Awake()
    {
        if (_ember == null)
        {
            // 엠버 프리팹 원격으로 가져오기
            Logger.Log("엠버 프리팹 가져오기");
        }

        if (_wade == null)
        {
            // 웨이드 프리팹 원격으로 가져오기
            Logger.Log("웨이드 프리팹 가져오기");
        }

        _ember.SetActive(false);
        _wade.SetActive(false);
        Logger.Log("엠버, 웨이드 초기화 및 비활성화 완료");
    }

    // 초기화
    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;

        var stages = GetComponentsInChildren<StageController>(true);    // 비활성화된 object도 탐색

        _stages.Clear();
        foreach (var stage in stages)
        {
            int id = stage.StageId;

            if (_stages.ContainsKey(id))
            {
                Debug.LogError($"[StageManager.Init] {id} 번째 스테이지 중복 할당");
                continue;
            }

            _stages[id] = stage;                                        // dict 채우기
            stage.gameObject.SetActive(false);                          // 모두 비활성화 하기
        }

        _gameManager.OnGameStateChanged += HandleStateChanged;
        Debug.Log($"[StageManager.Init] 등록된 Stage 수: {_stages.Count}");
    }

    private void Update()
    {
        if (_gameManager.CurrentGameState == GameState.Play)
        {
            // 타이머 돌아가는 로직 작성
            Timer += Time.deltaTime;
        }
        //Logger.Log($"시간: {Timer.ToString()}");
    }

    private void OnEnable()
    {
        Debug.Log("[StageManager] OnEnable 호출됨");
    }

    private void OnDisable()
    {
        Debug.Log("[StageManager] OnDisable 호출됨");
        _gameManager.OnGameStateChanged -= HandleStateChanged;
    }

    // 스테이지 선택
    public void SelectStage(int id)
    {
        if (!_stages.TryGetValue(id, out var stage))
        {
            Logger.LogWarning($"{id} 번째 스테이지 존재하지 않음");
            return;
        }

        if (_currentStage != null)
        {
            _currentStage.gameObject.SetActive(false);          // 이전 스테이지 확인
        }

        _currentStage = _stages[id];
        _currentStage.gameObject.SetActive(true);               // 활성화
        Logger.Log($"{id} 번째 스테이지 활성화");
        _gameManager.ChangeGameState(GameState.Start);          // 자동 시작
    }

    /// <summary>
    /// 게임 상태에 따라 다른 로직을 처리하기 위한 메서드
    /// 유효성 검사를 위해 GameManager의 ChangeGameState 사용을 권장
    /// </summary>
    /// <param name="state"></param>
    private void HandleStateChanged(GameState state)
    {
        if (_currentStage == null)
        {
            Logger.LogWarning($"현재 저장된 stage 없음");
        }

        switch (state)
        {
            case GameState.Start:                   // 카운트 다운, 로딩 등
                StartStage();
                break;
            case GameState.Play:                    // 실제 플레이(조작, 점수/시간 측정)
                Logger.Log("플레이 중");
                break;
            case GameState.Stop:                    // 조작 불가, 시간 멈춤, 메뉴 표시
                PauseStage();
                break;
            case GameState.Dead:                    // 실패, 재시작 대기
                GameOver();
                break;
            case GameState.Clear:                   // 성공, 점수 계산
                ClearStage();
                break;
            case GameState.End:                     // 맵으로 나가기
                ExitStage();
                break;
            case GameState.Next:                    // 다음 스테이지
                NextStage();
                break;
            default:
                break;
        }
    }

    private void StartStage()
    {
        ResetStageInfo();
        SetPlayerActive(true);
        _currentStage.SetSpawnPoint(_ember, _wade);
        _gameManager.ChangeGameState(GameState.Play);
    }

    public void PauseStage()
    {
        _currentStage.PauseStage();
    }

    public void GameOver()
    {
        ResetStageInfo();
        _currentStage.GameOver();
    }

    public void ExitStage()
    {
        ResetStageInfo();
        SetPlayerActive(false);
        _currentStage.gameObject.SetActive(false);  // 비활성화
        _currentStage = null;

        _gameManager.ChangeGameState(GameState.None);
    }

    public void ClearStage()
    {
        _currentStage.ClearStage();
        _currentStage.CheckScore();
    }

    public void NextStage()
    {
        int id = _currentStage.StageId;
        SelectStage(id + 1);                        // 예외처리는 SelectStage에서 진행
    }

    private void ResetStageInfo()
    {
        Timer = 0f;
    }

    private void SetPlayerActive(bool active)
    {
        Logger.Log($"Ember, Wade {(active ? "활성화" : "비활성화")}");
        _ember.SetActive(active);
        _wade.SetActive(active);
    }
}
