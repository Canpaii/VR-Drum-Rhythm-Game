using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DrumSfxSelect : MonoBehaviour
{
    [Header("Drum Sfx Presets")]
    public DrumSampleSounds[] drumSampleSounds;
    public int selectedDrumSounds;
    
    [Header("AudioSources")]
    public AudioSource snareSource;
    public AudioSource hiHatClosedSource;
    public AudioSource hiHatOpenSource;
    public AudioSource kickSource;
    public AudioSource midTomSource;
    public AudioSource highTomSource;
    public AudioSource floorTomSource;
    public AudioSource crashCymbalSource;
    
    public AudioSource[] drumSticksSource;
    
    
    
    public void DistributeSounds()
    {
        snareSource.clip = drumSampleSounds[selectedDrumSounds].snareClip;
        hiHatClosedSource.clip = drumSampleSounds[selectedDrumSounds].hiHatClosedClip;
        hiHatOpenSource.clip = drumSampleSounds[selectedDrumSounds].hiHatOpenClip;
        kickSource.clip = drumSampleSounds[selectedDrumSounds].bassClip;
        midTomSource.clip = drumSampleSounds[selectedDrumSounds].midTomClip;
        highTomSource.clip = drumSampleSounds[selectedDrumSounds].highTomClip;
        floorTomSource.clip = drumSampleSounds[selectedDrumSounds].floorTomClip;
        crashCymbalSource.clip = drumSampleSounds[selectedDrumSounds].crashCymbalClip;

    }
}
