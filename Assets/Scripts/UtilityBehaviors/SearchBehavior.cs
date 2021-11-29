using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchBehavior : UtilityBehavior, ISerializationCallbackReceiver
{    
    public override float UpdateBehavior(BehaviorController behaviorController)
    {
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        SensorController sensorController = behaviorController.GetComponent<SensorController>();

        if (isActive) {
            weight -= Time.deltaTime / 8;
        }

        if (navigationController.lastKnownPosition.HasValue && !isActive && !sensorController.objectVisible) {
            weight = 1;
            rank = 1;
        }

        return weight;
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.MoveToPosition((Vector3) navigationController.lastKnownPosition);
        navigationController.search = true;
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        weight = 0;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.lastKnownPosition = null;
        navigationController.search = false;
        navigationController.DestroyGameObjects();
    }
}
