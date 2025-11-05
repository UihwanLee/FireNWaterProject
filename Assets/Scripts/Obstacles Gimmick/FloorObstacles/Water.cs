using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour , InteractWithController
{ 
        
    public void Activate(BaseController ember)
    {
      //캐릭터 사망
      ember.Death();
    }
    
}
