using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class CaveExit : MonoBehaviour
{
    public Tilemap cave;
    public Tilemap caveCollider;
    public Collider2D worldBoundary;

    private bool appear = false;

    private void Update()
    {
        // says if appear, fade in and dont fade in once complete
        if (appear)
        {
            cave.color = new Color(cave.color.r, cave.color.g, cave.color.b, Mathf.MoveTowards(cave.color.a, 1, Time.deltaTime));
            caveCollider.color = new Color(cave.color.r, cave.color.g, cave.color.b, Mathf.MoveTowards(caveCollider.color.a, 1, Time.deltaTime));
            if (cave.color.a == 1 && caveCollider.color.a == 1)
            {
                appear = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            appear = true;
            FindObjectOfType<CaveEntrance>().CancelCaveEntrance();
            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = worldBoundary;
        }
    }

    public void CancelCaveExit()
    {
        appear = false;
    }

    public void Restart()
    {
        appear = true;
        FindObjectOfType<CaveEntrance>().CancelCaveEntrance();
    }
}
