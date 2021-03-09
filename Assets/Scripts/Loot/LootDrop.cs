using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootDrop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            string itemName = eventData.pointerDrag.GetComponent<DragDrop>().itemName;
            if (itemName != "")
            {
                GameManager.instance.AddItem(itemName);
            }
            if (eventData.pointerDrag.GetComponent<DragDrop>().currency)
            {
                LootMenu.instance.GoldTextPopup(GameManager.instance.GetItemDetails(eventData.pointerDrag.GetComponent<DragDrop>().itemName).value);             
            }

            eventData.pointerDrag.GetComponent<Image>().enabled = false;
        }
    }
}
