using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchBehavior : UtilityBehavior, ISerializationCallbackReceiver
{
    public float searchTime;
    private float startSearchTime;
    void Start()
    {
        startSearchTime = searchTime;
    }
    public override void UpdateBehavior(BehaviorController behaviorController)
    {
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        SensorController sensorController = behaviorController.GetComponent<SensorController>();
        
        Debug.Log("Last position known: " + navigationController.lastKnownPosition.HasValue);

        if (navigationController.lastKnownPosition.HasValue && !isActive && !sensorController.objectVisible) {
            weight = 1;
            rank = 1;
        }

        if (isActive) {
            weight -= Time.deltaTime / 4;
        }
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.lookAround = true;
        navigationController.StartMovingToPosition(navigationController.lastKnownPosition);
        Debug.Log("Searching...");
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        weight = 0;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.lastKnownPosition = null;
        navigationController.lookAround = false;
        Debug.Log("Stopped searching.");
    }
}
