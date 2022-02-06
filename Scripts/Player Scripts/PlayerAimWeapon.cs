using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private Transform aimGunEndPointTransform;
    private GameObject gun;

    //public MeshRenderer shootingAnimation;
    //public AnimatedTexture animatedTexture;
    //public GameObject muzzleflash;

    //public GameObject bullet;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
        gun = GameObject.Find("Gun");
        //shootingAnimation.enabled = false;
    }

    private void Update()
    {
        handleAiming();
        handleShooting();
        handleReloading();
    }

    private void handleAiming()
    {
        Vector3 mousePosition = getMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        // Getting Euler Angle (we want a fixed x and y coordinate system, with the z-axis being the only one rotating for 2D).
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; //Source: https://youtu.be/fuGQFdhSPg4?t=275
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void handleShooting()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = getMouseWorldPosition();
            //muzzleflash.SetActive(true);

            Vector3 gunEndPointPosition = aimGunEndPointTransform.position;
            Vector3 shootPosition = mousePosition;

            Vector3 aimDirection = (mousePosition - transform.position).normalized;
            
            // Getting Euler Angle (we want a fixed x and y coordinate system, with the z-axis being the only one rotating for 2D).
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; //Source: https://youtu.be/fuGQFdhSPg4?t=275

            var ourGun = gun.GetComponent<Gun>();
            if (ourGun != null)
                ourGun.GetComponent<Gun>().shoot(gunEndPointPosition, angle, aimDirection);
        }
    }

    private void handleReloading()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            var ourGun = gun.GetComponent<Gun>();
            ourGun.reload();
        }
    }

    public static Vector3 getMouseWorldPosition()
    {
        Vector3 vec = getMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0;
        return vec;
    }

    public static Vector3 getMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}