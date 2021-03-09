using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> enemiesSpawned;
    public GameObject[] enemies;
    public float interval;
    public int maxEnemies;
    public Collider2D walkZone;
    public bool bossSpawner = false;

    private float timer;
    private bool spawn;

    private float secondsInDay = 0;

    private void Awake()
    {
        if (!bossSpawner)
        {
            if (PlayerPrefs.HasKey("SPAWNER_TIME"))
            {
                secondsInDay = PlayerPrefs.GetFloat("SPAWNER_TIME");
                SpawnIdle();
            }
            else
            {
                secondsInDay = CurrentTime();
                PlayerPrefs.SetFloat("SPAWNER_TIME", secondsInDay);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Sets up spawner
        timer = interval;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            secondsInDay = CurrentTime();
            PlayerPrefs.SetFloat("SPAWNER_TIME", secondsInDay);
        }
        if (!pause && !bossSpawner)
        {
            SpawnIdle();
        }
    }

    private void SpawnIdle()
    {
        if (CompareTime() < 0)
        {
            if (enemiesSpawned.Count >= maxEnemies)
            {
                return;
            }

            GameObject spawned = Instantiate(enemies[Random.Range(0, enemies.Length)]);
            spawned.transform.position = transform.position;
            enemiesSpawned.Add(spawned);
            timer = interval;
            spawned.GetComponent<NPCMovement>().SetWalkZone(walkZone);
            secondsInDay += interval;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if spawn is true and the max enemies limit has been reached
        if (spawn && enemiesSpawned.Count < maxEnemies)
        {
            // Increments timer down
            timer -= Time.deltaTime;

            // Spawns a random enemy from enemies[] and adds it to enemiesSpawned[]
            if (timer <= 0)
            {
                GameObject spawned = Instantiate(enemies[Random.Range(0, enemies.Length)]);
                spawned.transform.position = transform.position;
                enemiesSpawned.Add(spawned);
                timer = interval;
                spawned.GetComponent<NPCMovement>().SetWalkZone(walkZone);
            }
        }

        // Checks if enemies are null meaning they died and removes them then sets spawn to true
        for (int i = 0; i < enemiesSpawned.Count; i++)
        {
            if (enemiesSpawned[i] == null)
            {
                enemiesSpawned.Remove(enemiesSpawned[i]);
                spawn = true;
            }
        }

        if (enemiesSpawned.Count < maxEnemies)
        {
            spawn = true;
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

    private float CurrentTime()
    {
        string currentTime = System.DateTime.Now.ToString("HH:mm");
        currentTime = currentTime.Replace(":", ".");
        float current = BaseTen(float.Parse(currentTime));
        return current;
    }

    private float CompareTime()
    {
        return secondsInDay - CurrentTime();
    }

    public void ActivateBoss()
    {
        if (bossSpawner)
        {
            for (int i = 0; i < enemiesSpawned.Count; i++)
            {
                enemiesSpawned[i].GetComponentInChildren<EnemyAi>().boss = true;
            }
            FindObjectOfType<CinemachineVirtualCamera>().Follow = enemiesSpawned[0].transform;
        }
    }
}
