using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangedAttackBehavior : UtilityBehavior
{
    public override float UpdateBehavior(BehaviorController behaviorController)
    {
        CombatController combatController = behaviorController.GetComponent<CombatController>();
        SensorController sensorController = behaviorController.GetComponent<SensorController>();

        if (combatController.available && sensorController.objectVisible) {
            weight = Vector3.Distance(sensorController.gameObject.transform.position, sensorController.objectTransform.position) < 30f ? 1 : 0;
        } else {
            weight = 0;
        }

        return weight;
    }

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
