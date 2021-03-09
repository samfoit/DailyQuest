using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionConsumeButton : MonoBehaviour
{
    public GameObject healthButton, manaButton;
    public Item healthPotion, manaPotion;

    public Joystick attackJoystick;

    private void Update()
    {
        if (GameManager.instance.hasHealthPotion && PlayerStats.instance.currentHealth != PlayerStats.instance.maxHealth)
        {
            healthButton.GetComponent<Button>().interactable = true;
        }
        if (!GameManager.instance.hasHealthPotion || PlayerStats.instance.currentHealth == PlayerStats.instance.maxHealth)
        {
            healthButton.GetComponent<Button>().interactable = false;
        }
        if (GameManager.instance.hasManaPotion && PlayerStats.instance.currentMana != PlayerStats.instance.maxMana)
        {
            manaButton.GetComponent<Button>().interactable = true;
        }
        if (!GameManager.instance.hasManaPotion || PlayerStats.instance.currentMana == PlayerStats.instance.maxMana)
        {
            manaButton.GetComponent<Button>().interactable = false;
        }
    }

    public void HealthPotionButton()
    {
        healthPotion.Use();
        PopupTextManager.instance.ShowHpText(Mathf.Round(PlayerStats.instance.maxHealth * .2f), PlayerStats.instance.transform);
        GameManager.instance.CheckForPotions();
    }

    public void ManaPotionButton()
    {
        manaPotion.Use();
        GameManager.instance.CheckForPotions();
    }
}
