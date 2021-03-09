using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellMenu : MonoBehaviour
{
    public static SellMenu instance;
    public Image itemSprite;
    public Text priceText;
    public Slider amountSlider;
    public Text amountSliderText;

    public SellButton[] sellButtons;

    public string selectedItem;
    public Item activeItem;

    private bool itemSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        CheckIfItem();

        amountSliderText.text = amountSlider.value.ToString();
        if (priceText.isActiveAndEnabled)
        {
            priceText.text = (activeItem.value * amountSlider.value).ToString();
        }
    }

    private void CheckIfItem()
    {
        if (activeItem != null)
        {
            itemSelected = true;
        }
        if (activeItem == null)
        {
            itemSelected = false;
        }

        if (!itemSelected)
        {
            itemSprite.gameObject.SetActive(false);
            amountSlider.gameObject.SetActive(false);
            priceText.gameObject.SetActive(false);
        }

        if (itemSelected)
        {
            itemSprite.gameObject.SetActive(true);
            amountSlider.gameObject.SetActive(true);
            priceText.gameObject.SetActive(true);
        }
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for (int i = 0; i < sellButtons.Length; i++)
        {
            sellButtons[i].buttonValue = i;

            if (GameManager.instance.itemName[i] != "")
            {
                sellButtons[i].buttonImage.gameObject.SetActive(true);
                sellButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemName[i]).itemSprite;
                sellButtons[i].amountText.text = GameManager.instance.itemQuantity[i].ToString();
            }
            else
            {
                sellButtons[i].buttonImage.gameObject.SetActive(false);
                sellButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        selectedItem = activeItem.itemName;
        itemSprite.sprite = activeItem.itemSprite;
        priceText.text = activeItem.value.ToString();
        amountSlider.maxValue = GameManager.instance.GetItemQuantity(selectedItem);
        amountSlider.value = 1;
    }

    public void Sell()
    {
        if (activeItem != null)
        {
            for (int i = 0; i < amountSlider.value; i++)
            {
                GameManager.instance.RemoveItem(selectedItem);
                GameManager.instance.CheckForPotions();
                PlayerStats.instance.money += activeItem.value;
            }

            activeItem = null;
            selectedItem = "";
        }

        CloseActiveItemUI();
    }

    public void CloseActiveItemUI()
    {
        itemSprite.gameObject.SetActive(false);
        amountSlider.gameObject.SetActive(false);
        priceText.gameObject.SetActive(false);
        activeItem = null;
        selectedItem = "";
    }
}
