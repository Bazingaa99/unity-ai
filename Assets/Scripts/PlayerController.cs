using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    public float speed;
    [SerializeField] private LayerMask groundMask;
    private Camera mainCamera;
    CharacterController cc;
    private ItemHandler itemHandler;
    private RangedWeapon rangedWeapon;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        itemHandler = GetComponent<ItemHandler>();
        rangedWeapon = itemHandler.primaryRangedWeapon.GetComponent<RangedWeapon>();
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        cc.Move(new Vector3(horizontalInput, 0, verticalInput) * speed * Time.deltaTime);

        Aim();

        if (Input.GetMouseButtonDown(0)) {
            rangedWeapon.Shoot();
        }

        if (Input.GetMouseButtonDown(1)) {
            rangedWeapon.Reload();
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return (success: true, position: hitInfo.point);
        }
        else
        {
            return (success: false, position: Vector3.zero);
        }
    }

    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;

            direction.y = 0;

            transform.forward = direction;
        }
    }
}
