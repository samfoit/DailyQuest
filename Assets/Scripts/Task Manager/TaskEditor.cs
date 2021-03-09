using UnityEngine;
using UnityEngine.UI;

public class TaskEditor : MonoBehaviour
{
    public static TaskEditor instance;

    public GameObject EditField;
    public InputField taskName;
    public InputField startTime;
    public InputField endTime;
    public Button taskFinishedButton;
    public Text startTimezone;
    public Text endTimezone;

    public Task taskSelected;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void FinishedEdit()
    {
        taskSelected.SetTask(taskName.text, ConvertTimeText(startTime), ConvertTimeText(endTime),
            startTime.text, startTimezone.text.ToLower(), endTime.text, endTimezone.text.ToLower());
        CloseTaskEditor();
        TaskManager.instance.SaveTask(taskName.text, ConvertTimeText(startTime), ConvertTimeText(endTime),
            startTime.text, startTimezone.text, endTime.text, endTimezone.text, taskSelected.taskNumber);
    }

    public void TaskInfo(string name, string startT, string startTz, string endT, string endTz)
    {
        taskName.text = name;
        startTime.text = RevertTimeText(startT);
        startTimezone.text = startTz.ToUpper();
        endTime.text = RevertTimeText(endT);
        endTimezone.text = endTz.ToUpper();
    }

    public void OpenTaskEditor()
    {
        EditField.SetActive(true);
    }

    public void CloseTaskEditor()
    {
        EditField.SetActive(false);
    }

    private float ConvertTimeText(InputField timeText)
    {
        string rawTime = timeText.text;
        string floatTime = timeText.text;
        if (rawTime.Length == 3)
        {
            floatTime = rawTime.Insert(1, ".");
        }
        else if (rawTime.Length == 4)
        {
            floatTime = rawTime.Insert(2, ".");
        }

        float time = float.Parse(floatTime);

        if (timeText.GetComponentInChildren<TimeText>().pm && time < 12)
        {
            time += 12;
        }
        else if (timeText.GetComponentInChildren<TimeText>().am && time >= 12)
        {
            time += 12;
        }

        return time;
    }

    private string RevertTimeText(string time)
    {
        return time.Replace(":", "");
    }

    public void DeleteTask()
    {
        TaskManager.instance.activeTasks.Remove(taskSelected);
        Destroy(taskSelected.gameObject);
        CloseTaskEditor();

        TaskManager.instance.RemoveTask(taskSelected.taskNumber);
    }
}
