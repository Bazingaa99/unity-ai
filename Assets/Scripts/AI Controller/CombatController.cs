using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private ItemHandler itemHandler;
    float currentWeaponFireRate = 0;
    float timeBetweenShots;
    public bool available = true;
    public bool attack = false;


    void Awake()
    {
        itemHandler = GetComponent<ItemHandler>();
        timeBetweenShots = 0;
    }

    void Update()
    {
        if (timeBetweenShots >= 0) {
            available = false;
            timeBetweenShots -= Time.deltaTime;
        } else {
            available = true;
        }
    }

    public void Shoot()
    {
        GameObject go = itemHandler.primaryRangedWeapon;
        RangedWeapon weapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
        timeBetweenShots = weapon.fireRate;
        weapon.Shoot();
    }
}
