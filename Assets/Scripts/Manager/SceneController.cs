using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    StartScene,
    StageScene,
    EndScene
}

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

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

    //�� �̵�
    public void LoadScene(SceneType type)
    {
        if (type == SceneType.StageScene)
        {
            GameManager.Instance.LoadStageScene();
        }
        string sceneName = sceneNames[type];
        SceneManager.LoadScene(sceneName);
    }

    //���� ���� ���
    public void ExitGame()
    {
        Debug.Log("���� ����");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
