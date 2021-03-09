using UnityEngine;
using UnityEngine.UI;

public class TaskTimeText : MonoBehaviour
{
    public string startTimeText, startTimezone, endTimeText, endTimezone;

    public Text timeText;

    public void SetTimeText(string startTime, string sTimezone, string endTime, string eTimezone)
    {
        startTimeText = DisplayTimeText(startTime);
        startTimezone = sTimezone;
        endTimeText = DisplayTimeText(endTime);
        endTimezone = eTimezone;

        timeText.text = startTimeText + " "+ sTimezone + " - " + endTimeText + " " + eTimezone;
    }

    private string DisplayTimeText(string time)
    {
        switch (time.Length)
        {
            case 1:
                return time += ":00";
            case 2:
                return time += ":00";
            case 3:
                return time.Insert(1, ":");
            case 4:
                return time.Insert(2, ":");
            default:
                return time;
        }
    }

}
