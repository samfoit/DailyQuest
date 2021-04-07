using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootMenu : MonoBehaviour
{
    public string[] loot;

    public GameObject lootMenu;
    public GameObject bagIcon;

    public Image[] lootImage;

    public static LootMenu instance;

    public List<GameObject> lootBags;
    public int lootbagNumber;

    public GameObject goldTextPopup;
    public Transform goldTextPosition;

    private bool firstOpen = true;
    private bool firstClose = true;

    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("firstOpen"))
        {
            firstOpen = false;
        }

        if (PlayerPrefs.HasKey("firstClose"))
        {
            firstClose = false;
        }
    }

    private void Update()
    {
        HandleLoot();
    }

    public void EnableLoot()
    {
        lootMenu.SetActive(true);
        bagIcon.SetActive(true);

        for (int i = 0; i < lootImage.Length; i++)
        {
            if (loot[i] != "")
            {
                lootImage[i].sprite = GameManager.instance.GetItemDetails(loot[i]).itemSprite;
                lootImage[i].gameObject.GetComponent<DragDrop>().itemName = GameManager.instance.GetItemDetails(loot[i]).itemName;
                lootImage[i].gameObject.GetComponent<DragDrop>().currency = GameManager.instance.GetItemDetails(loot[i]).isCurrency;
                lootImage[i].enabled = true;
            }
            else
            {
                lootImage[i].enabled = false;
            }
        }

        GameManager.instance.isLooting = true;
        if (firstOpen)
        {
            TutorialManager.instance.LootAnimation();
            PlayerPrefs.SetString("firstOpen", "false");
            firstOpen = false;
        }
    }

    public void DisableLoot()
    {
        lootMenu.SetActive(false);
        bagIcon.SetActive(false);
        GameManager.instance.isLooting = false;
        if (firstClose)
        {
            TutorialManager.instance.MenuAnimation();
            PlayerPrefs.SetString("firstClose", "false");
            firstClose = false;
        }
    }

    public void SetLoot(string[] lootToDrop)
    {
        for (int i = 0; i < lootImage.Length; i++)
        {
            loot[i] = lootToDrop[i];
        }
    }

    public void UpdateLoot()
    {
        for (int j = 0; j < lootBags.Count; j++)
        {
            if (lootBags[j].GetComponent<LootBag>() == null)
            {
                lootBags.Remove(lootBags[j]);
            }
            else
            {
                if (lootBags[j].GetComponent<LootBag>().lootNumber == lootbagNumber)
                {
                    for (int i = 0; i < lootImage.Length; i++)
                    {
                        if (!lootImage[i].enabled)
                        {
                            lootBags[j].GetComponent<LootBag>().lootToDrop[i] = "";
                            loot[i] = "";
                        }
                    }
                }
            }
        }
    }

    public void ReEnableLoot()
    {
        for (int i = 0; i < lootImage.Length; i++)
        {
            lootImage[i].enabled = true;
        }
    }

    public void HandleLoot()
    {
        UpdateLoot();

        for (int i = 0; i < lootBags.Count; i++)
        {
            if (!lootBags[i].GetComponent<LootBag>().LootCheck())
            {
                DisableLoot();
                Destroy(lootBags[i]);
                lootBags.Remove(lootBags[i]);
                ReEnableLoot();
            }
        }
    }

    public void SetLootBagNumber(int number)
    {
        lootbagNumber = number;
    }

    public void GoldTextPopup(int value)
    {
        GameObject gold = Instantiate(goldTextPopup, transform);
        gold.transform.position = goldTextPosition.position;
        gold.GetComponent<FloatingText>().SetGoldText(value);
    }

}
