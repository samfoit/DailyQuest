using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawner : MonoBehaviour
{
    [SerializeField] GameObject trigger;
    [SerializeField] Transform location;

    public void Spawn()
    {
        GameObject obj = Instantiate(trigger);
        obj.transform.position = location.position;
    }
}
