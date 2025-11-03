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

    public void Activate(BaseController bc)
    {
        if (bc.gameObject.layer == LayerMask.NameToLayer("Ember"))
        {
            exitCalculator.AddPlayer(bc);
            exitCalculator.IsStageOver();
        }
    }

    public void Deactivate(BaseController bc)
    {
        if (bc.gameObject.layer == LayerMask.NameToLayer("Ember"))
        {
            exitCalculator.RemovePlayer(bc);
        }
    }

    
    
}
