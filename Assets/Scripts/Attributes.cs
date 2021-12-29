using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    public float maxHealth;
    
    public float health;

    void Start()
    {
        health = maxHealth;
    }
}
