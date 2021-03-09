using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectile;
    public Transform player;
    public Joystick attackJoystick;
    public GameObject baseProjectile;

    private static float timer = 0f;

    private void Start()
    {
        if (global::PlayerStats.instance.equippedWpn != "")
        {
            if (GameManager.instance.GetItemDetails(global::PlayerStats.instance.equippedWpn).projectile != null)
            {
                projectile = GameManager.instance.GetItemDetails(global::PlayerStats.instance.equippedWpn).projectile;
            }
        }
    }

    private void Update()
    {
        if (!PlayerController.instance.stun)
        {
            if (attackJoystick.Horizontal != 0 || attackJoystick.Vertical != 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    Shoot();
                    timer = 0.5f;
                }
            }
        }
    }

    public void Shoot()
    {
        Instantiate(projectile, player.position, RotateProjectile());
    }

    public Quaternion RotateProjectile()
    {
       if (attackJoystick.Vertical < 0)
       {
            return  Quaternion.Euler(new Vector3(180, 0, attackJoystick.Horizontal * -90));
       }
       else
       {
            return Quaternion.Euler(new Vector3(0, 0, attackJoystick.Horizontal * -90));
       }
    }

    public void SetProjectile(GameObject proj)
    {
        projectile = proj;
    }

    public void SetBaseProjectile()
    {
        projectile = baseProjectile;
    }
}
