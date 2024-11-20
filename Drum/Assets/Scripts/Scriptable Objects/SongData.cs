using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "ScriptableObject/SongData")]
public class SongData : ScriptableObject
{ 
    [Header("Song & Beatmap Details")] // Details that get displayed at the level selector 
    public string songName;
    public string songAuthor;
    public string difficulty;
    public string songDuration;
    
    public int bpm;
    public Sprite songIcon;
    
    [Header("MIDI & Audio")] // Data needed for generating the beatmap
    public String midiFile;
    public AudioClip SongAudioClip;
}
