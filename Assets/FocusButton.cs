using UnityEngine;

public class FocusButton : MonoBehaviour
{
    Task task;

    private void Awake()
    {
        task = GetComponentInParent<Task>();
    }

    public void Press()
    {
        TaskManager.instance.ActivateRewardTimer(task.endTime);
    }

    private float GetSeconds()
    {
        string currentTime = System.DateTime.Now.ToString("HH:mm");
        currentTime = currentTime.Replace(":", ".");
        float current = BaseTen(float.Parse(currentTime));
        float end = BaseTen(task.endTime);
        float timeDifference = end - current;
        if (timeDifference < 0) { timeDifference *= 0; }

        return timeDifference;
    }

    private float BaseTen(float time)
    {
        float minutes = time % 1;
        double min = System.Math.Round(minutes, 2);
        minutes = (float)min;
        time -= minutes;
        time *= 60;
        minutes *= 100;
        if (time + minutes < 0)
        {
            return (time + minutes) * -1;
        }
        return time + minutes;
    }
}
