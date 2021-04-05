using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TutorialRoomExit : MonoBehaviour
{
    public Tilemap wall;
    public Tilemap room;

    private bool appear = false;

    private void Update()
    {
        // Says if vanish, fade away when it hits zero dont fade
        if (appear)
        {
            wall.color = new Color(wall.color.r, wall.color.g, wall.color.b, Mathf.MoveTowards(wall.color.a, 1, Time.deltaTime));
            room.color = new Color(room.color.r, room.color.g, room.color.b, Mathf.MoveTowards(room.color.a, 1, Time.deltaTime));
            if (wall.color.a == 0 && room.color.a == 0)
            {
                appear = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            appear = true;
            FindObjectOfType<TutorialRoomEnter>().CancelEntrance();
        }
    }

    public void CancelExit()
    {
        appear = false;
    }
}
