using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SubsystemsImplementation;

public class Note : MonoBehaviour
{
    public string name;
    [Header("Bool")]
    public bool drumRollable;
    public bool hit;
    
    [Header("Core Variables")]
    public float speed; 
    public float distance;
    public float despawnTime; // amount of time the note stays visible
    
    public double timeStamp; // the time this note needs to be hit, used for other calculations 
    [Header("References")] 
    public Path path;
    public void Initialize(float noteSpeed, float distance, double hitTimeStamp, float despawnTime)
    {
        speed = noteSpeed;
        this.distance = distance;
        
        timeStamp = hitTimeStamp;
        
        this.despawnTime = despawnTime;
    }

    public void Start() // few seconds after instantiating destroy the note. this way the note doesnt vanish in players view 
    {
        
       Destroy(gameObject, (distance/speed) + despawnTime);
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
         transform.position += transform.forward * speed * Time.deltaTime;
    }
}
