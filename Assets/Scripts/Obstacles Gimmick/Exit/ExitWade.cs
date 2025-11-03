using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitWade : MonoBehaviour, InteractWithController
{
    ExitCalculator exitCalculator;

    private void Awake()
    {
        exitCalculator = GetComponentInParent<ExitCalculator>();
    }

    public void Activate(BaseController bc)
    {
        if (bc.gameObject.layer == LayerMask.NameToLayer("Wade"))
        {
            exitCalculator.AddPlayer(bc);
            exitCalculator.IsStageOver();
        }
    }

    public void Deactivate(BaseController bc)
    {
        if (bc.gameObject.layer == LayerMask.NameToLayer("Wade"))
        {
            exitCalculator.RemovePlayer(bc);
        }
    }
}
