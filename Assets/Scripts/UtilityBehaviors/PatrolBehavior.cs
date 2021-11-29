using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class PatrolBehavior : UtilityBehavior
{
    public override float UpdateBehavior(BehaviorController behaviorController)
    {
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        SensorController sensorController = behaviorController.GetComponent<SensorController>();

        if (sensorController.objectVisible || navigationController.lastKnownPosition != null) {
            weight = 0;
        } else if (navigationController.path != null) {
            weight = 1f;
        }

        return weight;
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.patrol = true;
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.patrol = false;
    }
}
