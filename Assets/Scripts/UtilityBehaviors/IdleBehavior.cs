using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : UtilityBehavior
{
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
}
