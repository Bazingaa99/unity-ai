using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : UtilityBehavior
{
    private Vector3 idlePosition;
    public int checkpointCount = 1;
    public override float UpdateBehavior(BehaviorController behaviorController)
    {
        if (agentIsBusy(behaviorController)) {
            weight = 0;
            rank = 0;
        } else {
            weight = 0.5f;
            rank = 3;
        }

        return weight;
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.lookAround = true;
        navigationController.LookAround();
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.lookAround = false;
    }

    private bool agentIsBusy(BehaviorController behaviorController)
    {
        SensorController sensorController = behaviorController.GetComponent<SensorController>();
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();

        return (navigationController.path != null) || (navigationController.lastKnownPosition != null);
    }
}
