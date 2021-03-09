using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject itemWindow;
    public GameObject windowBackground;

    public Text itemText;
    public Text itemDescriptionText;
    public Image itemImage;
    public Text useButtonText;

    public ItemButton[] itemButtons;

    public static GameMenu instance;

    public string selectedItem;
    public Item activeItem;

    public GameObject itemDetails;
    public GameObject discardMenu;

    public Image noSword, noArmor, noAbility;
    public Image sword, armor, ability;

    private Vector2 startInputPos;
    private Vector2 currentInputPos;

    private float timer = 0.5f;
    public bool canStart = true;

    [Header("Player")]
    public Slider hpSlider, mpSlider, expSlider;
    public Text hpText, mpText, expText, lvlText, moneyText;

    private void Start()
    {
        canStart = true;
        timer = 0.5f;
        instance = this;
        ShowItems();
    }

    private void Update()
    {

        if (activeItem == null)
        {
            itemDetails.SetActive(false);
        }

        if (itemWindow.activeInHierarchy || GameManager.instance.isTalking || GameManager.instance.isLooting || GameManager.instance.isShopping)
        {
            canStart = false;
        }

        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > 250)
        {
            startInputPos = Input.mousePosition;
            timer -= Time.time;
            canStart = true;
        }

        if (canStart)
        {
            currentInputPos = Input.mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                if (startInputPos.y > currentInputPos.y && timer <= 0)
                {
                    ActivateMenu();
                    timer = 0.5f;
                }

                startInputPos = Vector2.zero;
                currentInputPos = Vector2.zero;
            }
        }
        else
        {
            startInputPos = Vector2.zero;
            currentInputPos = Vector2.zero;
        }

    }

    public void ActivateMenu()
    {
        GameManager.instance.DeactivateJoysticks();
        itemDetails.SetActive(false);
        GameManager.instance.SortItems();
        itemWindow.SetActive(true);
        windowBackground.SetActive(true);
        ShowItems();
        GameManager.instance.isInventory = true;
        SetPlayerUI();
        Time.timeScale = 0;
    }

    public void DeactivateMenu()
    {
        GameManager.instance.ActivateJoysticks();
        activeItem = null;
        itemWindow.SetActive(false);
        windowBackground.SetActive(false);
        GameManager.instance.isInventory = false;
        Time.timeScale = 1;
    }

    public void CloseMenu()
    {
        activeItem = null;
        itemWindow.SetActive(false);
        windowBackground.SetActive(false);
        GameManager.instance.isInventory = false;
        GameManager.instance.ActivateJoysticks();
    }

    public void ShowItems()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.instance.itemName[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemName[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.itemQuantity[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }

        SetHeaderImages();
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        itemDetails.SetActive(true);

        if (activeItem.isItem)
        {
            useButtonText.text = "Use";
        }
        if (activeItem.isWeapon || activeItem.isArmor)
        {
            useButtonText.text = "Equip";
        }

        itemText.text = activeItem.itemName;
        itemDescriptionText.text = activeItem.description;
        itemImage.sprite = activeItem.itemSprite;
    }

    public void DiscardButtonPress()
    {
        if (activeItem != null)
        {
            discardMenu.SetActive(true);
        }
    }

    public void Discard()
    {
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
            discardMenu.SetActive(false);
        }
        else
        {
            discardMenu.SetActive(false);
        }
    }

    public void CloseDiscardMenu()
    {
        discardMenu.SetActive(false);
    }

    public void SetHeaderImages()
    {
        if (PlayerStats.instance.equippedWpn != "")
        {
            noSword.gameObject.SetActive(false);
            sword.gameObject.SetActive(true);
            sword.sprite = GameManager.instance.GetItemDetails(PlayerStats.instance.equippedWpn).itemSprite;
        }

        else if (PlayerStats.instance.equippedWpn == "")
        {
            noSword.gameObject.SetActive(true);
            sword.gameObject.SetActive(false);
        }

        if (PlayerStats.instance.equippedArmr != "")
        {
            noArmor.gameObject.SetActive(false);
            armor.gameObject.SetActive(true);
            armor.sprite = GameManager.instance.GetItemDetails(PlayerStats.instance.equippedArmr).itemSprite;
        }

        else if (PlayerStats.instance.equippedArmr == "")
        {
            noArmor.gameObject.SetActive(true);
            armor.gameObject.SetActive(false);
        }

        if (PlayerStats.instance.equippedAb != "")
        {
            noAbility.gameObject.SetActive(false);
            ability.gameObject.SetActive(true);
            ability.sprite = GameManager.instance.GetItemDetails(PlayerStats.instance.equippedAb).itemSprite;
        }

        else if (PlayerStats.instance.equippedAb == "")
        {
            noAbility.gameObject.SetActive(true);
            ability.gameObject.SetActive(false);
        }
    }

    public void UseItem()
    {
        if (activeItem != null)
        {
            activeItem.Use();
            SetPlayerUI();
        }
    }

    public void SetPlayerUI()
    {
        PlayerStats player = PlayerStats.instance;
        hpSlider.value = player.currentHealth / player.maxHealth;
        mpSlider.value = player.currentMana / player.maxMana;
        if (PlayerStats.instance.level < 20)
        {
            expSlider.value = PlayerStats.instance.currentExp / PlayerStats.instance.expToNextLevel[PlayerStats.instance.level];
            expText.text = Mathf.RoundToInt(player.currentExp) + " / " + player.expToNextLevel[player.level - 1];
        }
        else
        {
            expSlider.value = 1;
            expText.text = player.expToNextLevel[19] + " / " + player.expToNextLevel[19];
        }
        hpText.text = Mathf.RoundToInt(player.currentHealth) + " / " + player.maxHealth;
        mpText.text = Mathf.RoundToInt(player.currentMana) + " / " + player.maxMana;
        expText.text = Mathf.RoundToInt(player.currentExp) + " / " + player.expToNextLevel[player.level - 1];
        lvlText.text = "LVL: " + player.level;
        moneyText.text = player.money.ToString();
    }

}
