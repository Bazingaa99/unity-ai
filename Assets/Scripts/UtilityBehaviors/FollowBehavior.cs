using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : UtilityBehavior
{
    private float timeVisibleWeight = 0;
    void Start()
    {
        rank = 2;
    }

    public override void UpdateBehavior(BehaviorController behaviorController)
    {
        SensorController sensorController = behaviorController.GetComponent<SensorController>();
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        float distanceWeight = 0;
        if (sensorController.objectTransform != null) {
            distanceWeight = (sensorController.distance - Vector3.Distance(sensorController.gameObject.transform.position, sensorController.objectTransform.position)) / sensorController.distance;
            timeVisibleWeight += Time.deltaTime / 100;
        }

        if (!sensorController.objectVisible) { 
            weight = 0;
        } else if (sensorController.objectVisible && navigationController.lastKnownPosition != null) {
            weight = 1;
        } else {
            weight = distanceWeight + timeVisibleWeight; // Periferini regejima geriau naudot
        }
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        StartFollow(behaviorController.GetComponent<NavigationController>(), behaviorController.GetComponent<SensorController>());
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        StopFollow(behaviorController.GetComponent<NavigationController>(), behaviorController.GetComponent<SensorController>());
    }

    private void StartFollow(NavigationController navigationController, SensorController sensorController)
    {
        navigationController.followObject = true;
        navigationController.StartFollow(sensorController.objectTransform);
        Debug.Log("Following object");
    }

    private void StopFollow(NavigationController navigationController, SensorController sensorController)
    {
        timeVisibleWeight = 0;
        navigationController.followObject = false;
        sensorController.SetIsObjectVisible(false);
        navigationController.StopFollow();
        Debug.Log("Stopped following object");
    }
}
