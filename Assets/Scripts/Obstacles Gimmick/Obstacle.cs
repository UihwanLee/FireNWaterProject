using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Lava,
    Water,
    Poison
}


public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleType type = ObstacleType.Lava;
    private void OnTriggerEnter2D(Collider2D other)
    {
        
       BaseController controller = other.GetComponent<BaseController>();

        bool DeadCondition = false;
        
        if (controller != null)
        {
            Debug.Log("Find Controller");
            var layer = controller.gameObject.layer;
            Debug.Log(layer);
            int maskEmber = LayerMask.GetMask("Ember");
            Debug.Log(maskEmber);
            int maskWade = LayerMask.GetMask("Wade");
            
            if (type == ObstacleType.Poison || (type == ObstacleType.Lava && layer == maskWade)
                                           || (type == ObstacleType.Water && layer == maskEmber))
            {
                DeadCondition = true;
            }
        }

        if (DeadCondition)
        {
            Debug.Log("Dead");
        }
        
        
    }
}
