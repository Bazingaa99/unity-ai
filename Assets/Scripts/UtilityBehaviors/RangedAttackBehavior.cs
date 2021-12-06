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
        ItemHandler itemHandler = behaviorController.GetComponent<ItemHandler>();
        RangedWeapon rangedWeapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();

        if (combatController.available && combatController.hasAmmo && sensorController.objectVisible) {
            weight = Vector3.Distance(sensorController.gameObject.transform.position, sensorController.objectTransform.position) < 30f ? 1.0f : 0f;
            // weight -= (float) (Mathf.Abs(rangedWeapon.ammo - rangedWeapon.maxAmmo) / rangedWeapon.maxAmmo);
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
