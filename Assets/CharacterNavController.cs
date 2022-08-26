using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed = 120f;
    public float stopDistance = 0.2f;
    public Vector3 destination;
    public bool reachedDestination = false;

    // Update is called once per frame
    void Update()
    {
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destination.y = 0;

            float destinationDistance = destinationDirection.magnitude;

            movementSpeed = Random.Range(0.9f, 10f);

            if (destinationDistance >= stopDistance)
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

                Debug.Log(transform.position);

            }
            else
            {
                reachedDestination = true;

            }

        }
        else
        {
            reachedDestination = true;
        }
    }

    public void setDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }
}
