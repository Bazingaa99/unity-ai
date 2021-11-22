using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private ItemHandler itemHandler;
    float currentWeaponFireRate = 0;
    public bool available = true;
    public bool attack = false;


    void Awake()
    {
        itemHandler = GetComponent<ItemHandler>();
    }

    void Update()
    {

    }

    public void StartShooting()
    {
        GameObject go = itemHandler.primaryRangedWeapon;
        RangedWeapon weapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
        currentWeaponFireRate = weapon.fireRate;
        StartCoroutine(Shoot(weapon));
    }

    private IEnumerator Shoot(RangedWeapon weapon)
    {
        while (attack) {
            weapon.Shoot();

            available = false;
            yield return new WaitForSeconds(currentWeaponFireRate);
            available = true;
        }
    }
}
