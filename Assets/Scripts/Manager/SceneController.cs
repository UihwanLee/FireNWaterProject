using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    public enum SceneType
    {
        StartScene,
        StageScene,
        EndScene
    }

    private Dictionary<SceneType, string> sceneNames = new Dictionary<SceneType, string>()
    {
        { SceneType.StartScene, "StartScene" },
        { SceneType.StageScene, "StageScene" },
        { SceneType.EndScene, "EndScene" }
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //씬 이동
    public void LoadScene(SceneType type)
    {
        string sceneName = sceneNames[type];
        SceneManager.LoadScene(sceneName);
    }

    //게임 종료 기능
    public void ExitGame()
    {
        Debug.Log("게임 종료");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
