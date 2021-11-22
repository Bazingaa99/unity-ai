using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public GameObject primaryRangedWeapon;
    public GameObject secondaryRangedWeapon;
    public GameObject meleeWeapon;
    public GameObject item;

    void Start()
    {
        equipItem();
    }

    public void equipItem()
    {
        Vector3 itemPosition = transform.position + transform.right * transform.localScale.x + transform.up * transform.localScale.y;
        primaryRangedWeapon = Instantiate(primaryRangedWeapon, itemPosition, Quaternion.identity);
        primaryRangedWeapon.transform.Rotate(new Vector3(90, -180, 0), Space.World);
        primaryRangedWeapon.transform.parent = transform;
    }
}
