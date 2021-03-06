using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public ItemHandler itemHandler;
    private Attributes attributes;
    private SensorController sensorController;
    float timeBetweenShots;
    public float reloadTime;
    public bool available = true;
    public bool hasAmmo = true;


    void Awake()
    {
        itemHandler = GetComponent<ItemHandler>();
        sensorController = GetComponent<SensorController>();
        attributes = GetComponent<Attributes>();
        timeBetweenShots = 0;
        reloadTime = 0;
    }

    void Update()
    {
        if (timeBetweenShots >= 0) {
            available = false;
            timeBetweenShots -= Time.deltaTime;
        } else if (reloadTime >= 0) {
            available = false;
            reloadTime -= Time.deltaTime;
        } else {
            available = true;
        }

        Heal();
    }

    public void Shoot()
    {
        GameObject go = itemHandler.primaryRangedWeapon;
        RangedWeapon weapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
        timeBetweenShots = weapon.fireRate;

        weapon.Shoot();
        hasAmmo = weapon.ammo > 0;
    }

    public void Reload()
    {
        if (reloadTime <= 0) {
            GameObject go = itemHandler.primaryRangedWeapon;
            RangedWeapon weapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
            reloadTime = weapon.reloadTime;
            StartCoroutine(Reloading(weapon));
        }
    }

    private IEnumerator Reloading(RangedWeapon weapon)
    {
        weapon.ammo = 0;

        yield return new WaitForSeconds(reloadTime);

        weapon.Reload();
        hasAmmo = true;
    }

    private void Heal()
    {
        attributes.health += Time.deltaTime;
    }
}
