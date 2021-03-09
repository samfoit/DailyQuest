using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    public int buttonValue;
    public Image buttonImage;
    public Text amountText;

    public void Press()
    {
        if (GameManager.instance.itemName[buttonValue] != "")
        {
            SellMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemName[buttonValue]));
        }
    }
}
