using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectMover : MonoBehaviour
{
    public ParticleManage particleManage; // Reference to the ParticleManage script
    public float targetTime;             // Time at which the GameObject moves
    public Vector3 startPosition;        // Starting position of the GameObject
    public Vector3 endPosition;          // Target position of the GameObject
    public float moveDuration = 1.0f;    // Duration of the movement in seconds

    private bool hasMoved = false;       // Flag to ensure the movement happens only once
    private float moveProgress = 0.0f;   // Progress of the movement

    void Start()
    {
        if (particleManage == null)
        {
            Debug.LogError("ParticleManage reference is not assigned!");
        }
        transform.position = startPosition; // Set the initial position
    }

    void Update()
    {
        if (particleManage != null && !hasMoved && particleManage.currentTime >= targetTime)
        {
            StartCoroutine(MoveObject());
            hasMoved = true; // Prevents multiple triggers
        }
    }

    private System.Collections.IEnumerator MoveObject()
    {
        Vector3 initialPosition = transform.position;
        while (moveProgress < 1.0f)
        {
            moveProgress += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(initialPosition, endPosition, moveProgress);
            yield return null; // Wait for the next frame
        }
        transform.position = endPosition; // Ensure it ends exactly at the target
    }
}
