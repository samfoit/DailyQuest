using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialProjectile : MonoBehaviour
{
    public GameObject radialProjectilePrefab;

    [SerializeField] private float damage;
    private float timer;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool rotate = false;

    private Vector2 startPoint;
    private const float radius = 1f;


    // Start is called before the first frame update
    void Start()
    {
        timer = lifeTime;
        startPoint = transform.position;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetStartPos(Transform pos)
    {
        startPoint = pos.position;
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

            GameObject proj = Instantiate(radialProjectilePrefab, startPoint, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = projectileMoveDirection;
            if (rotate)
            {
                float rotation = Mathf.Atan2(projectileMoveDirection.y, projectileMoveDirection.x) * Mathf.Rad2Deg;
                proj.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
            }

            angle += angleStep;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().TakeDamage(damage);
            if (gameObject.GetComponent<ElementalDamage>() != null)
            {
                gameObject.GetComponent<ElementalDamage>().ElementDamage(collision.gameObject);
            }
            Destroy(gameObject);
        }
        else
        {
            if (!collision.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}
