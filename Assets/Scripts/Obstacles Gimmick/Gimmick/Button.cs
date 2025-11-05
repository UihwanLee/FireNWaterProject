using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    
    [Header("누를 수 있는 레이어")]
    [SerializeField] private LayerMask pusherLayers;  
    
    [SerializeField] private Gate targetGate;
    [SerializeField] private float moveRange = 0.4f;
    [SerializeField] private float moveSpeed = 0.2f;

    private SpriteRenderer _sprite;
    
    private readonly HashSet<GameObject> _inside = new(); // 현재 트리거 안에 있는 오브젝트
    
    private bool _pressed;

    private Coroutine moveRoutine;
    
    private Vector3 initPos; //시점 선언
    private Vector3 endPos; //종점 선언
    
    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        initPos = _sprite.transform.position;
        endPos = new Vector3(initPos.x, initPos.y - moveRange, initPos.z);
    }

    //pusher 등록 및 동작
    public void RegisterPusher(GameObject pusher)
    {
        if (!IsPusher(pusher)) return;
        if (_inside.Add(pusher))
        {
            EvaluateNow();
        }
    }

    //pusher 해제 및 동작
    public void UnregisterPusher(GameObject pusher)
    {
        if (!IsPusher(pusher)) return;
        if (_inside.Remove(pusher))
        {
            StopCoroutine(moveRoutine);
            EvaluateNow();
        }
    }
    
    //Pusher 조건 검사
    private bool IsPusher(GameObject go)
        => ((1 << go.layer) & pusherLayers) != 0;
    
    //상태변화 감지, 신호 및 동작 제어
    private void EvaluateNow()
    {
        bool nextPressed = (_inside.Count > 0);
        if (nextPressed == _pressed) return;  // 상태 변화시에만

        _pressed = nextPressed;

        if (!gameObject.activeInHierarchy)
        {
            Logger.Log("비활성화라 코루틴 실행 안됨");
            return;
        }

        Logger.Log("평가 실행");
        // 1) 게이트 신호 — 단 한 번
        if (targetGate != null)
        {
            targetGate.Interact(_pressed);
        }

        // 2) 버튼 애니 — 코루틴 1개만 유지
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveButton(_pressed));
    }
    
    
    private IEnumerator MoveButton(bool on)
    {
        Vector3 targetPos = on ? endPos : initPos;

        while (Vector3.Distance(_sprite.transform.position, targetPos) > 0.01)
        {
            _sprite.transform.position = Vector3.MoveTowards(_sprite.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        _sprite.transform.position = targetPos;
    }
    
    
}
