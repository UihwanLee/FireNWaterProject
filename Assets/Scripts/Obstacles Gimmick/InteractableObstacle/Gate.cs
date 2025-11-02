using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, InteractableObstacle
{
    /*
     * 길이는 1 or 2로 고정
     * 수평,수직인지 결정
     * 열리는 게이트 닫히는 게이트
     * 시작점 / 종점
     * 가동 거리
     *
     *일단 신호 주고받는 구조 완성
     * 
     */
    
    [Range(1, 2)][SerializeField] private int moveRange = 2;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private bool isHorizontal = true; // 수직수평 여부
    [SerializeField] private bool openType = false; // 오픈형 폐쇄형 여부
    
    private Vector3 _closedDir;
    private Vector3 _openDir;
    private bool acttivate = false;
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
        //가동 방향 설정
        Vector3 targetPos = openType ? _closedDir: _openDir;
        Vector3 initPos = !openType ? _closedDir: _openDir;
        
       //가동
        moveRoutine = StartCoroutine(MoveGate(on?targetPos:initPos));
    }

    private IEnumerator MoveGate(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
    }
    
}
