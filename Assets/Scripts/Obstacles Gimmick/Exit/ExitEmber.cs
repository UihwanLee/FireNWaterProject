using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitEmber : MonoBehaviour, InteractWithController
{
    
    public void Activate(BaseController bc)
    {
        if (bc.gameObject.layer == LayerMask.NameToLayer("Ember"))
        {
            //게임 매니저에 "엠버 진입" 신호
        }
    }

    public void Deactivate()
    {
        // 게임 매니저에 "엠버 탈주" 신호
    }
}
