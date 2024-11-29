using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SubsystemsImplementation;

public class Note : MonoBehaviour
{
    [Header("Bool")]
    public bool hit;
    public bool drumRollable;
    
    [Header("Core Variables")]
    private float _missMargin; // the time the player has to hit this note. after this time note cant be hit.
    private float _speed;
    private float _distance;
    private float _despawnTime; // amount of time the note stays visible
    
    public double timeStamp; // the time this note needs to be hit, used for other calculations 

    [Header("References")] 
    private Drum _drum; // will need this reference to change the index  
    
    public void Initialize(Drum drumPath, float noteSpeed, float distance, float missMargin, double hitTimeStamp, float despawnTime)
    { 
        _drum = drumPath;
        
        _speed = noteSpeed;
        _distance = distance;
        
        _missMargin = missMargin;
        timeStamp = hitTimeStamp;
        
        _despawnTime = despawnTime;
    }

    public void Start() // few seconds after instantiating destroy the note. this way the note doesnt vanish in players view 
    {
       StartCoroutine(AbleToGetHit()); 
       
       Destroy(gameObject, (_distance/_speed) + _despawnTime);
    }

    IEnumerator AbleToGetHit() // after a certain time note won't be able to b hit anymore, but this way it doesnt randomly dissapear from your screen.
    {
        yield return new WaitForSeconds(_distance / _speed + _missMargin);
        
        ScoreManager.Instance.Miss();
        _drum.currentNoteIndex++; // increase note index so the note after this one will be the one called 
    }

    void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }
}
