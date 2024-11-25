using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private float speed;
    
    private Transform beginTransform;
    private double timeInstantiated;
    
    public void Initialize(float noteSpeed)
    { 
        speed = noteSpeed;
    }
    
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
