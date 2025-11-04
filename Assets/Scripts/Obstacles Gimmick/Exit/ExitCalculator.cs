using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ExitCalculator : MonoBehaviour , IObstacle
{
    //리스트 중복 방지
    private HashSet<BaseController> players = new();
    private bool cleared;
    SpriteRenderer spriteRenderer;
    
    public void Init()
    {
        Logger.Log("Exit  초기화");
        //플레이어 알파값 초기화
        foreach (var p in players)
        {
            spriteRenderer = p.GetComponentInChildren<SpriteRenderer>();
            var color = spriteRenderer.color;
            color.a = 1;
            spriteRenderer.color = color;
        }
        //출구 초기화
        var eE = exitEmber.GetComponentInChildren<SpriteRenderer>();
        var eW = exitWade.GetComponentInChildren<SpriteRenderer>();
        var cEe = eE.color;
        var cEw = eW.color;
        cEe.a = 1;
        cEw.a = 1;
        eE.color = cEe;
        eW.color = cEw;
    }
    
    public void AddPlayer(BaseController player)
    {
        players.Add(player);
    }

    public void RemovePlayer(BaseController player)
    {
        players.Remove(player);
    }
    
    public void IsStageOver()
    {
        if (cleared) return;            // 이미 끝났으면 무시
        if (players.Count >= 2)
        {
            cleared = true;
            
            EndingAction(players);
            
            GameManager.Instance.ClearStage();  // 딱 한 번만 호출
        }
    }

    
     [SerializeField] private float exitDurationTime = 1.5f;
     [SerializeField] private GameObject exitEmber;
     [SerializeField] private GameObject exitWade;


     private void EndingAction(HashSet<BaseController> p)
    {
        if (p.Count != 2) return;
        
        

        foreach (BaseController player1 in p)
        {
            StartCoroutine(CharacterFadeOut(player1, exitDurationTime));
        }

        StartCoroutine(ExitFadeOut(exitEmber, exitDurationTime));
        StartCoroutine(ExitFadeOut(exitWade, exitDurationTime));

    }

    //출구 페이드아웃
    IEnumerator ExitFadeOut(GameObject go ,float duration)
    {
        var spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
        
        if (spriteRenderer == null) yield break;
        
        Color startColor = spriteRenderer.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0);
            
        float timer = 0.0f;

        while (timer < duration)
        {
            var t = timer / duration;
            spriteRenderer.color = Color.Lerp(startColor, targetColor, t);
            
            yield return null;
            timer += Time.deltaTime;
        }
        spriteRenderer.color = targetColor;
        
    }
    
    //캐릭터 페이드아웃
    IEnumerator CharacterFadeOut(BaseController bc, float duration)
    {
        var spriteRenderer = bc.GetComponentInChildren<SpriteRenderer>();
        var rigidBody2D = bc.GetComponentInChildren<Rigidbody2D>();
        
        //중단 조건 
        if (spriteRenderer == null || rigidBody2D == null || duration ==0 ) yield break;
        
        //초기값
        Color startColor = spriteRenderer.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        
        Vector2 startV = rigidBody2D.velocity;
        
        float timer = 0.0f;

        while (timer < duration)
        {
            var t = timer / duration;
            
            spriteRenderer.color = Color.Lerp(startColor, targetColor, t);

            rigidBody2D.velocity = Vector2.Lerp(startV, Vector2.zero, t);
            
            yield return null;
            timer += Time.deltaTime;
        }
        spriteRenderer.color = targetColor;
        rigidBody2D.velocity = Vector2.zero;
    }
    
    public void DebugPlayers()
    {
        foreach (var bc in players)
        {
            Debug.Log($"{bc.name}");
        }   
    }
    
}
