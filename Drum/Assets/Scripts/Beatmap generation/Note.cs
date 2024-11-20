using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("")]
    public Transform drumObject;
    private Transform beginTransform;
    private double timeInstantiated;
    void Start()
    {
        beginTransform = transform;
        if (BeatmapManager.Instance != null)
        {
            timeInstantiated = BeatmapManager.GetAudioSourceTime();
        }
        else
        {
            Debug.LogWarning("Beatmap Manager is null!");
        }
        
    }
    
    void Update()
    {
        double timeSinceInstantiated = BeatmapManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (BeatmapManager.Instance.noteSpeed * 2));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(beginTransform.position, drumObject.position, t);
        }
       
    }
}
