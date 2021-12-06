using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBehavior : UtilityBehavior
{
    public override float UpdateBehavior(BehaviorController behaviorController)
    {
        ItemHandler itemHandler = behaviorController.GetComponent<ItemHandler>();
        RangedWeapon rangedWeapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
        CombatController combatController = behaviorController.GetComponent<CombatController>();
        if (combatController.reloadTime <= 0) {
            weight = (float) (Mathf.Abs(rangedWeapon.ammo - rangedWeapon.maxAmmo) / rangedWeapon.maxAmmo);
        } else {
            weight = 0;
        }
        

        return weight;
    }

    public override void Trigger(BehaviorController behaviorController)
    {
        isActive = true;
        CombatController combatController = behaviorController.GetComponent<CombatController>();
        combatController.Reload();
    }

    public override void Reset(BehaviorController behaviorController)
    {
        isActive = false;
    }
}
