using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScrollbar : MonoBehaviour
{
    [SerializeField] private Image handler;
    [SerializeField] private Scrollbar scroller;

    private bool fadeIn = false;

    // Update is called once per frame
    void Update()
    {
        if (scroller.value <= 0.8f)
        {
            fadeIn = true;
        }

        if (fadeIn)
        {
            handler.color = new Color(handler.color.r, handler.color.g, handler.color.b, Mathf.MoveTowards(handler.color.a, 1f, 0.02f));
        }

        if (scroller.value >= 0.8f)
        {
            fadeIn = false;
        }

        if (!fadeIn)
        {
            handler.color = new Color(handler.color.r, handler.color.g, handler.color.b, Mathf.MoveTowards(handler.color.a, 0f, 0.02f));
        }
    }
}
