using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public bool tutorial = true;

    public GameObject attackJoystick;
    public GameObject abilityButton;
    public GameObject hpButton;
    public GameObject mpButton;
    public GameObject playerStatus;

    public bool questEvent = false;
    public bool uiEvent = false;
    public bool menuEvent = false;

    public Transform startPos;
    public Animator animator;

    private void Awake()
    {

        if (PlayerPrefs.HasKey("tutorial"))
        {
            tutorial = false;
        }
        instance = this;
        if (tutorial)
        {
            attackJoystick.SetActive(false);
            abilityButton.SetActive(false);
            hpButton.SetActive(false);
            mpButton.SetActive(false);
            playerStatus.SetActive(false);
            startPos.position = new Vector2(-274, 0);
        }
    }

    public void Activate(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void Update()
    {

    }

    public void MenuAnimation()
    {
        string[] text = { "Swipe down to access the menu" };
        DialogManager.instance.ShowBox();
        DialogManager.instance.ShowDialog(text, false);
        animator.SetTrigger("Swipe");
    }

    public void PlayerUIAnimation()
    {
        string[] text = { "The bar up above is you!", "It shows your health, mana, and experience respectively.",
            "You gain experience when you defeat enemies", "Don't let your healthbar drop to 0!"};
        DialogManager.instance.ShowBox();
        DialogManager.instance.ShowDialog(text, false);
    }
}
