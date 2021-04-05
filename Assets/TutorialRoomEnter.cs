using UnityEngine;
using UnityEngine.Tilemaps;

public class TutorialRoomEnter : MonoBehaviour
{
    public Tilemap wall;
    public Tilemap room;

    private bool vanish = false;

    private void Update()
    {
        // Says if vanish, fade away when it hits zero dont fade
        if (vanish)
        {
            wall.color = new Color(wall.color.r, wall.color.g, wall.color.b, Mathf.MoveTowards(wall.color.a, 0, Time.deltaTime));
            room.color = new Color(room.color.r, room.color.g, room.color.b, Mathf.MoveTowards(room.color.a, 0, Time.deltaTime));
            if (wall.color.a == 0 && room.color.a == 0)
            {
                vanish = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            vanish = true;
            FindObjectOfType<TutorialRoomExit>().CancelExit();
        }
    }

    public void CancelEntrance()
    {
        vanish = false;
    }
}
