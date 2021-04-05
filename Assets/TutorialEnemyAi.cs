using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyAi : MonoBehaviour
{
    [Header("AI ")]
    // Won't attack unless attacked
    public bool passive;
    // Attacks if player is in cirlce collider
    public bool neutral;
    // Chases player if attacked
    public bool aggressive;

    public bool boss = false;

    [Header("Enemy")]
    public GameObject enemyParent;
    private EnemyShooter enemyShooter;
    private TutorialEnemy stats;

    // Move towards logic
    private Vector2 target;
    [SerializeField] private float speed = 3f;
    private bool moveTowards = false;
    private float timer;
    private Vector2 offset;
    private float timeToRandomize = 3f;
    private float randomTimer;

    // Start is called before the first frame update
    void Start()
    {
        enemyShooter = GetComponentInParent<EnemyShooter>();
        stats = GetComponentInParent<TutorialEnemy>();
        offset = new Vector2(1, 1);
        randomTimer = timeToRandomize;
    }

    // Update is called once per frame
    void Update()
    {
        if (passive && stats.currentHealth != stats.maxHealth || moveTowards || aggressive && stats.currentHealth != stats.maxHealth)
        {
            float step = speed * Time.deltaTime;
            target = FindObjectOfType<PlayerStats>().gameObject.transform.position;
            enemyParent.transform.position = Vector2.MoveTowards(enemyParent.transform.position, target - offset, step);
            enemyShooter.shoot = true;
            RandomizeOffset();
        }

        if (boss)
        {
            float step = speed * Time.deltaTime;
            target = FindObjectOfType<PlayerStats>().gameObject.transform.position;
            enemyParent.transform.position = Vector2.MoveTowards(enemyParent.transform.position, target - offset, step);
            enemyShooter.shoot = true;
            RandomizeOffset();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (neutral && other.tag == "Player")
        {
            enemyShooter.shoot = true;
        }

        if (aggressive && other.tag == "Player")
        {
            timer = 5f;
            enemyShooter.shoot = true;
            moveTowards = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (neutral && other.tag == "Player")
        {
            enemyShooter.shoot = false;
        }

        if (aggressive && other.tag == "Player")
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                moveTowards = false;
            }
        }
    }

    private void RandomizeOffset()
    {
        randomTimer -= Time.deltaTime;

        if (randomTimer <= 0)
        {
            offset = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            randomTimer = timeToRandomize;
        }
    }
}
