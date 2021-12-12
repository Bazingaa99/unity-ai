using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCoverBehavior : UtilityBehavior
{

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.takeCover = true;
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.takeCover = false;
    }
}
