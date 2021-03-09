using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackLoadScreen : MonoBehaviour
{
    private bool fadeOut = false;
    private Image screen;

    public GameObject quest;

    private void OnApplicationFocus(bool focus)
    {
        if (focus && OrientationManager.portrait && quest.activeInHierarchy == false)
        {
            screen = GetComponent<Image>();
            screen.color = new Color(0f, 0f, 0f, 1f);
            fadeOut = true;
        }

        if (focus && !OrientationManager.portrait && quest.activeInHierarchy == true)
        {
            screen = GetComponent<Image>();
            screen.color = new Color(0f, 0f, 0f, 1f);
            fadeOut = true;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause && OrientationManager.portrait && quest.activeInHierarchy == false)
        {
            screen = GetComponent<Image>();
            screen.color = new Color(0f, 0f, 0f, 1f);
            fadeOut = true;
        }

        if (!pause && !OrientationManager.portrait && quest.activeInHierarchy == true)
        {
            screen = GetComponent<Image>();
            screen.color = new Color(0f, 0f, 0f, 1f);
            fadeOut = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            screen.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(screen.color.a, 0f, 0.02f));

            if (screen.color.a == 0)
            {
                fadeOut = false;
            }
        }
    }
}
