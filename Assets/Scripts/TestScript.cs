using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform player;
    public LayerMask layerMask;
    public bool InPlayerSight()
    {
        return Physics.Linecast(transform.position, player.position, layerMask);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Player can see the cube : " + InPlayerSight());
    }
}
