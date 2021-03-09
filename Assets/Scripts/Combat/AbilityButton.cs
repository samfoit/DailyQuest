using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    private Button abilityButton;
    public GameObject abilitySprite;

    public Joystick moveJoystick;

    private float rotation;

    // Start is called before the first frame update
    void Start()
    {
        abilityButton = GetComponent<Button>();
    }

    private void Update()
    {
        if (PlayerStats.instance.equippedAb == "")
        {
            abilitySprite.SetActive(false);
        }
        if (PlayerStats.instance.equippedAb != "")
        {
            abilitySprite.SetActive(true);
            abilitySprite.GetComponent<Image>().sprite = GameManager.instance.GetItemDetails(PlayerStats.instance.equippedAb).itemSprite;
        }
        if (PlayerStats.instance.currentMana - PlayerStats.instance.maxMana / 3 >= 0 && PlayerStats.instance.equippedAb != "")
        {
            abilityButton.interactable = true;
        }
        else
        {
            abilityButton.interactable = false;
        }

        if (moveJoystick.Horizontal != 0 || moveJoystick.Vertical != 0)
        {
            rotation = moveJoystick.Horizontal;
        }
    }

    public void OnClick()
    {
        Instantiate(GameManager.instance.GetItemDetails(PlayerStats.instance.equippedAb).projectile, PlayerStats.instance.gameObject.transform.position, RotateAbility());
    }

    private Quaternion RotateAbility()
    {
        if (moveJoystick.Vertical < 0)
        {
            return Quaternion.Euler(new Vector3(180, 0, rotation * -90));
        }
        else
        {
            return Quaternion.Euler(new Vector3(0, 0, rotation * -90));
        }
    }

}
