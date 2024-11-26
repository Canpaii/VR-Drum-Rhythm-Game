using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Note : MonoBehaviour
{
    public float missMargin;
    
    private float speed;
    private float _distance;
    private Transform beginTransform;
    private double timeInstantiated;
    private float timer;
    
    public void Initialize(float noteSpeed, float distance)
    { 
        speed = noteSpeed;
        _distance = distance;
    }

    public void Start() // few seconds after instantiating destroy the note and count it as a miss if not hit.
    {
        Destroy(gameObject,(_distance / speed) + missMargin );
    }

    public void OnDestroy()
    {
        
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        
        timer += Time.deltaTime;

        if (timer >= (_distance / speed) + )
        {
            Destroy(gameObject);
        }
    }
}
