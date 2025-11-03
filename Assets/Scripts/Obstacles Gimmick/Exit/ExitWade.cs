using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitWade : MonoBehaviour, InteractWithController
{
    public void Activate(BaseController bc)
    {
        if (bc.gameObject.layer == LayerMask.NameToLayer("Wade"))
        {
            //게임 매니저에 "웨이드 진입" 신호
        }
    }

    public void Deactivate()
    {
        // 게임 매니저에 "웨이드 탈주" 신호
    }
}
//트리거 동시에 플레이어가 존재하면 게임매니저에 게임종료 로직 호출