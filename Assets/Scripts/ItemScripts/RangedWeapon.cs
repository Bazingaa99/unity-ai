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
    private float downTime;
    public float launchVelocity;
    private bool isShot = false;

    // Start is called before the first frame update
    void Awake()
    {
        downTime = fireRate;
        ammo = maxAmmo;
        reloadTime = maxReloadTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        if (ammo > 0) {
            GameObject shotProjectile = Instantiate(projectile, transform.position + transform.up, transform.rotation);

            Destroy(shotProjectile, 5f);
            shotProjectile.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * launchVelocity);

            ammo--;
        }
    }

    public void Reload()
    {
        ammo = maxAmmo;
    }
}
