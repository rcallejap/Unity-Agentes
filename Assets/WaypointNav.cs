using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNav : MonoBehaviour
{
    CharacterNavController controller;
    public Waypoints currentWaypoint;

    int direction;

    private void Awake()
    {
        controller = GetComponent<CharacterNavController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.reachedDestination && currentWaypoint != null)
        {
            if (direction == 0)
            {
                if (currentWaypoint.nextWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                }
                else
                {
                    currentWaypoint = null;
                }
            }
            else if (direction == 1)
            {
                if (currentWaypoint.prevousWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.prevousWaypoint;
                }
                else
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                    direction = 0;
                }
            }
            if (currentWaypoint != null)
            {
                controller.setDestination(currentWaypoint.GetPosition());
            }
        }
    }
}
