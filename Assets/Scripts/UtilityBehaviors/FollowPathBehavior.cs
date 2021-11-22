using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class FollowPathBehavior : UtilityBehavior
{
    public override void UpdateBehavior(BehaviorController behaviorController)
    {
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        SensorController sensorController = behaviorController.GetComponent<SensorController>();
        weight = 0;
        rank = 0;
        if (navigationController.path != null && navigationController.lastKnownPosition == null) {
            weight = navigationController.path.weight;
            rank = 1;
        }
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.followPath = isActive;
        navigationController.StartPath(navigationController.path);
        Debug.Log("Following path.");
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.followPath = isActive;
        navigationController.StopFollow();
        Debug.Log("Stopped following path.");
    }
}
