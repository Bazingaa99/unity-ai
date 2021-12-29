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
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }
    }

    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position;

            // You might want to delete this line.
            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            transform.forward = direction;
        }
    }
}
