using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : UtilityBehavior
{
    public override void UpdateBehavior(BehaviorController behaviorController)
    {
        weight = 1;
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        CombatController combatController = behaviorController.GetComponent<CombatController>();
        combatController.StartShooting();
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
    }
}
