public class FireGem : BaseGem, InteractWithController
{
    public void Activate(BaseController ember)
    {
        AudioManager.instance.PlayClip(Define.SFX_JEM);

        //잼 먹고
        currentColor.a = 0f;
        spriteRenderer.color = currentColor;
        
        //점수 체크
        GameManager.Instance.AddFireGem();
        
        //비활성화
        gameObject.SetActive(false);
    }
}
