using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkObject : MonoBehaviour
{
    public int integer;
    public bool boolean;
    public float floating;
    public int timesUpdated;

    void Update()
    {
        integer += Mathf.RoundToInt((Random.value * 100));
        boolean = !boolean;
        floating += Random.value * 100;
        timesUpdated++;
    }
}