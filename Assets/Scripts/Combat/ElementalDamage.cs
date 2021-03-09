using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalDamage : MonoBehaviour
{
    public bool fire, ice, lightning, poison, ability;
    [SerializeField] private float damage;
    [SerializeField] private float dps;
    [SerializeField] private float lifeTime;
    [SerializeField] private int max;

    public void ElementDamage(GameObject enemy)
    {
        if (fire)
        {
            int chance = Random.Range(1, max);
            if (chance == 1)
            {
                DamageOverTime(enemy, true);
            }
        }
        if (poison)
        {
            int chance = Random.Range(1, max);
            if (chance == 1)
            {
                DamageOverTime(enemy, false);
            }
        }
        if (ice)
        {
            int chance = Random.Range(1, max);
            if (chance == 1)
            {
                StartCoroutine(Stun(enemy));
            }
        }
    }

    private void DamageOverTime(GameObject enemy, bool fire)
    {
        if (!ability)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {
                enemy.GetComponent<Enemy>().ActivateDamageOverTimer(damage, lifeTime, dps, fire);
            }
            if (enemy.GetComponent<PlayerStats>() != null)
            {
                enemy.GetComponent<PlayerStats>().ActivateDamageOverTimer(damage, lifeTime, dps, fire);
            }
        }
        else
        {
            enemy.GetComponent<Enemy>().ActivateDamageOverTimer(PlayerStats.instance.abilityDamage, lifeTime, dps, fire);
        }
    }

    IEnumerator Stun(GameObject enemy)
    {
        if (enemy.GetComponent<Enemy>() != null)
        {
            enemy.GetComponent<Enemy>().Stun(lifeTime);
        }
        if (enemy.GetComponent<PlayerStats>() != null)
        {
            enemy.GetComponent<PlayerStats>().Stun(lifeTime);
        }

        yield return new WaitForSeconds(lifeTime);

        if (enemy != null && enemy.GetComponent<Enemy>() != null)
        {
            enemy.GetComponent<Enemy>().UnStun();
        }
        if (enemy != null && enemy.GetComponent<PlayerStats>() != null)
        {
            PlayerController.instance.stun = false;
        }
    }
}
