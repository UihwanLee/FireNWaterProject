using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, InteractWithController
{
    [SerializeField] private Gate targetGate;
    [SerializeField] private float moveRange = 0.4f;
    [SerializeField] private float moveSpeed = 0.2f;
    
    private Coroutine moveRoutine;
    private SpriteRenderer _sprite;
    
    private Vector3 initPos;
    private Vector3 endPos;
    
    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        initPos = _sprite.transform.position;
        endPos = new Vector3(initPos.x, initPos.y - moveRange, initPos.z);
    }
    
    
    
    
    
    
    

    public void Activate(BaseController bc)
    {
        //게이트에 신호
        if (targetGate != null)
        {
            targetGate.Interact(true);
            moveRoutine = StartCoroutine(MoveButton(true));

        }
        
    }
    
    public void Activate(Box box)
    {
        //게이트에 신호
        if (targetGate != null)
        {
            targetGate.Interact(true);
            moveRoutine = StartCoroutine(MoveButton(true));
        }
    }
   
    public void Recovery()
    {
        //게이트에 신호
        if (targetGate != null)
        {
            targetGate.Interact(false);
            moveRoutine = StartCoroutine(MoveButton(false));
        }
        
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
