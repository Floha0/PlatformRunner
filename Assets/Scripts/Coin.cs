using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float moveDistance = 1.0f; // Total distance to move up and down
    [SerializeField] private float moveSpeed = 1.0f; // Speed of the movement
    [SerializeField] private float rotateSpeed = 60.0f; // Speed of rotation (degrees per second)

    private Vector3 initialPosition;
    private bool movingUp = true;

    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Calculate the target position based on current direction (up or down)
        Vector3 targetPosition = movingUp ? initialPosition + Vector3.up * moveDistance : initialPosition;

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

        // Change direction when reaching the target position
        if (transform.position == targetPosition)
        {
            movingUp = !movingUp;
        }
        
        
        // Rotate continuously
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().ChangeScore(1);
            Destroy(this.gameObject);
        }
    }
}
