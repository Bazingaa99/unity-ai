using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : UtilityBehavior
{
    public override void UpdateBehavior(BehaviorController behaviorController)
    {
        SensorController sensorController = behaviorController.GetComponent<SensorController>();
        weight = sensorController.playerVisible;
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        Follow(behaviorController.GetComponent<NavigationController>(), behaviorController.GetComponent<SensorController>());
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        StopFollow(behaviorController.GetComponent<NavigationController>());
    }

    private void Follow(NavigationController navigationController, SensorController sensorController)
    {
        navigationController.Follow(sensorController.playerTransform);
    }

    private void StopFollow(NavigationController navigationController)
    {
        navigationController.StopFollow();
    }
}
