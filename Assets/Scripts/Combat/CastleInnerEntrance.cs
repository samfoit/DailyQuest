using UnityEngine;
using UnityEngine.Tilemaps;

public class CastleInnerEntrance : MonoBehaviour
{
    public Tilemap outerEntrance;
    public Tilemap oECollider;
    public Tilemap dark;

    private bool vanish = false;

    // Update is called once per frame
    void Update()
    {
        if (vanish)
        {
            outerEntrance.color = new Color(outerEntrance.color.r, outerEntrance.color.g, outerEntrance.color.b, Mathf.MoveTowards(outerEntrance.color.a, 0, Time.unscaledDeltaTime));
            oECollider.color = new Color(oECollider.color.r, oECollider.color.g, oECollider.color.b, Mathf.MoveTowards(oECollider.color.a, 0, Time.unscaledDeltaTime));
            dark.color = new Color(dark.color.r, dark.color.g, dark.color.b, Mathf.MoveTowards(dark.color.a, 1, Time.unscaledDeltaTime));
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
            FindObjectOfType<CastleInnerExit>().CancelExit();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.transform.position.y > transform.position.y)
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    public void CancelEntrance()
    {
        vanish = false;
    }
}