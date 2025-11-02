using System.Collections;
using System.Collections.Generic;
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

    #region 캐릭터 스탯

    [Header("Character Stat")]
    public static readonly float MIN_SPEED = 1f;
    public static readonly float MAX_SPEED = 5f;
    public static readonly float SLOPE_SPEED = 3f;

    public static readonly float ACCELERATION = 30f;
    public static readonly float DECELERATION = 0.5f;

    public static readonly float JUMPFORCE = 50f;

    public const float BASE_FIRCITON = 0.4f;
    public const float SLOPE_FIRCITON = 0.0f;

    public const float BASE_GRAVITY_SCALE = 1.0f;
    public const float SLOPE_GRAVITY_SCALE = 0.5f;

    #endregion

    #region 장애물 : Pulley

    public static readonly int BASE_CHAIN_COUNT = 4;
    public static readonly Vector2 BASE_STOOL_SCALE = new Vector2(1.6f, 1f);
    public const float DISTANCE_ANCHOR = -0.65f;
    public const float CHAIN_PADDING_VALUE = -0.45f;

    #endregion
}
