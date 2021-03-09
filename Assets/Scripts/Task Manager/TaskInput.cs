using UnityEngine;
using UnityEngine.UI;

public class TaskInput : MonoBehaviour
{
    public GameObject inputField;
    public InputField taskName;
    public InputField startTime;
    public InputField endTime;
    public Button taskFinishedButton;
    public Text startTimezone;
    public Text endTimezone;
    public Dropdown setDate;

    public int tasksCreated = 0;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("TASKS_CREATED"))
        {
            tasksCreated = PlayerPrefs.GetInt("TASKS_CREATED");
        }
    }

    private void Update()
    {
        if (taskName.text == "" || startTime.text == "" || endTime.text == "")
        {
            taskFinishedButton.interactable = false;
        }
        else
        {
            taskFinishedButton.interactable = true;
        }
    }

    private void OnDisable()
    {
        setDate.value = 0;
    }
    public void CreatedTask()
    {
        if (setDate.value == 0)
        {
            TaskManager.instance.CreateTask(taskName.text, ConvertTimeText(startTime), ConvertTimeText(endTime), startTime.text,
            startTimezone.text.ToLower(), endTime.text, endTimezone.text.ToLower(), tasksCreated);
            TaskManager.instance.SaveTask(taskName.text, ConvertTimeText(startTime), ConvertTimeText(endTime), startTime.text,
                startTimezone.text.ToLower(), endTime.text, endTimezone.text.ToLower(), tasksCreated);
            inputField.SetActive(false);
            tasksCreated++;
            PlayerPrefs.SetInt("TASKS_CREATED", tasksCreated);
        }
        else
        {
            TaskManager.instance.QueueTask(taskName.text, ConvertTimeText(startTime), ConvertTimeText(endTime), startTime.text,
            startTimezone.text.ToLower(), endTime.text, endTimezone.text.ToLower(), tasksCreated);
            TaskManager.instance.SaveQueueTask(taskName.text, ConvertTimeText(startTime), ConvertTimeText(endTime), startTime.text,
            startTimezone.text.ToLower(), endTime.text, endTimezone.text.ToLower(), tasksCreated);
            inputField.SetActive(false);
            tasksCreated++;
            PlayerPrefs.SetInt("TASKS_CREATED", tasksCreated);
        }
    }

    public void OpenInputField()
    {
        inputField.SetActive(true);
        taskName.text = "";
        startTime.text = "";
        endTime.text = "";
        taskFinishedButton.interactable = false;
        taskName.ActivateInputField();
    }

    public void CloseInputField()
    {
        taskName.text = "";
        startTime.text = "";
        endTime.text = "";
        inputField.SetActive(false);
        taskFinishedButton.interactable = false;
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
}
