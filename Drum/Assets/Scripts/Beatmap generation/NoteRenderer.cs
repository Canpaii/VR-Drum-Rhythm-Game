using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteRenderer : MonoBehaviour
{
    [SerializeField] private Note note; // Need the note component for variables
    private Renderer _renderer;
    
    private float _speed;
    private float _distance;
    void Start()
    {
        _renderer = GetComponent<Renderer>();

        _speed = note.speed;
        _distance = note.distance;
    }
    
    void Update() // 
    {
        float totalLifetime = (_distance / _speed);
        float timeSinceSpawn = totalLifetime - ((_distance - Vector3.Distance(transform.position, Vector3.zero)) / _speed);
         
        // Clamp timeSinceSpawn to ensure it stays between 0 and totalLifetime
        timeSinceSpawn = Mathf.Clamp(timeSinceSpawn, 0f, totalLifetime);
         
        // Normalize timeSinceSpawn to get the progress from 0 to 1
        float progress = timeSinceSpawn / totalLifetime;
         
        // Reverse the progress to flip the interpolation so it goes out instead of in
        float reversedProgress = 1 - progress;
         
        // Interpolate shader value from -0.7 to -0.119 based on reversed progress
        float shaderValue = Mathf.Lerp(-0.7f, 0f, reversedProgress);
        _renderer.material.SetFloat("_Slider", shaderValue);
    }
}
