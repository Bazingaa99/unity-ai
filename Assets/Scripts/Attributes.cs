using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    public float maxHealth;
    
    public float health;

    public RectTransform rect;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (health <= 0) {
            health = 0;
        } else if (health >= maxHealth) {
            health = maxHealth;
        }

        rect.localScale = new Vector3(health / maxHealth, rect.localScale.y, rect.localScale.z);
    }
}
