using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHover : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxMovement;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        float newY = originalPosition.y + Mathf.Sin(Time.time * speed) * maxMovement;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetMaxMovement(float newMaxMovement)
    {
        maxMovement = newMaxMovement;
    }
}
