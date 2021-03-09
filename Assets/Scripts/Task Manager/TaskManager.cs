using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    // This script needs to hangle creating a new task and placing it in the tasks bar
    public static TaskManager instance;
    public GameObject task;
    public List<Task> activeTasks;
    public List<QueuedTask> queuedTasks;

    public GameObject rewardTimer;

    private QueuedTask queuedTask;

    private int today;
    private int tomorrow;


    private void Awake()
    {
        instance = this;
        LoadTasks();
        if (PlayerPrefs.HasKey("TIMER_SET_ACTIVE"))
        {
            rewardTimer.SetActive(bool.Parse(PlayerPrefs.GetString("TIMER_SET_ACTIVE")));
        }
        today = System.DateTime.Now.Day;
        if (PlayerPrefs.HasKey("TOMORROW"))
        {
            tomorrow = PlayerPrefs.GetInt("TOMORROW");
        }
        else
        {
            tomorrow = System.DateTime.Now.Day + 1;
            PlayerPrefs.SetInt("TOMORROW", tomorrow);
        }
    }

    private void OnEnable()
    {
        today = System.DateTime.Now.Day;
        if (today == tomorrow)
        {
            NewDay();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            today = System.DateTime.Now.Day;
            if (today == tomorrow)
            {
                NewDay();
            }
        }
    }

    public void ActivateRewardTimer(float seconds)
    {
        rewardTimer.SetActive(true);
        Timer.instance.SetTime(seconds);
        PlayerPrefs.SetString("TIMER_SET_ACTIVE", "true");
    }

    public void CreateTask(string taskName, float start, float end, string timeStart, string startTimezone, string timeEnd, string endTimezone, int taskNumber)
    {
        GameObject quest = Instantiate(task);
        quest.transform.SetParent(transform);
        quest.transform.localScale = new Vector3(1, 1, 1);
        quest.GetComponent<Task>().SetTask(taskName, start, end, timeStart, startTimezone, timeEnd, endTimezone);
        activeTasks.Add(quest.GetComponent<Task>());
        quest.GetComponent<Task>().taskNumber = taskNumber;
        SaveTask(taskName, start, end, timeStart, startTimezone, timeEnd, endTimezone, taskNumber);
    }

    public void QueueTask(string taskName, float start, float end, string timeStart, string startTimezone, string timeEnd, string endTimezone, int taskNumber)
    {
        queuedTask = gameObject.AddComponent<QueuedTask>();
        queuedTask.StoreTask(taskName, start, end, timeStart, startTimezone, timeEnd, endTimezone, taskNumber);
        queuedTasks.Add(queuedTask);
    }

    private void NewDay()
    {
        for (int i = 0; i < queuedTasks.Count; i++)
        {
            QueuedTask quest = queuedTasks[i];
            CreateTask(quest.taskName, quest.startTime, quest.endTime, quest.startTimeText,
                quest.startTimezone, quest.endTimeText, quest.endTimezone, quest.taskNumber);
            RemoveQTask(queuedTasks[i].taskNumber);
        }
        tomorrow++;
        PlayerPrefs.SetInt("TOMORROW", tomorrow);
        queuedTasks.Clear();
    }


    public void SaveTask(string taskName, float start, float end, string timeStart, string startTimezone, string timeEnd, string endTimezone, int taskNumber)
    {
        PlayerPrefs.SetString("TASK" + taskNumber + "_TASK_NAME", taskName);
        PlayerPrefs.SetFloat("TASK" + taskNumber + "_START", start);
        PlayerPrefs.SetFloat("TASK" + taskNumber + "_END", end);
        PlayerPrefs.SetString("TASK" + taskNumber + "_TIME_START", timeStart);
        PlayerPrefs.SetString("TASK" + taskNumber + "_START_TIMEZONE", startTimezone);
        PlayerPrefs.SetString("TASK" + taskNumber + "_TIME_END", timeEnd);
        PlayerPrefs.SetString("TASK" + taskNumber + "_END_TIMEZONE", endTimezone);
    }

    public void SaveQueueTask(string taskName, float start, float end, string timeStart, string startTimezone, string timeEnd, string endTimezone, int taskNumber)
    {
        PlayerPrefs.SetString("QTASK" + taskNumber + "_TASK_NAME", taskName);
        PlayerPrefs.SetFloat("QTASK" + taskNumber + "_START", start);
        PlayerPrefs.SetFloat("QTASK" + taskNumber + "_END", end);
        PlayerPrefs.SetString("QTASK" + taskNumber + "_TIME_START", timeStart);
        PlayerPrefs.SetString("QTASK" + taskNumber + "_START_TIMEZONE", startTimezone);
        PlayerPrefs.SetString("QTASK" + taskNumber + "_TIME_END", timeEnd);
        PlayerPrefs.SetString("QTASK" + taskNumber + "_END_TIMEZONE", endTimezone);
    }

    public void RemoveTask(int taskNumber)
    {
        PlayerPrefs.DeleteKey("TASK" + taskNumber + "_TASK_NAME");
        PlayerPrefs.DeleteKey("TASK" + taskNumber + "_START");
        PlayerPrefs.DeleteKey("TASK" + taskNumber + "_END");
        PlayerPrefs.DeleteKey("TASK" + taskNumber + "_TIME_START");
        PlayerPrefs.DeleteKey("TASK" + taskNumber + "_START_TIMEZONE");
        PlayerPrefs.DeleteKey("TASK" + taskNumber + "_TIME_END");
        PlayerPrefs.DeleteKey("TASK" + taskNumber + "_END_TIMEZONE");
    }

    public void RemoveQTask(int taskNumber)
    {
        PlayerPrefs.DeleteKey("QTASK" + taskNumber + "_TASK_NAME");
        PlayerPrefs.DeleteKey("QTASK" + taskNumber + "_START");
        PlayerPrefs.DeleteKey("QTASK" + taskNumber + "_END");
        PlayerPrefs.DeleteKey("QTASK" + taskNumber + "_TIME_START");
        PlayerPrefs.DeleteKey("QTASK" + taskNumber + "_START_TIMEZONE");
        PlayerPrefs.DeleteKey("QTASK" + taskNumber + "_TIME_END");
        PlayerPrefs.DeleteKey("QTASK" + taskNumber + "_END_TIMEZONE");
    }

    public void LoadTasks()
    {
        if (PlayerPrefs.HasKey("TASKS_CREATED"))
        {
            for (int i = 0; i < PlayerPrefs.GetInt("TASKS_CREATED"); i++)
            {
                if (PlayerPrefs.HasKey("TASK" + i + "_TASK_NAME"))
                {
                    CreateTask(PlayerPrefs.GetString("TASK" + i + "_TASK_NAME"),
                        PlayerPrefs.GetFloat("TASK" + i + "_START"),
                        PlayerPrefs.GetFloat("TASK" + i + "_END"),
                        PlayerPrefs.GetString("TASK" + i + "_TIME_START"),
                        PlayerPrefs.GetString("TASK" + i + "_START_TIMEZONE"),
                        PlayerPrefs.GetString("TASK" + i + "_TIME_END"),
                        PlayerPrefs.GetString("TASK" + i + "_END_TIMEZONE"),
                        i);
                }
                else
                {
                    continue;
                }
            }
        }
        LoadQueuedTasks();
    }

    public void LoadQueuedTasks()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("TASKS_CREATED"); i++)
        {
            if (PlayerPrefs.HasKey("QTASK" + i + "_TASK_NAME"))
            {
                QueueTask(PlayerPrefs.GetString("QTASK" + i + "_TASK_NAME"),
                        PlayerPrefs.GetFloat("QTASK" + i + "_START"),
                        PlayerPrefs.GetFloat("QTASK" + i + "_END"),
                        PlayerPrefs.GetString("QTASK" + i + "_TIME_START"),
                        PlayerPrefs.GetString("QTASK" + i + "_START_TIMEZONE"),
                        PlayerPrefs.GetString("QTASK" + i + "_TIME_END"),
                        PlayerPrefs.GetString("QTASK" + i + "_END_TIMEZONE"),
                        i);
            }
        }
    }
}
