using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public Text timerText;
    public Image timerFill;
    public GameObject beginButton;
    public GameObject stopButton;

    private float startingSeconds = 100f;
    private float currentSeconds;
    private bool countdown = false;
    public RewardManager reward;

    private float taskEndTime;

    private void Awake()
    {
        instance = this;
        timerFill.fillAmount = currentSeconds / startingSeconds;
        if (PlayerPrefs.HasKey("TIMER_END_TIME"))
        {
            taskEndTime = PlayerPrefs.GetFloat("TIMER_END_TIME");
            currentSeconds = ConvertToSeconds();
            BeginCoutdown();
        }

        if (PlayerPrefs.HasKey("TIMER_STARTING_TIME"))
        {
            startingSeconds = PlayerPrefs.GetFloat("TIMER_STARTING_TIME");
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause && !countdown)
        {
            startingSeconds = ConvertToSeconds();
            currentSeconds = ConvertToSeconds();
            if (currentSeconds <= 0)
            {
                StopCountdown();
            }
        }
        if (!pause)
        {
            currentSeconds = ConvertToSeconds();
            if (currentSeconds <= 0)
            {
                Reward();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DisplayTime(startingSeconds);
    }

    private void Update()
    {
        if (countdown)
        {
            currentSeconds -= Time.unscaledDeltaTime;
            DisplayTime(currentSeconds);
            timerFill.fillAmount = currentSeconds / startingSeconds;
            beginButton.SetActive(false);
            stopButton.SetActive(true);

            if (currentSeconds <= 0)
            {
                Reward();
                countdown = false;
            }
        }
    }

    public void BeginCoutdown()
    {
        countdown = true;
    }

    public void StopCountdown()
    {
        countdown = false;
        beginButton.SetActive(true);
        stopButton.SetActive(false);
        gameObject.SetActive(false);
        PlayerPrefs.SetString("TIMER_SET_ACTIVE", "false");
        PlayerPrefs.DeleteKey("TIMER_END_TIME");
    }


    public void SetTime(float endTime)
    {
        taskEndTime = endTime;
        startingSeconds = ConvertToSeconds();
        currentSeconds = startingSeconds;
        DisplayTime(startingSeconds);
        PlayerPrefs.SetFloat("TIMER_STARTING_TIME", startingSeconds);
        PlayerPrefs.SetFloat("TIMER_END_TIME", taskEndTime);
    }

    private void Reward()
    {
        reward.SetReward(Mathf.RoundToInt(startingSeconds / 60));
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private float ConvertToSeconds()
    {
        string currentTime = System.DateTime.Now.ToString("HH:mm");
        currentTime = currentTime.Replace(":", ".");
        float current = BaseTen(float.Parse(currentTime));
        float end = BaseTen(taskEndTime);
        float timeDifference = end - current;
        if (timeDifference < 0) { timeDifference *= 0; }

        return timeDifference * 60;
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