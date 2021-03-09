using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyHealthSlider : MonoBehaviour
{
    [SerializeField] private Slider moneyHealth;
    [SerializeField] private Text currentGold;

    private void Update()
    {
        SetMoney(PlayerStats.instance.money);
    }

    public void ActivateMoneyHealth()
    {
        moneyHealth.maxValue = PlayerStats.instance.money;
        moneyHealth.value = PlayerStats.instance.money;
        currentGold.text = PlayerStats.instance.money.ToString();
    }

    public void SetMoney(int playerMoney)
    {
        if (playerMoney > moneyHealth.maxValue)
        {
            moneyHealth.maxValue = playerMoney;
        }
        moneyHealth.value = playerMoney;
        currentGold.text = playerMoney.ToString();
    }
}
