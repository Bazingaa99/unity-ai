using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    public GameObject projectile;

    public int ammo;

    public float fireRate;
    private float downTime;
    public float launchVelocity;
    private bool isShot = false;

    // Start is called before the first frame update
    void Start()
    {
        downTime = fireRate;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        GameObject shotProjectile = Instantiate(projectile, transform.position + transform.up, transform.rotation);

        Destroy(shotProjectile, 5f);
        shotProjectile.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * launchVelocity);
    }
}
