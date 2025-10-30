using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    Fire,
    Water
}

public class Temp_Player : MonoBehaviour
{
    public PlayerType type = PlayerType.Fire;

    public void PlayerDead()
    {
        Debug.Log("PlayerDead");
    }
    
}
