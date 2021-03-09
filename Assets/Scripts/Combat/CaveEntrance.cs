using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class CaveEntrance : MonoBehaviour
{
    public Tilemap cave;
    public Tilemap caveCollider;
    public Collider2D caveBoundary;

    private bool vanish = false;

    private void Update()
    {
        // Says if vanish, fade away when it hits zero dont fade
        if (vanish)
        {
            cave.color = new Color(cave.color.r, cave.color.g, cave.color.b, Mathf.MoveTowards(cave.color.a, 0, Time.deltaTime));
            caveCollider.color = new Color(cave.color.r, cave.color.g, cave.color.b, Mathf.MoveTowards(caveCollider.color.a, 0, Time.deltaTime));
            if (cave.color.a == 0 && caveCollider.color.a == 0)
            {
                vanish = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            vanish = true;
            FindObjectOfType<CaveExit>().CancelCaveExit();
            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = caveBoundary;
        }
    }

    public void CancelCaveEntrance()
    {
        vanish = false;
    }
}
