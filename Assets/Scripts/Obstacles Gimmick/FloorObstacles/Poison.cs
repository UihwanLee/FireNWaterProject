using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour, InteractWithController
{
    public void Activate(BaseController bc)
    {
        //캐릭터 사망
        bc.Death();
        
    }
}
