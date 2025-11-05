using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lava : MonoBehaviour , InteractWithController
{ 
        public void Activate(BaseController wade)
        {
                //캐릭터 사망
                wade.Death();
        }
    
}

