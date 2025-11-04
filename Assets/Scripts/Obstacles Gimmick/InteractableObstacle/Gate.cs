using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IObstacle
{
    [Header("종점 기준으로 배치해주세요 ")]
    [Range(1, 20)][SerializeField] private int moveRange = 3;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private bool isHorizontal = true; // 수직수평 여부
    [Header("이동방향: OpenType 체크 -> 왼쪽 / 해제 -> 오른족 ")]
    [SerializeField] private bool openType = false; // 오픈형 폐쇄형 여부
    
   // 데이터 입력 셋
   private readonly HashSet<GameObject> _inside = new();
    
    //종점,시점
    private Vector3 _closedDir;
    private Vector3 _openDir;
    
    //초기화 좌표
    [SerializeField] private Vector3 _originPosition;
    
    //코루틴 종점 시점
    private Vector3 iPos;
    private Vector3 tPos;
    bool _isOpen;   
    private Coroutine moveRoutine;
    
    private int activeCount = 0;  // 현재 Gate를 활성화 중인 버튼 수

    private bool isPusing;
   
    
    public void Init()
    {
        gameObject.GetComponentInChildren<Transform>().localPosition =  Vector3.zero;
        gameObject.transform.position = _originPosition;
    }
    
    private void Awake()
    {
        //초기화: 게이트가 닫혀있을 때 기준
        _originPosition = gameObject.transform.localPosition;       
       
        //종점 시점 설정
        Vector3 initPos = transform.position;
        if (isHorizontal)
        {
            _openDir = initPos;
            _closedDir = new Vector3(initPos.x - moveRange, initPos.y, initPos.z);
        }
        else
        {
            _openDir = initPos;
            _closedDir = new Vector3(initPos.x , initPos.y- moveRange, initPos.z);
        }
        
        tPos = openType ? _closedDir: _openDir;
        iPos = !openType ? _closedDir: _openDir;
        
        //폐쇄형일 경우 시작점 보정
        if (isHorizontal&&!openType)
        {
            initPos.x -= moveRange;
            transform.position = initPos;
        }
        if (!isHorizontal && !openType)
        {
            initPos.y -= moveRange;
            transform.position = initPos;
        }
        
    }
    
    
    public void Interact( bool on) 
    {
        //활성 객체 수 카운트
        if (on) activeCount++;
        else activeCount--;
        
        if (activeCount < 0) activeCount = 0;
        
        //활성자가 있으면 열어라 
        bool shouldOpen = activeCount > 0;
        //상태변화 없으면 리턴
        if (shouldOpen == _isOpen) return;
        
        _isOpen = on;
        
       //가동
       if (moveRoutine != null)
       {
           StopCoroutine(moveRoutine);
       } 
       moveRoutine = StartCoroutine(MoveGate(on? tPos :iPos));

    }

    private IEnumerator MoveGate(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        moveRoutine = null;
    }
    
    
}
