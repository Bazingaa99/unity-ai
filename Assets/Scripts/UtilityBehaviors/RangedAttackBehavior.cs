using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangedAttackBehavior : UtilityBehavior
{
    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        CombatController combatController = behaviorController.GetComponent<CombatController>();
        combatController.Shoot();
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
    }
}
