using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EventDialog : MonoBehaviour
{
    public bool boss = false;
    public string[] dialog;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (boss)
            {
                PlayerController.instance.stun = true;
                Time.timeScale = 0;
                DialogManager.instance.ShowBox();
                DialogManager.instance.ShowDialog(dialog, false);
                Destroy(gameObject);
                foreach (Spawner spawner in FindObjectsOfType<Spawner>())
                {
                    spawner.ActivateBoss();
                }
            }
            else
            {
                PlayerController.instance.stun = true;
                Time.timeScale = 0;
                DialogManager.instance.ShowBox();
                DialogManager.instance.ShowDialog(dialog, false);
                Destroy(gameObject);
            }
        }
    }
}
