using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, InteractWithController, InteractableObstacle
{
    [SerializeField] private Gate targetGate;
    public void Activate(BaseController bc)
    {
        //애니메이션 작동하고
    }

    public void Interact()
    {
        //게이트에 신호
        targetGate.Interact();
    }
   
    public void Recovery()
    {
        //애니메이션 작동하고
        //게이트에 신호보내고
    }
    
    
}
