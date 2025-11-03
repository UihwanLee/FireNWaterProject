using UnityEngine;

public abstract class BaseGem : MonoBehaviour
{
    public virtual void ResetObject()
    {
        gameObject.SetActive(true);
    }
}
