using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusAbility : MonoBehaviour
{
    [SerializeField] private bool buff;
    [SerializeField] private bool heal;
    [SerializeField] private bool cure;

    [SerializeField] private float lifeTime = 1f;
    private float timer;
    private PlayerStats player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerStats.instance;
        timer = lifeTime;
        player.currentMana -= player.maxMana / 3;
        StatsUI.instance.ChangeStatSliders();
        if (buff)
        {
            StartCoroutine(Buff());
        }
        if (heal)
        {
            Heal();
        }
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

    private IEnumerator Buff()
    {
        player.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 1f);
        FindObjectOfType<PlayerController>().speed = 3f;
        player.strength += 5;
        yield return new WaitForSeconds(PlayerStats.instance.level);
        FindObjectOfType<PlayerController>().speed = 2.5f;
        player.strength -= 5;
        player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    private void Heal()
    {
        if (player.currentHealth + (player.maxMana / 3) > player.maxHealth)
        {
            PopupTextManager.instance.ShowHpText(player.maxHealth - player.currentHealth, transform);
            player.currentHealth = player.maxHealth;
            StatsUI.instance.ChangeStatSliders();
        }
        else
        {
            player.currentHealth += player.maxMana / 3;
            PopupTextManager.instance.ShowHpText(player.maxMana / 3, transform);
            StatsUI.instance.ChangeStatSliders();
        }

        PlayerStats.instance.Cleanse();
    }
}
