using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    public float maxHealth;
    
    public float health;

    public float maxEnergy;
    public float energy;

    void Start()
    {
        health = maxHealth;
        energy = maxEnergy;
    }
}
