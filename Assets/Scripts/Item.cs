using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;
    public bool isAbility;
    public bool isCurrency;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header("Potion Details")]
    public float percentChange;
    public bool affectHp, affectMP, affectStr;

    [Header("Weapon/Armor/Ability Details")]
    public int weaponStrength;

    public int armorStrength;

    public int abilityStrength;

    [Header("Projectile")]
    public GameObject projectile;

    public void Use()
    {
        PlayerStats player = PlayerStats.instance;

        if (isItem)
        {
            if (affectHp)
            {
                float increase = player.maxHealth * (percentChange / 100);
                player.currentHealth += increase;
                if (player.currentHealth > player.maxHealth)
                {
                    player.currentHealth = player.maxHealth;
                }
                StatsUI.instance.ChangeStatSliders();
            }
            if (affectMP)
            {
                float increase = player.maxMana * (percentChange / 100);
                player.currentMana += increase;
                if (player.currentMana > player.maxMana)
                {
                    player.currentMana = player.maxMana;
                }
                StatsUI.instance.ChangeStatSliders();
            }
        }

        if (isWeapon)
        {
            if (player.equippedWpn != "")
            {
                GameManager.instance.AddItem(player.equippedWpn);
            }

            player.equippedWpn = itemName;
            player.wpnPwr = weaponStrength;

            GameMenu.instance.SetHeaderImages();
            if (projectile != null)
            {
                FindObjectOfType<Shooter>().SetProjectile(projectile);
            }
            else
            {
                FindObjectOfType<Shooter>().SetBaseProjectile();
            }
        }

        if (isArmor)
        {
            if (player.equippedArmr != "")
            {
                GameManager.instance.AddItem(player.equippedArmr);
            }

            player.equippedArmr = itemName;
            player.armPwr = armorStrength;

            GameMenu.instance.SetHeaderImages();
        }

        if (isAbility)
        {
            if (player.equippedAb != "")
            {
                GameManager.instance.AddItem(player.equippedAb);
            }

            player.equippedAb = itemName;
            player.abPwr = abilityStrength;

            GameMenu.instance.SetHeaderImages();
        }

        GameManager.instance.RemoveItem(itemName);
        GameManager.instance.CheckForPotions();
        GameManager.instance.SaveInventory();
    }
    
}
