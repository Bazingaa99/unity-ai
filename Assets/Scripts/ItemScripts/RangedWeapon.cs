using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    public GameObject projectile;

    public float ammo;
    public float maxAmmo;
    public float reloadTime;
    public float maxReloadTime;
    public float fireRate;
    private float downTime = 0;
    public float launchVelocity;

    void Awake()
    {
        downTime = fireRate;
        ammo = maxAmmo;
        reloadTime = maxReloadTime;
    }

    void Update()
    {
        if (downTime >= 0) {
            downTime -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (ammo > 0 && downTime <= 0) {
            GameObject shotProjectile = Instantiate(projectile, transform.position + transform.up, transform.rotation);
            shotProjectile.GetComponent<CollisionScript>().launcher = transform.parent.gameObject;

            Destroy(shotProjectile, 5f);
            shotProjectile.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * launchVelocity);

            downTime = fireRate;
            ammo--;
        }
    }

    public void Reload()
    {
        ammo = maxAmmo;
    }
}
