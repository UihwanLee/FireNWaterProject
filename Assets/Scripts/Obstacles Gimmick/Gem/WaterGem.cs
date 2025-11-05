public class Watergem : BaseGem, InteractWithController
{
    public void Activate(BaseController wade)
    {
        AudioManager.instance.PlayClip(Define.SFX_JEM);

        //잼 먹고 
        currentColor.a = 0f;
        spriteRenderer.color = currentColor;
        //점수 체크 
        GameManager.Instance.AddWaterGem();
        //비활성화
        gameObject.SetActive(false);
    }

}
