using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectile;
    private float timer = 0f;
    public bool shoot = false;

    [Header("Radial")]
    [SerializeField] bool radial = false;
    [SerializeField] GameObject radialProjectile;
    [SerializeField] int numberOfProjectiles = 8;
    [SerializeField] private float radialInterval = 5f;
    private float radialTimer;

    private void Start()
    {
        radialTimer = radialInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Shoot();
                timer = 1.5f;
            }
        }

        if (radial)
        {
            if (shoot)
            {
                radialTimer -= Time.deltaTime;
                if (radialTimer <= 0f)
                {
                    RadialProjectile proj = radialProjectile.GetComponent<RadialProjectile>();
                    proj.SetStartPos(transform);
                    proj.SpawnProjectiles(numberOfProjectiles);
                    radialTimer = radialInterval;
                }
            }
        }
    }

    public void Shoot()
    {
        GameObject shot = Instantiate(projectile);
        shot.transform.position = transform.position;
    }

    public void SetProjectile(GameObject proj)
    {
        projectile = proj;
    }

    public void SetRadial(GameObject rad)
    {
        radialProjectile = rad;
    }
}
