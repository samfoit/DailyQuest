using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyMenu : MonoBehaviour
{
    public Image itemImage;
    public Text itemName;
    public Text itemDescription;
    public Text gold;


   public void SetBuyMenu (Item item)
    {
        itemImage.sprite = item.itemSprite;
        itemName.text = item.itemName;
        itemDescription.text = item.description;
        gold.text = item.value.ToString();
    }
}
