using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fail : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the Player tag
        if (other.CompareTag("Player"))
        {
            // Get the CharacterController component from the player
            CharacterController controller = other.GetComponent<CharacterController>();

            if (controller != null)
            {
                // Temporarily disable the CharacterController to avoid issues
                controller.enabled = false;

                // Teleport the player to the start point
                other.transform.position = startPoint.position;
                other.transform.rotation = startPoint.rotation;

                // Re-enable the CharacterController
                controller.enabled = true;

                Debug.Log("Player teleported to: " + startPoint.position);
            }
            else
            {
                // If no CharacterController is found, just teleport
                other.transform.position = startPoint.position;
                other.transform.rotation = startPoint.rotation;
                Debug.Log("Player teleported to: " + startPoint.position);
            }
        }
    }
}
