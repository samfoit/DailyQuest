using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    public bool am, pm;

    private Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponent<Text>();
        if (timeText.text == "AM")
        {
            am = true;
            pm = false;
        }
        if (timeText.text == "PM")
        {
            pm = true;
            am = false;
        }
    }

    public void OnClick()
    {
        if (timeText.text == "AM")
        {
            am = false;
            pm = true;
            timeText.text = "PM";
        }
        else if (timeText.text == "PM")
        {
            am = true;
            pm = false;
            timeText.text = "AM";
        }
    }
}
