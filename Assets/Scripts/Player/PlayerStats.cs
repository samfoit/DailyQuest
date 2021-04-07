using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public string charName;

    public float maxHealth = 100f;
    public float currentHealth;
    public float maxMana = 100f;
    public float currentMana;
    public float strength;
    public float abilityDamage;
    public float defense;
    public int wpnPwr;
    public int armPwr;
    public int abPwr;
    public string equippedAb;
    public string equippedWpn;
    public string equippedArmr;

    public int level = 1;
    public float currentExp;
    public float[] expToNextLevel;
    public int maxLevel = 20;
    public int baseExp = 100;
    public int money = 0;

    public static PlayerStats instance;

    private float timer = 1f;
    private float secondsInDay = 0;

    // Elemental damage stuff
    private bool doT = false;
    [SerializeField] private float doTDamage;
    private float tickTimer;
    private float totalTimer;
    private float totalTime;
    private float dTimer;
    private bool burning = false;
    public bool poisoned = false;
    public SpriteRenderer sprite;
    private bool green;

    private void Awake()
    {
        if (FindObjectsOfType<PlayerStats>().Length >= 2)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        expToNextLevel = new float[maxLevel + 1];
        expToNextLevel[1] = baseExp;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.RoundToInt(expToNextLevel[i - 1] * 1.2f + baseExp);
        }

        expToNextLevel[0] = 120;
        expToNextLevel[1] = 160;

        if (PlayerPrefs.HasKey("PLAYER_LEVEL"))
        {
            level = PlayerPrefs.GetInt("PLAYER_LEVEL");
            LoadStats();
        }
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (PlayerPrefs.HasKey("PLAYER_SECONDS"))
        {
            secondsInDay = PlayerPrefs.GetFloat("PLAYER_SECONDS");
            RegenOverload();
        }
        else
        {
            secondsInDay = CurrentTime();
            PlayerPrefs.SetFloat("PLAYER_SECONDS", secondsInDay);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            secondsInDay = CurrentTime();
            PlayerPrefs.SetFloat("PLAYER_SECONDS", secondsInDay);
        }

        if (!pause)
        {
            RegenOverload();
        }
    }

    private void Update()
    {
        if (currentHealth < maxHealth)
        {
            RegenHealth();
        }
        if (currentMana < maxMana)
        {
            RegenMana();
        }

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

    public void AddExp(float expToAdd)
    {
        if (level < maxLevel)
        {
            currentExp += expToAdd;

            PopupTextManager.instance.ShowExpText(expToAdd, transform);

            while (currentExp >= expToNextLevel[level - 1])
            {
                currentExp -= expToNextLevel[level - 1];

                level++;

                maxHealth += 10 * level;
                currentHealth = maxHealth;
                maxMana += 5 * level;
                currentMana = maxMana;
                strength++;
                abilityDamage++;
                defense++;
                PopupTextManager.instance.ShowLevelUpText(transform);

                if (currentExp < 0)
                {
                    currentExp = 0;
                }
            }
            SaveStats();
            if (!TutorialManager.instance.tutorial)
            {
                StatsUI.instance.ChangeStatSliders();
            }
        }
    }

    public void SubtractExp(float expToSubtract)
    {
        if (level >= 1)
        {
            currentExp -= expToSubtract;
            while (currentExp < 0)
            {
                currentExp += expToNextLevel[level - 2];

                level--;
                maxHealth -= 10 * (level + 1);
                currentHealth = maxHealth;
                maxMana -= 5 * (level + 1);
                currentMana = maxMana;
                strength--;
                abilityDamage--;
                defense--;
            }
            SaveStats();
            StatsUI.instance.ChangeStatSliders();
        }
    }

    public void TakeDamage(float damage)
    {
        damage -= armPwr;
        if (damage < 1) { damage = 1; }
        currentHealth -= damage;
        StatsUI.instance.ChangeStatSliders();
        PopupTextManager.instance.ShowDamageText(damage, transform);

        if (currentHealth <= 0)
        {
            DeathScreen.instance.Death();
        }
    }

    public void Stun(float lifeTime)
    {
        StartCoroutine(GameManager.instance.Freeze(transform, lifeTime));
        PlayerController.instance.stun = true;
    }

    public void DamageOverTime(float damage)
    {
        currentHealth -= damage;
        PopupTextManager.instance.ShowDamageText(damage, transform);

        if (currentHealth <= 0)
        {
            DeathScreen.instance.Death();
        }
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

    public void Cleanse()
    {
        doT = false;
        poisoned = false;
        SpriteRenderer[] effects = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].gameObject.tag == "Effect")
            {
                Destroy(effects[i].gameObject);
            }
        }
    }

    private void RegenHealth()
    {
        currentHealth = Mathf.MoveTowards(currentHealth, maxHealth, Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StatsUI.instance.ChangeStatSliders();
            timer = 1f;
        }
    }

    private void RegenMana()
    {
        currentMana = Mathf.MoveTowards(currentMana, maxMana, Time.deltaTime * 2);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StatsUI.instance.ChangeStatSliders();
            timer = 1f;
        }
    }

    private void RegenOverload()
    {
        if (CompareTime() < 0)
        {
            float restore = CompareTime() * -1;
            if (currentHealth + restore > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += restore;
                StatsUI.instance.ChangeStatSliders();
            }
            if (currentMana + restore > maxMana)
            {
                currentMana = maxMana;
            }
            else
            {
                currentMana += restore;
                StatsUI.instance.ChangeStatSliders();
            }
        }
    }

    public void Reset()
    {
        maxHealth = 100f;
        currentHealth = maxHealth;
        maxMana = 100f;
        currentMana = maxMana;
        strength = 1f;
        abilityDamage = 1f;
        defense = 1f;
        wpnPwr = 0;
        armPwr = 0;
        abPwr = 0;
        /*
        equippedArmr = "";
        equippedWpn = "";
        equippedAb = "";
        GameManager.instance.ClearItems();
        */
        level = 1;
        currentExp = 0;
        money = 20;
        GetComponent<Shooter>().SetBaseProjectile();
        SaveStats();
        Cleanse();
        StatsUI.instance.ChangeStatSliders();
    }

    public void StartAgain()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        transform.position = new Vector3(-37, 0, 0);
        if (GameManager.instance.expFromQuests < 0) { GameManager.instance.expFromQuests = 0; }
        AddExp(GameManager.instance.expFromQuests * (level / 2f));
        SaveStats();
        GameManager.instance.CheckForPotions();
    }


    private float BaseTen(float time)
    {
        float minutes = time % 1;
        double min = System.Math.Round(minutes, 2);
        minutes = (float)min;
        time -= minutes;
        time *= 60;
        minutes *= 100;
        // Returns time in minutes
        return time + minutes;
    }

    private float CurrentTime()
    {
        string currentTime = System.DateTime.Now.ToString("HH:mm");
        currentTime = currentTime.Replace(":", ".");
        float current = BaseTen(float.Parse(currentTime));
        return current * 60;
    }

    private float CompareTime()
    {
        return secondsInDay - CurrentTime();
    }

    public void SaveStats()
    {
        PlayerPrefs.SetInt("PLAYER_LEVEL", level);
        PlayerPrefs.SetFloat("PLAYER_MAX_HEALTH", maxHealth);
        PlayerPrefs.SetFloat("PLAYER_CURRENT_HEALTH", currentHealth);
        PlayerPrefs.SetFloat("PLAYER_MAX_MANA", maxMana);
        PlayerPrefs.SetFloat("PLAYER_CURRENT_MANA", currentMana);
        PlayerPrefs.SetFloat("PLAYER_STRENGTH", strength);
        PlayerPrefs.SetFloat("PLAYER_ABILITY_DAMAGE", abilityDamage);
        PlayerPrefs.SetFloat("PLAYER_DEFENSE", defense);
        PlayerPrefs.SetInt("PLAYER_WPN_PWR", wpnPwr);
        PlayerPrefs.SetInt("PLAYER_ARM_PWR", armPwr);
        PlayerPrefs.SetInt("PLAYER_AB_PWR", abPwr);
        PlayerPrefs.SetString("PLAYER_WPN", equippedWpn);
        PlayerPrefs.SetString("PLAYER_ARM", equippedArmr);
        PlayerPrefs.SetString("PLAYER_AB", equippedAb);
        PlayerPrefs.SetInt("PLAYER_MONEY", money);
        PlayerPrefs.SetFloat("PLAYER_CURRENT_EXP", currentExp);
    }

    private void LoadStats()
    {
        level = PlayerPrefs.GetInt("PLAYER_LEVEL");
        maxHealth = PlayerPrefs.GetFloat("PLAYER_MAX_HEALTH");
        currentHealth = PlayerPrefs.GetFloat("PLAYER_CURRENT_HEALTH");
        maxMana = PlayerPrefs.GetFloat("PLAYER_MAX_MANA");
        currentMana = PlayerPrefs.GetFloat("PLAYER_CURRENT_MANA");
        strength = PlayerPrefs.GetFloat("PLAYER_STRENGTH");
        abilityDamage = PlayerPrefs.GetFloat("PLAYER_ABILITY_DAMAGE");
        defense = PlayerPrefs.GetFloat("PLAYER_DEFENSE");
        wpnPwr = PlayerPrefs.GetInt("PLAYER_WPN_PWR");
        armPwr = PlayerPrefs.GetInt("PLAYER_ARM_PWR");
        abPwr = PlayerPrefs.GetInt("PLAYER_AB_PWR");
        equippedWpn = PlayerPrefs.GetString("PLAYER_WPN");
        equippedArmr = PlayerPrefs.GetString("PLAYER_ARM");
        equippedAb = PlayerPrefs.GetString("PLAYER_AB");
        money = PlayerPrefs.GetInt("PLAYER_MONEY");
        currentExp = PlayerPrefs.GetFloat("PLAYER_CURRENT_EXP");
    }

}
