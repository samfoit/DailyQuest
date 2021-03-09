using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.3f;
    public LayerMask enemyLayers;

    public int[] attacks;
    public static int click = 0;

    public const float MAX_START_TIME = 0.5f;

    public List<Enemy> enemies = new List<Enemy>();

    private Animator animator;

    public bool enemyNear;
    public bool noHit;

    private Vector2 playerPosition;
    private Vector2 closestEnemyPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestEnemy();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Add(other.GetComponent<Enemy>());
        }
        if (other.tag == "NPC")
        {
            NPCMovement movement = other.GetComponent<NPCMovement>();
            if (movement == null) { return; }
            movement.dontWalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Remove(other.GetComponent<Enemy>());
        }
        if (other.tag == "NPC")
        {
            NPCMovement movement = other.GetComponent<NPCMovement>();
            if (movement == null) { return; }
            movement.dontWalk = false;
        }
    }

    private void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;

        if (enemies.Count > 0)
        {
            enemyNear = true;
            foreach (Enemy currentEnemy in enemies)
            {
                float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;
                if (distanceToEnemy < distanceToClosestEnemy)
                {
                    distanceToClosestEnemy = distanceToEnemy;
                    closestEnemy = currentEnemy;
                }
            }

            Debug.DrawLine(transform.position, closestEnemy.transform.position);
            closestEnemyPosition = closestEnemy.transform.position;
        }
        else
        {
            enemyNear = false;
        }
    }


    public void Attack()
    {
        //Plays attack animation
        CheckDirection();
        Combo();

        //Detect enemy in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(5);
        }
    }

    private void CheckDirection()
    {
        animator.SetFloat("posX", 0f);
        animator.SetFloat("posY", 0f);

        playerPosition = transform.position;
        if (enemies.Count == 0) { return; }

        float xDifference = playerPosition.x - closestEnemyPosition.x;
        float yDifference = playerPosition.y - closestEnemyPosition.y;

        if (playerPosition.x > closestEnemyPosition.x && Mathf.Abs(yDifference) <= 0.5)
        {
            // Attack Right
            animator.SetFloat("posX", -.1f);
            animator.SetFloat("lastMoveX", 1f);
            animator.SetFloat("lastMoveY", 0f);
        }
        else if (playerPosition.x > closestEnemyPosition.x && Mathf.Abs(yDifference) > 0.5)
        {
            if (playerPosition.y < closestEnemyPosition.y)
            {
                // Attack Down Right
                animator.SetFloat("posX", -.1f);
                animator.SetFloat("lastMoveX", -1f);
                animator.SetFloat("posY", .1f);
                animator.SetFloat("lastMoveY", 1f);
            }
            else
            {
                // Attack Up Right
                animator.SetFloat("posX", -.1f);
                animator.SetFloat("lastMoveX", -1f);
                animator.SetFloat("posY", -.1f);
                animator.SetFloat("lastMoveY", -1f);
            }
        }
        else if (playerPosition.y < closestEnemyPosition.y && Mathf.Abs(xDifference) <= 0.5)
        {
            // Attack Down
            animator.SetFloat("posY", .1f);
            animator.SetFloat("lastMoveY", -1f);
            animator.SetFloat("lastMoveX", 0f);
        }
        else if (playerPosition.x < closestEnemyPosition.x && Mathf.Abs(yDifference) <= 0.5)
        {
            // Attack Left
            animator.SetFloat("posX", .1f);
            animator.SetFloat("lastMoveX", -1f);
            animator.SetFloat("lastMoveY", 0f);
        }
        else if (playerPosition.x < closestEnemyPosition.x && Mathf.Abs(xDifference) > 0.5)
        {
            if (playerPosition.y < closestEnemyPosition.y)
            {
                // Attack Down Left
                animator.SetFloat("posX", .1f);
                animator.SetFloat("lastMoveX", 1f);
                animator.SetFloat("posY", .1f);
                animator.SetFloat("lastMoveY", 1f);
            }
            else
            {
                // Attack Up Left
                animator.SetFloat("posX", .1f);
                animator.SetFloat("lastMoveX", 1f);
                animator.SetFloat("posY", -.1f);
                animator.SetFloat("lastMoveY", -1f);
            }
        }
        else if (playerPosition.y > closestEnemyPosition.y && Mathf.Abs(xDifference) <= 0.5)
        {
            // Attack Up
            animator.SetFloat("posY", -.1f);
            animator.SetFloat("lastMoveY", 1f);
            animator.SetFloat("lastMoveX", 0f);
        }
    }

    public void Combo()
    {
        string attack = "attack1";

        if (click == 0)
        {
            animator.SetTrigger(attack);
            click++;
        }
        else if (click > 0 && click != attacks.Length)
        {
            int nextClick = click + 1;
            attack = "attack" + nextClick;
            animator.SetTrigger(attack);
            click++;
        }
        else
        {
            click = 1;
            attack = "attack1";
            animator.SetTrigger(attack);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) { return; }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
