using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedScreen : MonoBehaviour
{
    private bool fadeIn = false;
    private bool red = false;

    public Image redScreen;

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.instance.currentHealth <= PlayerStats.instance.maxHealth * .25)
        {
            fadeIn = true;
        }
        else
        {
            fadeIn = false;
        }

        if (fadeIn)
        {
            if (!red)
            {
                redScreen.color = new Color(1f, 0f, 0f, Mathf.MoveTowards(redScreen.color.a, 0.2f, Time.deltaTime));
                if (redScreen.color.a == 0.2)
                {
                    red = true;
                }
            }

            if (red)
            {
                redScreen.color = new Color(1f, 0f, 0f, Mathf.MoveTowards(redScreen.color.a, 0f, Time.deltaTime));

                if (redScreen.color.a == 0)
                {
                    red = false;
                }
            }
        }
        else
        {
            redScreen.color = new Color(1f, 0f, 0f, 0f);
        }
    }
}
