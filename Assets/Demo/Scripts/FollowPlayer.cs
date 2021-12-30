using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    
    void LateUpdate()
    {
        if (player != null) {
            float newXPosition = player.position.x - offset.x;
            float newZPosition = player.position.z - offset.z;
        
            transform.position = new Vector3(newXPosition, transform.position.y, newZPosition);
        }
    }
}
