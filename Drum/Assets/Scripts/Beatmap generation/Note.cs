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
        transform.position += transform.forward * _speed * Time.deltaTime;
    }
}
