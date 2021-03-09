using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 10f;
    public Rigidbody2D rigidBody;
    public bool rotate = false;
    private Transform player;

    private float timer;
    public float lifeTime = 2f;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>().gameObject.transform;
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = (player.position - transform.position).normalized * speed;
        if (rotate)
        {
            transform.right = player.position - transform.position;
        }
        timer = lifeTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
            timer = lifeTime;
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
