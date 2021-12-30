using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FollowPlayerBehavior : UtilityBehavior
{    
    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.followPlayer = true;
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        SensorController sensorController = behaviorController.GetComponent<SensorController>();
        navigationController.searchTime = navigationController.maxSearchTime;
        if (sensorController.objectTransform != null) {
            navigationController.lastKnownPosition = sensorController.objectTransform.position;
        }
        navigationController.followPlayer = false;
        navigationController.navMeshAgent.isStopped = false;
    }
}
