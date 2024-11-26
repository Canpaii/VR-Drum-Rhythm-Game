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
    
    [Header("Core Variables")]
    private float _missMargin;
    private float speed;
    private float _distance;
    
    public void Initialize(float noteSpeed, float distance, float missMargin)
    { 
        speed = noteSpeed;
        _distance = distance;
        _missMargin = missMargin;
    }

    public void Start() // few seconds after instantiating destroy the note and count it as a miss if not hit.
    {
        Destroy(gameObject,(_distance / speed) + _missMargin);
    }

    public void OnDestroy()
    {
        if (!hit)
        {
            ScoreManager.Instance.Miss();
        }
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
