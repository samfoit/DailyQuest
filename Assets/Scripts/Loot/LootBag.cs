using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public string[] lootToDrop = new string[12];
    private float timer = 300f;
    public int lootNumber = 0;

    private void Start()
    {
        lootNumber = FindObjectsOfType<LootBag>().Length + 1;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !GameManager.instance.isLooting && !GameManager.instance.isInventory)
        {
            LootMenu.instance.SetLoot(lootToDrop);
            LootMenu.instance.EnableLoot();
            LootMenu.instance.SetLootBagNumber(lootNumber);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            LootMenu.instance.DisableLoot();
        }
    }

    public void SetLootDrops(string[] lootDrop)
    {
        int chance = 4;

        if (lootToDrop.Length >= 1)
        {
            for (int i = 0; i < lootToDrop.Length; i++)
            {
                if (i == 0)
                {
                    lootToDrop[i] = lootDrop[Random.Range(0, lootDrop.Length)];
                }
                else if (i > 0)
                {
                    if (Random.Range(0, 10) <= chance)
                    {
                        lootToDrop[i] = lootDrop[Random.Range(0, lootDrop.Length)];
                        chance--;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }

    public bool LootCheck()
    {
        for (int i = 0; i < lootToDrop.Length; i++)
        {
            if (lootToDrop[i] != "")
            {
                return true;
            }
        }
        return false; 
    }
}
