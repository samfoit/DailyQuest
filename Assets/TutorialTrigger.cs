using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public bool attackJoystick;
    public bool abilityButton;
    public bool hpButton;
    public bool mpButton;
    public bool playerStatus;
    public bool tutorial;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (attackJoystick)
            {
                TutorialManager.instance.Activate(TutorialManager.instance.attackJoystick);
            }
            if (abilityButton)
            {
                TutorialManager.instance.Activate(TutorialManager.instance.abilityButton);
            }
            if (hpButton)
            {
                TutorialManager.instance.Activate(TutorialManager.instance.hpButton);
                TutorialManager.instance.Activate(TutorialManager.instance.mpButton);
            }
            if (playerStatus)
            {
                TutorialManager.instance.Activate(TutorialManager.instance.playerStatus);
                TutorialManager.instance.PlayerUIAnimation();
                Destroy(gameObject);
            }
            if (tutorial)
            {
                TutorialManager.instance.tutorial = false;
                PlayerStats.instance.gameObject.transform.position = new Vector2(-37, 0);
                PlayerPrefs.SetString("tutorial", "false");
            }
        }
    }
}
