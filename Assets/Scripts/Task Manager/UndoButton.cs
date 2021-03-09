using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
    public static UndoButton instance;
    public bool transparent;
    public float expToSubtract;

    private float timer;
    [SerializeField] private float lifeTime = 3f;

    private Image buttonImage;
    private Button button;

    private string taskName;
    private float start;
    private float end;
    private string timeStart;
    private string startTimzeZone;
    private string timeEnd;
    private string endTimeZone;
    private int taskNumber;

    // Start is called before the first frame update
    void Start()
    {
        timer = lifeTime;
        instance = this;
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0f);
    }

    private void OnDisable()
    {
        transparent = true;
        buttonImage.color = new Color(1f, 1f, 1f, 0f);
    }

    private void Update()
    {
        if (transparent)
        {
            button.interactable = false;
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, Mathf.MoveTowards(buttonImage.color.a, 0, 0.015f));
        }
        else
        {
            button.interactable = true;
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, Mathf.MoveTowards(buttonImage.color.a, 0.5f, 0.015f));
        }

        if (buttonImage.color.a == 0.5)
        {
            timer = Mathf.MoveTowards(timer, 0, 0.015f);
            if (timer <= 0)
            {
                transparent = true;
                timer = lifeTime;
            }
        }
    }

    public void OnClick()
    {
        Undo();
    }

    private void Undo()
    {
        // Subtract exp
        PlayerStats.instance.SubtractExp(expToSubtract);
        GameManager.instance.expFromQuests -= expToSubtract / (PlayerStats.instance.level / 2);
        // Recreate Task
        TaskManager.instance.CreateTask(taskName, start, end, timeStart, startTimzeZone, timeEnd, endTimeZone, taskNumber);
        TaskManager.instance.SaveTask(taskName, start, end, timeStart, startTimzeZone, timeEnd, endTimeZone, taskNumber);
        // Fade out
        transparent = true;
    }

    public void LastTask(Task task)
    {
        taskName = task.taskName;
        start = task.startTime;
        end = task.endTime;
        timeStart = task.taskTimeText.startTimeText;
        startTimzeZone = task.taskTimeText.startTimezone;
        timeEnd = task.taskTimeText.endTimeText;
        endTimeZone = task.taskTimeText.endTimezone;
        taskNumber = task.taskNumber;
    }
}
