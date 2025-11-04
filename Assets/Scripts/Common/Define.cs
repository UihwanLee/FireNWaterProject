using UnityEngine;

/*
 * GAME 내에서 사용하는 변수들 Define
 */

public static class Define
{
    public static class LayerMask
    {
        public const int GROUND = 1 << 6;
        public const int SLOPE = 1 << 9;
        public const int EMBER = 1 << 7;
        public const int WADE = 1 << 8;
    }

    [Header("Character Stat")]
    public static readonly float MIN_SPEED = 1f;
    public static readonly float MAX_SPEED = 5f;
    public static readonly float SLOPE_SPEED = 5f;

    public static readonly float ACCELERATION = 30f;
    public static readonly float DECELERATION = 0.5f;

    public static readonly float SLOPE_ACCELERATION = 60f;

    public static readonly float JUMPFORCE = 50f;

    public const float BASE_FIRCITON = 0.4f;
    public const float SLOPE_FIRCITON = 0.0f;

    public const float BASE_GRAVITY_SCALE = 1.0f;
    public const float SLOPE_GRAVITY_SCALE = 0.1f;

    [Header("AudioSetting")]
    public const int AVAILABLE_SOUNDSORUCE_COUNT = 10;

    // 배경음악
    public const string BGM_INPLAY = "BGM_INPLAY";
    public const string BGM_INTRO = "BGM_INTRO";
    public const string BGM_OUTRO = "BGM_OUTRO";

    // 효과음
    public const string SFX_DIE = "SFX_DIE";
    public const string SFX_FALL = "SFX_FALL";
    public const string SFX_JUMP = "SFX_JUMP";
    public const string SFX_LOSE = "SFX_LOSE";
    public const string SFX_SELECT = "SFX_SELECT";
    public const string SFX_WIN = "SFX_WIN";
    public const string SFX_JEM = "SFX_JEM";

    [Header("Stage Setting")]
    // 튜토리얼[0] + 개인 제작[1 ~ 5] 
    public const int STAGE_NUM = 6;
}
