using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuedTask : MonoBehaviour
{
    public int taskNumber;

    //Display
    public string taskName;
    public string startTimezone;
    public string endTimezone;
    public string startTimeText;
    public string endTimeText;

    // Calculation
    public float startTime;
    public float endTime;
    public float expToGive;

    public void StoreTask (string task, float startT, float endT, string timeStart, string startTimez, string timeEnd, string endTimez, int number)
    {
        taskName = task;
        startTime = startT;
        endTime = endT;
        startTimeText = timeStart;
        startTimezone = startTimez;
        endTime = endT;
        endTimeText = timeEnd;
        endTimezone = endTimez;
        taskNumber = number;
    }
}
