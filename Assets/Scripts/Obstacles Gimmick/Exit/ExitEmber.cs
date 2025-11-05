using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitEmber : MonoBehaviour, InteractWithController
{
    ExitCalculator exitCalculator;

    private void Awake()
    {
        exitCalculator = GetComponentInParent<ExitCalculator>();
    }
    
    // [ 엠버-출구 ] 상호작용
    public void Activate(BaseController bc)
    {
        //엡버 입장 시 체크
        if (bc.gameObject.layer == LayerMask.NameToLayer("Ember"))
        {
            exitCalculator.AddPlayer(bc);
            exitCalculator.IsStageOver();
        }
    }

    public void Deactivate(BaseController bc)
    {
        //엠버 퇴장 시 체크
        if (bc.gameObject.layer == LayerMask.NameToLayer("Ember"))
        {
            exitCalculator.RemovePlayer(bc);
        }
    }

    
    
}
