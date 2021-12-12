using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class PatrolBehavior : UtilityBehavior
{
    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.patrol = true;
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        NavigationController navigationController = behaviorController.GetComponent<NavigationController>();
        navigationController.patrol = false;
    }
}
