using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButton : MonoBehaviour
{
    public SceneController.SceneType targetScene;

    public void OnClickLoadScene()
    {
        if (targetScene == SceneController.SceneType.StageScene)
        {
            GameManager.Instance.LoadStageScene();
        }
        SceneController.Instance.LoadScene(targetScene);
    }
}
