using UnityEngine;

public abstract class BaseGem : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Color currentColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentColor = spriteRenderer.color;
    }

    public virtual void ResetObject()
    {
        currentColor.a = 1f;
        spriteRenderer.color = currentColor;

        Logger.Log("젬 활성화");
        gameObject.SetActive(true);
    }
}
