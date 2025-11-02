using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, InteractableObstacle
{
    [Range(1, 2)][SerializeField] private int moveRange = 2;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private bool isHorizontal = true; // 수직수평 여부
    [SerializeField] private bool openType = false; // 오픈형 폐쇄형 여부
    
   
    //종점,시점
    private Vector3 _closedDir;
    private Vector3 _openDir;
    
    //코루틴 종점 시점
    private Vector3 iPos;
    private Vector3 tPos;
    
    bool _isOpen;   
    private Coroutine moveRoutine;
   
    private void Awake()
    {
        //초기화: 게이트가 닫혀있을 때 기준
        
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

    
    
    public void Interact(bool on)
    {
        
        if (on == _isOpen) return;
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
