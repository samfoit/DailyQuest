using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedScreen : MonoBehaviour
{
    public bool fadeIn = false;
    public bool red = false;

    public Image redScreen;

    private float timer = 0f;
    private float lifeTime = 0.5f;

    private void Awake()
    {
        timer = lifeTime;
    }

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
                if (redScreen.color.a >= 0.19f)
                {
                    red = true;
                }
            }

            if (red)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    redScreen.color = new Color(1f, 0f, 0f, Mathf.MoveTowards(redScreen.color.a, 0f, Time.deltaTime));

                    if (redScreen.color.a <= 0)
                    {
                        timer = lifeTime;
                        red = false;
                    }
                }
            }
        }
        else
        {
            redScreen.color = new Color(1f, 0f, 0f, 0f);
        }
    }
}
