using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public float defense = 0f;
    public float expToGive = 0f;

    public string[] drops;

    [SerializeField] Slider hpSlider;

    private bool doT = false;
    [SerializeField] private float doTDamage;
    private float tickTimer;
    private float totalTimer;
    private float totalTime;
    private float dTimer;
    private bool frozen = false;
    private bool burning = false;
    public bool poisoned = false;
    public SpriteRenderer sprite;
    private bool green;

    [SerializeField] private bool tutorial;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = currentHealth;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (doT)
        {
            totalTime -= Time.deltaTime;
            dTimer -= Time.deltaTime;

            if (dTimer <= 0)
            {
                DamageOverTime(doTDamage);
                dTimer = tickTimer;
            }

            if (totalTime <= 0)
            {
                doT = false;
                burning = false;
                poisoned = false;
                totalTime = totalTimer;
            }
        }

        if (poisoned)
        {
            if (!green)
            {
                Poison();
            }
            if (green)
            {
                UnPoison();
            }
        }
        if (!poisoned)
        {
            UnPoison();
        }
    }

    public void TakeDamage(float damage)
    {
        damage -= defense;

        GetComponentInChildren<Healthbar>().fadeIn = true;

        if (damage <= 0)
        {
            damage = 1;
        }

        damage = Random.Range((int) damage, (int) damage+3);

        currentHealth -= damage;
        hpSlider.value = currentHealth;
        PopupTextManager.instance.ShowDamageText(damage, transform);

        if(currentHealth <= 0)
        {
            if (tutorial)
            {
                GameManager.instance.SetDropItems(drops);
                GameManager.instance.TutorialLoot(transform);
            }
            else
            {
                GameManager.instance.SetDropItems(drops);
                GameManager.instance.DropLootChance(transform);
            }
            Destroy(gameObject);
            PlayerStats.instance.AddExp(expToGive);
            DialogManager.instance.DisableTalkingOnDeath();
            if (GetComponent<Boss>() != null)
            {
                GetComponent<Boss>().Victory();
            }
        }
    }

    public void Stun(float lifetime)
    {
        if (!frozen)
        {
            if (GetComponentInChildren<EnemyAi>() != null)
            {
                GetComponentInChildren<EnemyAi>().enabled = false;
            }
            if (GetComponent<NPCMovement>() != null)
            {
                GetComponent<NPCMovement>().enabled = false;
            }
            if (GetComponent<EnemyShooter>() != null)
            {
                GetComponent<EnemyShooter>().enabled = false;
            }
            StartCoroutine(GameManager.instance.Freeze(transform, lifetime));
            frozen = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void UnStun()
    {
        if (frozen)
        {
            GetComponentInChildren<EnemyAi>().enabled = true;
            GetComponent<NPCMovement>().enabled = true;
            GetComponent<EnemyShooter>().enabled = true;
            frozen = false;
        }
    }

    public void DamageOverTime(float damage)
    {
        // Does the exact same thing as TakeDamage() except it doesn't use defense
        GetComponentInChildren<Healthbar>().fadeIn = true;

        currentHealth -= damage;
        hpSlider.value = currentHealth;
        PopupTextManager.instance.ShowDamageText(damage, transform);

        if (currentHealth <= 0)
        {
            GameManager.instance.SetDropItems(drops);
            GameManager.instance.DropLootChance(transform);
            Destroy(gameObject);
            PlayerStats.instance.AddExp(expToGive);
            DialogManager.instance.DisableTalkingOnDeath();
        }
        // Does the exact same thing as TakeDamage() except it doesn't use defense
    }

    public void ActivateDamageOverTimer(float damage, float lifetime, float increment, bool fire)
    {
        if (fire)
        {
            if (!burning)
            {
                doT = true;
                doTDamage = damage;
                totalTimer = lifetime;
                tickTimer = increment;

                totalTime = totalTimer;
                dTimer = tickTimer;
                StartCoroutine(GameManager.instance.Burn(transform, totalTimer));
                burning = true;
            }
        }
        else
        {
            if (!poisoned)
            {
                doT = true;
                doTDamage = damage;
                totalTimer = lifetime;
                tickTimer = increment;
                totalTime = totalTimer;
                dTimer = tickTimer;
                poisoned = true;
            }
        }
    }

    private void Poison()
    {
        if (sprite.color.r == 0 && sprite.color.b == 0)
        {
            green = true;
        }
        else
        {
            sprite.color = new Color(Mathf.MoveTowards(sprite.color.r, 0f, 1f * Time.deltaTime), sprite.color.g, Mathf.MoveTowards(sprite.color.b, 0f, 1f * Time.deltaTime));
        }
    }

    private void UnPoison()
    {
        if (sprite.color.r == 1 && sprite.color.b == 1)
        {
            green = false;
        }
        else
        {
            sprite.color = new Color(Mathf.MoveTowards(sprite.color.r, 1f, 1f * Time.deltaTime), sprite.color.g, Mathf.MoveTowards(sprite.color.b, 1f, 1f * Time.deltaTime));
        }
    }

}
