using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public static ShopMenu instance;
    public GameObject blackScreenHolder;
    public Image[] blackFill;
    public Image whiteFill;
    public GameObject shopGame;
    public int count = 0;
    private bool fadeInBlack = false;
    private bool fadeOutWhite = false;

    [Header("Shop Menus")]
    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;
    public GameObject bargainMenu;
    public GameObject eventMenu;

    public string[] itemsToSell;

    public BuyMenu bMenu;
    public SellMenu sMenu;

    [SerializeField] private MoneyHealthSlider moneyHP;


    private void Start()
    {
        instance = this;
        blackScreenHolder.SetActive(false);
        for (int i = 0; i < blackFill.Length; i++)
        {
            blackFill[i].fillAmount = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (fadeInBlack)
        {
            for (int i = 0; i < blackFill.Length; i++)
            {
                blackFill[i].fillAmount = Mathf.MoveTowards(blackFill[i].fillAmount, 1, Time.deltaTime);
                if (blackFill[i].fillAmount == 1)
                {
                    fadeInBlack = false;
                }
            }
        }

        if (fadeOutWhite)
        {
            whiteFill.color = new Color(1f, 1f, 1f, Mathf.MoveTowards(whiteFill.color.a, 0f, Time.deltaTime));
            if (whiteFill.color.a == 0)
            {
                fadeOutWhite = false;
                whiteFill.color = new Color(1f, 1f, 1f, 1f);
                whiteFill.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator BeginShop()
    {
        GameManager.instance.DeactivateJoysticks();
        moneyHP.ActivateMoneyHealth();
        blackScreenHolder.SetActive(true);
        fadeInBlack = true;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < blackFill.Length; i++)
        {
            blackFill[i].fillAmount = 0;
        }
        fadeOutWhite = true;
        whiteFill.gameObject.SetActive(true);
        shopGame.SetActive(true);
        shopMenu.SetActive(true);
        blackScreenHolder.SetActive(false);
    }

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
        shopMenu.SetActive(false);
    }

    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
        shopMenu.SetActive(true);
    }

    public void Shop()
    {
        StartCoroutine(BeginShop());
        GameManager.instance.isShopping = true;
    }

    public void OpenBuyMenu()
    {
        buyMenu.SetActive(true);
        shopMenu.SetActive(false);
        bMenu.SetBuyMenu(GameManager.instance.GetItemDetails(itemsToSell[count]));
    }

    public void OpenSellMenu()
    {
        sellMenu.SetActive(true);
        shopMenu.SetActive(false);
        sMenu.ShowItems();
    }

    public void NextItem()
    {
        count++;
        if (count == itemsToSell.Length)
        {
            count = 0;
        }
        bMenu.SetBuyMenu(GameManager.instance.GetItemDetails(itemsToSell[count]));
    }

    public void PreviousItem()
    {
        count--;
        if (count < 0)
        {
            count = itemsToSell.Length - 1;
        }
        bMenu.SetBuyMenu(GameManager.instance.GetItemDetails(itemsToSell[count]));
    }

    public void BuyItem()
    {
        if (GameManager.instance.GetItemDetails(itemsToSell[count]).value > PlayerStats.instance.money) { return; }
        GameManager.instance.AddItem(itemsToSell[count]);
        PlayerStats.instance.money -= GameManager.instance.GetItemDetails(itemsToSell[count]).value;
    }

    public void SetUpShop(string[] items)
    {
        if (items.Length == 0)
        {
            Debug.LogError("Forgot to setup Shopkeeper");
        }
        itemsToSell = items;
    }

    public void Flee()
    {
        StartCoroutine(CloseShop());
    }

    private IEnumerator CloseShop()
    {
        yield return new WaitForSeconds(2.5f);
        shopGame.SetActive(false);
        shopMenu.SetActive(false);
        eventMenu.SetActive(false);
        GameManager.instance.isShopping = false;
        GameManager.instance.ActivateJoysticks();
    }
}
