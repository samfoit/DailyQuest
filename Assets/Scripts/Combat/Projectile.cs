using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 0.1f;
    private float timer;

    private void Awake()
    {
        timer = lifeTime;
    }


    private void Update()
    {
        timer -= Time.deltaTime;

        transform.position += transform.up * speed * Time.deltaTime;

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "NPC")
        {
            other.GetComponent<Enemy>().TakeDamage(PlayerStats.instance.strength + PlayerStats.instance.wpnPwr);
            if (other.GetComponent<Enemy>() != null)
            {
                if (gameObject.GetComponent<ElementalDamage>() != null)
                {
                    gameObject.GetComponent<ElementalDamage>().ElementDamage(other.gameObject);
                }
            }
            Destroy(gameObject);
        }
    }
}
