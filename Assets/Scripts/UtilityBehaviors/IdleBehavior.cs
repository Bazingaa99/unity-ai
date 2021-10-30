using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : UtilityBehavior
{
    private Vector3 idlePosition;
    public override void UpdateBehavior(BehaviorController behaviorController)
    {
        SensorController sensorController = behaviorController.GetComponent<SensorController>();
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        weight = 1 - (sensorController.playerVisible + navigationController.path.weight);
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        gameObject.SetActive(true);
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        StartCoroutine(Idle(navigationController));
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
    }

    private IEnumerator Idle(NavigationController navigationController)
    {
        while (isActive) {
            Debug.Log("Idle!");
            yield return null;
        }
    }
}
