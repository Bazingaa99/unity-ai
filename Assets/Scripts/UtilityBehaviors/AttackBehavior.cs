using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackBehavior : UtilityBehavior
{
    void Start()
    {
        rank = 3;
    }
    public override void UpdateBehavior(BehaviorController behaviorController)
    {
        CombatController combatController = behaviorController.GetComponent<CombatController>();
        SensorController sensorController = behaviorController.GetComponent<SensorController>();

        Debug.Log("Attack available: " + combatController.available);
        if (combatController.available && sensorController.objectVisible) {
            weight = Vector3.Distance(sensorController.gameObject.transform.position, sensorController.objectTransform.position) < 30f ? 1 : 0;
        } else {
            weight = 0;
        }
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        CombatController combatController = behaviorController.GetComponent<CombatController>();
        combatController.attack = isActive;
        combatController.StartShooting();
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
        CombatController combatController = behaviorController.GetComponent<CombatController>();
        combatController.attack = isActive;
    }
}
