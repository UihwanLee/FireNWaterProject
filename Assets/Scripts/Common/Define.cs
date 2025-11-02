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

    [Header("Character Stat")]
    public static readonly float MIN_SPEED = 1f;
    public static readonly float MAX_SPEED = 5f;
    public static readonly float SLOPE_SPEED = 3f;

    public static readonly float ACCELERATION = 30f;
    public static readonly float DECELERATION = 0.5f;

    public static readonly float JUMPFORCE = 50f;

    public const float BASE_FIRCITON = 0.4f;
    public const float SLOPE_FIRCITON = 0.0f;
}
