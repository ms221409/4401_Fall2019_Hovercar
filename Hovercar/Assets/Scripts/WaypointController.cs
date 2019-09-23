using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public Transform[] waypoints;
    public float targetDistance = 3;


    public Transform GetWaypointTransform (int index)
    {
        return waypoints[index];
    }


    //
    public int GetNextWaypointIndex (int oldIndex)
    {
        int newIndex = oldIndex + 1;
        if (newIndex > waypoints.Length - 1)
            newIndex = 0;

        return newIndex;
    }
}
