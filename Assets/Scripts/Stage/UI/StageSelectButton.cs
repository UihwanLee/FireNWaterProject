using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    public int buttonId;
    public float alpha = 0.5f;

    private void OnEnable()
    {
        if (!TryGetComponent<Image>(out var img))
        {
            Logger.Log("이미지 못 찾음");
            return;
        }

        Color tempColor = img.color;

        if (buttonId <= GameManager.Instance.MaxClearStageId + 1) alpha = 1f;
        tempColor.a = tempColor.a * alpha;
        img.color = tempColor;
    }
}
