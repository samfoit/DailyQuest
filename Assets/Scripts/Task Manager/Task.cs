using UnityEngine;
using UnityEngine.UI;


public class Task : MonoBehaviour
{
    public int taskNumber;

    //Display
    public string taskName;
    public Text taskText;
    public Button completeButton;
    public TaskTimeText taskTimeText;

    // Calculation
    public float startTime;
    public float endTime;
    public float expToGive;

    public bool timeToFocus;
    public GameObject focus;
    private Animator animator;

    private int day;

    private void Start()
    {
        animator = GetComponent<Animator>();
        completeButton.interactable = false;
        timeToFocus = false;
        if (startTime >= 12 && startTime < 13 & taskTimeText.startTimezone == "am")
        {
            startTime -= 12;
        }
        if (endTime >= 12 && endTime < 13 & taskTimeText.endTimezone == "am")
        {
            endTime -= 12;
        }
        if (startTime > endTime) { endTime += 24; }
        if (PlayerPrefs.HasKey("TASK" + taskNumber + "DATE"))
        {
            day = PlayerPrefs.GetInt("TASK" + taskNumber + "DATE");
        }
        else
        {
            day = System.DateTime.Now.Day;
            PlayerPrefs.SetInt("TASK" + taskNumber + "DATE", day);
        }
    }

    private void Update()
    {
        if (System.DateTime.Now.Day == day || startTime > endTime)
        {
            if (Past() || day != System.DateTime.Now.Day)
            {
                completeButton.interactable = true;
            }
            else if (!Past())
            {
                completeButton.interactable = false;
            }

            if (During())
            {
                timeToFocus = true;
            }
            else if (!During())
            {
                timeToFocus = false;
            }

            if (timeToFocus)
            {
                focus.SetActive(true);
            }
            else if (!timeToFocus)
            {
                focus.SetActive(false);
            }
        }
        else
        {
            completeButton.interactable = true;
        }
    }

    public void SetTask(string task, float startT, float endT, string timeStart, string startTimezone, string timeEnd, string endTimezone)
    {
        taskName = task;
        CreateTaskName();
        taskTimeText.SetTimeText(timeStart, startTimezone, timeEnd, endTimezone);
        startTime = startT;
        endTime = endT;
    }

    public Task GetTask()
    {
        return this;
    }

    public void EditTask()
    {
        TaskEditor.instance.OpenTaskEditor();
        TaskEditor.instance.TaskInfo(taskName, taskTimeText.startTimeText, taskTimeText.startTimezone, taskTimeText.endTimeText, taskTimeText.endTimezone);
        TaskEditor.instance.taskSelected = GetTask();
    }

    private void CreateTaskName()
    {
        // Check if taskName string is more than 28 characters
        if (taskName.Length > 29)
        {
            taskText.text = "";
            // add the first 28 characters then add three dots
            for (int i  = 0; i < 29; i++)
            {
                taskText.text += taskName[i];
            }
            taskText.text += "...";
        }
        else
        {
            taskText.text = "";
            taskText.text = taskName;
        }
    }

    public void CompleteButton()
    {
        animator.SetTrigger("Complete");
    }

    public void FinishTask()
    {
        UndoButton.instance.LastTask(this);
        float round = endTime - startTime;
        if (round < 0) { round *= -1; }
        expToGive = round * 100 * (PlayerStats.instance.level / 2f) * 2.5f;
        PlayerStats.instance.AddExp(expToGive);
        TaskManager.instance.activeTasks.Remove(this);
        RewardTextManager.instance.ShowExpText(expToGive);
        SaveExpGained();
        Destroy(gameObject);
        TaskManager.instance.RemoveTask(taskNumber);
        PlayerPrefs.SetFloat("QUEST_EXP", GameManager.instance.expFromQuests);
        UndoButton.instance.expToSubtract = expToGive;
        UndoButton.instance.transparent = false;
    }

    private void SaveExpGained()
    {
        if (PlayerStats.instance.level == 1)
        {
            GameManager.instance.expFromQuests += expToGive;
        }
        else
        {
            GameManager.instance.expFromQuests += expToGive / (PlayerStats.instance.level / 2f);
        }
    }

    /// <summary>
    /// Used to determine if focus timer can be activated
    /// </summary>
    /// <returns></returns>
    private bool During()
    {
        string currentTime = System.DateTime.Now.ToString("HH:mm");
        currentTime = currentTime.Replace(":", ".");
        float current = BaseTen(float.Parse(currentTime));
        float end = BaseTen(endTime);
        float start = BaseTen(startTime);


        if (start > end)
        {
            if (current >= start)
            {
                return start <= current;
            }
            else
            {
                return current <= end;
            }
        }
        else
        {
            return start <= current && current < end;
        }
    }

    /// <summary>
    /// Used to determine if Complete Button can be turned on
    /// </summary>
    /// <returns></returns>
    private bool Past()
    {
        string currentTime = System.DateTime.Now.ToString("HH:mm");
        currentTime = currentTime.Replace(":", ".");
        float current = BaseTen(float.Parse(currentTime));
        float end = BaseTen(endTime);
        float start = BaseTen(startTime);


        if (start > end)
        {
            if (current >= start)
            {
                return false;
            }
            else
            {
                return current >= end;
            }
        }
        else
        {
            return current >= end;
        }
    }

    private float BaseTen(float time)
    {
        float minutes = time % 1;
        double min = System.Math.Round(minutes, 2);
        minutes = (float)min;
        time -= minutes;
        time *= 60;
        minutes *= 100;
        return time + minutes;
    }
}
