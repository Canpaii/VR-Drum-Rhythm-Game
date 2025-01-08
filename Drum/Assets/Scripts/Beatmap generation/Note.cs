using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SubsystemsImplementation;

public class Note : MonoBehaviour
{
    public string name;
    [Header("Bool")]
    public bool drumRollable;
    public bool hit;
    
    [Header("Core Variables")]
    private float _missMargin; // the time the player has to hit this note. after this time note cant be hit.
    private float _speed;
    private float _distance;
    private float _despawnTime; // amount of time the note stays visible
    
    public double timeStamp; // the time this note needs to be hit, used for other calculations 
    
    private Renderer _renderer;

    [Header("References")] 
    public Path path;
    public void Initialize(float noteSpeed, float distance, float missMargin, double hitTimeStamp, float despawnTime)
    {
        _speed = noteSpeed;
        _distance = distance;
        
        _missMargin = missMargin;
        timeStamp = hitTimeStamp;
        
        _despawnTime = despawnTime;
    }

    public void Start() // few seconds after instantiating destroy the note. this way the note doesnt vanish in players view 
    {
        _renderer = GetComponent<Renderer>();
       Destroy(gameObject, (_distance/_speed) + _despawnTime);
    }

    public void OnDestroy()
    {
        path.notes.Remove(gameObject);

        if (!hit) // If it didnt get hit count it as an instant miss
        {
            ScoreManager.Instance.Miss();
        }
    }
    
    void Update() // moves the note toward player
    {
         // Move the note forward
         transform.position += transform.forward * _speed * Time.deltaTime;
 
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
