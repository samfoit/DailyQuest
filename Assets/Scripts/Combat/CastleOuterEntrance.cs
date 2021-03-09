using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class CastleOuterEntrance : MonoBehaviour
{
    public Tilemap outerEntrance;
    public Tilemap oECollider;
    public Collider2D castleBoundary;

    private bool vanish = false;

    // Update is called once per frame
    void Update()
    {
        if (vanish)
        {
            outerEntrance.color = new Color(outerEntrance.color.r, outerEntrance.color.g, outerEntrance.color.b, Mathf.MoveTowards(outerEntrance.color.a, 0, Time.deltaTime));
            oECollider.color = new Color(oECollider.color.r, oECollider.color.g, oECollider.color.b, Mathf.MoveTowards(oECollider.color.a, 0, Time.deltaTime));
            if (outerEntrance.color.a == 0 && oECollider.color.a == 0)
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
            FindObjectOfType<CastleOuterExit>().CancelExit();
            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = castleBoundary;
        }
    }

    public void CancelEntrance()
    {
        vanish = false;
    }
}
