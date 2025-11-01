using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, InteractWithController
{
    [SerializeField] private Gate targetGate;
    
    public Animator animator;
    public void Activate(BaseController bc)
    {
        //애니메이션 및 동작
        if(animator != null)animator.SetTrigger("Activate");
        Debug.Log("push animation");
        
    }

    public void Interact()
    {
        //게이트에 신호
        if (targetGate != null)
        {
            targetGate.Interact(true);
        }
    }
   
    public void Recovery()
    {
        //애니메이션 작동하고
        if(animator != null)animator.SetTrigger("Recover");
        //게이트에 신호보내고
        Debug.Log("recovery animation");
    }
    
    
}
