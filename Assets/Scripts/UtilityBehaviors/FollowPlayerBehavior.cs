using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerBehavior : UtilityBehavior
{
    public override float UpdateBehavior(BehaviorController behaviorController)
    {
        SensorController sensorController = behaviorController.GetComponent<SensorController>();
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        CombatController combatController = behaviorController.GetComponent<CombatController>();
        float distanceWeight = 0;
        if (sensorController.objectTransform != null) {
            distanceWeight = (sensorController.distance - Vector3.Distance(sensorController.gameObject.transform.position, sensorController.objectTransform.position)) / sensorController.distance;
        }

        if (!sensorController.objectVisible || combatController.reloadTime > 0) { 
            weight = 0;
        } else if (sensorController.objectVisible && navigationController.lastKnownPosition != null) {
            weight = 1;
        } else {
            weight = 1; // Periferini regejima geriau naudot
        }

        return weight;
    }

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
        navigationController.lastKnownPosition = sensorController.objectTransform.position;
        navigationController.followPlayer = false;
        navigationController.navMeshAgent.isStopped = false;
    }
}
