using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : UtilityBehavior
{
    private Vector3 idlePosition;
    public int checkpointCount = 1;
    public override void UpdateBehavior(BehaviorController behaviorController)
    {
        if (agentIsBusy(behaviorController)) {
            weight = 0;
            rank = 0;
        } else {
            weight = 0.5f;
            rank = 3;
        }
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.StartLookingAround();
        Debug.Log("Doing nothing.");
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        Debug.Log("Stopped doing nothing.");
    }

    private bool agentIsBusy(BehaviorController behaviorController)
    {
        SensorController sensorController = behaviorController.GetComponent<SensorController>();
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();

        return (navigationController.path != null) || (navigationController.lastKnownPosition != null);
    }
}
