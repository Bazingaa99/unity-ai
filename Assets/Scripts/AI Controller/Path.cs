using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject[] checkpoints;
    public GameObject currentCheckpoint;
    public enum PathType {
        Stop,
        GoBack,
        BackAndForthPatrol,
        CirclePatrol
    }
    public PathType type;
    public float weight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
