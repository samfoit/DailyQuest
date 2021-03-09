using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider hpSlider;
    public Image background;
    public Image fill;

    private float fadeSpeed = 1f;
    public bool fadeIn = false;
    private float timer = 30f;

    // Start is called before the first frame update
    void Start()
    {
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (background.color.a == 1f && fill.color.a == 1f)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0 && !fadeIn)
        {
            FadeOut();
        }

        if (fadeIn)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        if (background.color.a == 1f && fill.color.a == 1f)
        {
            fadeIn = false;
            timer = 30f;
        }
        else
        {
            background.color = new Color(background.color.r, background.color.g, background.color.b, Mathf.MoveTowards(background.color.a, 1f, fadeSpeed * Time.deltaTime));
            fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, Mathf.MoveTowards(fill.color.a, 1f, fadeSpeed * Time.deltaTime));
        }
    }

    private void FadeOut()
    {
        if (background.color.a != 0f || fill.color.a != 0f)
        {
            background.color = new Color(background.color.r, background.color.g, background.color.b, Mathf.MoveTowards(background.color.a, 0f, fadeSpeed * Time.deltaTime));
            fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, Mathf.MoveTowards(fill.color.a, 0f, fadeSpeed * Time.deltaTime));
        }
        else
        {
            timer = 30f;
        }
    }
}
