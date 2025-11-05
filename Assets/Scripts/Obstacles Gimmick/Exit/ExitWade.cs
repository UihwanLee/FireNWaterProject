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

    // [ 엠버-출구 ] 상호작용
    public void Activate(BaseController bc)
    {
        //웨이드 입장 시 체크
        if (bc.gameObject.layer == LayerMask.NameToLayer("Wade"))
        {
            exitCalculator.AddPlayer(bc);
            exitCalculator.IsStageOver();
        }
    }

    public void Deactivate(BaseController bc)
    {
        //웨이드 퇴장 시 체크
        if (bc.gameObject.layer == LayerMask.NameToLayer("Wade"))
        {
            exitCalculator.RemovePlayer(bc);
        }
    }
}
