using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drum Presets", menuName = "ScriptableObject/Drum Presets")]
public class DrumSampleSounds : ScriptableObject
{
    [Header("Drum Clips")] 
    public AudioClip snareClip;
    public AudioClip bassClip;
    public AudioClip crashCymbalClip;
    public AudioClip midTomClip;
    public AudioClip highTomClip;
    public AudioClip hiHatClosedClip;
    public AudioClip hiHatOpenClip;
    public AudioClip floorTomClip;
}
