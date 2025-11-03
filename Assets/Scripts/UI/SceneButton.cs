using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButton : MonoBehaviour
{
    public SceneController.SceneType targetScene;

    public void OnClickLoadScene()
    {
        SceneController.Instance.LoadScene(targetScene);
    }
}
