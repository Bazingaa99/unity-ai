using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private ItemHandler itemHandler;
    float currentWeaponFireRate = 0;
    float downTime = 0;

    void Awake()
    {
        itemHandler = GetComponent<ItemHandler>();
    }
    
    public void StartShooting()
    {
        Debug.Log(itemHandler);
        GameObject go = itemHandler.primaryRangedWeapon;
        RangedWeapon weapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
        currentWeaponFireRate = weapon.fireRate;
        StartCoroutine(Shoot(weapon));
    }

    private IEnumerator Shoot(RangedWeapon weapon)
    {
        while (true) {
            weapon.Shoot();

            yield return new WaitForSeconds(currentWeaponFireRate);
        }
    }
}
