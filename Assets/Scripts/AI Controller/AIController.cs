using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIController : MonoBehaviour
{
    private InteractionController interactionController;
    private NavigationController navigationController;
    private SensorController sensorController;
    private BehaviorController behaviorController;
    void Start()
    {
        interactionController = GetComponent<InteractionController>();
        navigationController = GetComponent<NavigationController>();
        sensorController = GetComponent<SensorController>();
        behaviorController = GetComponent<BehaviorController>();
    }

    void Update()
    {
        
    }
}
