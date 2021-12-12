using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : UtilityBehavior
{
    // Step Function
    // -------------------------------------------------------------------------
    // Default Behavior - When there is nothing else to do
    private Vector3 idlePosition;
    public int checkpointCount = 1;

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
