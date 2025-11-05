using UnityEngine;

public class SceneButton : MonoBehaviour
{
    public enum ButtonType { LoadScene, ExitGame }

    public ButtonType buttonType;
    public SceneType targetScene;

    private UnityEngine.UI.Button btn;

    private void Awake()
    {
        btn = GetComponent<UnityEngine.UI.Button>();
        if (!btn) return;

        btn.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (buttonType == ButtonType.LoadScene)
            SceneController.Instance.LoadScene(targetScene);
        else
            SceneController.Instance.ExitGame();
    }
}
