using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAbility : MonoBehaviour
{
    public GameObject aoeProjectile;
    private float timer;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float speed = 5f;

    private Vector2 startPoint;
    private const float radius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        timer = lifeTime;
        startPoint = transform.position;
        SpawnProjectiles(8);
        PlayerStats.instance.currentMana -= PlayerStats.instance.maxMana / 3;
        StatsUI.instance.ChangeStatSliders();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnProjectiles(int numOfProj)
    {
        float angleStep = 360f / numOfProj;
        float angle = 0f;

        for (int i = 0; i < numOfProj; i++)
        {
            //Direction Calculations
            float projDirXPos = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projDirYPos = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2(projDirXPos, projDirYPos);
            Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * speed;

            GameObject proj = Instantiate(aoeProjectile, startPoint, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = projectileMoveDirection;

            angle += angleStep;
        }
    }
}
