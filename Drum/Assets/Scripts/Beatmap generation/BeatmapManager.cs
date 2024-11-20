using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Standards;


public class BeatmapManager : MonoBehaviour
{
    public static BeatmapManager Instance;

    [Header("Note Details")] 
    public float noteSpeed; //the speed the note moves towards the drum
    public Transform noteSpawnPoint;
    public List<DrumHits> drumHits;
    
    [SerializeField] private MidiFile midiFile; 
    public SongData _song;
    public AudioSource songAudioSource;

    public void Awake()
    {
        Instance = this;
    }
    
  #region Extract Drum notes

  public void ReadMidiFile(string midiFilePath)
  {
      midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + midiFilePath);
      
      drumHits = GetDrumHits();
      
      if (drumHits != null)
      { 
          Debug.Log($"Number of DrumHits in the list: {drumHits.Count}"); 
      }
      else
      {
          Debug.Log("DrumHits list is null!");
      }
  }
  public List<DrumHits> GetDrumHits() // extracts the drum notes from the file 
  {
      drumHits = new List<DrumHits>();
      
      TempoMap tempoMap = midiFile.GetTempoMap();
      
      foreach (var note in midiFile.GetNotes())
      {
          //check if the note is in channel 10 also known for being the drum channel (note.channel starts at 0)
          if (note.Channel == 9) 
          { 
              print($"Note Channel 10: {note.Channel}");
              Debug.Log($"Note Number: {note.NoteNumber}");
              
              // check if the note is playable on the virtual drum kit
              if (IsDrumNote(note.NoteNumber))
              { 
                  // convert ticks into seconds, 
                  MetricTimeSpan metricTimeSpan = note.TimeAs<MetricTimeSpan>(tempoMap);
                  double seconds = metricTimeSpan.TotalSeconds;
                  
                  // Add to list 
                  drumHits.Add(new DrumHits { Time = seconds, NoteNumber = note.NoteNumber });
              }
              else
              {
                  print("note is not a DrumNote!");
              }
          }
      }
      
      return drumHits; 
  }
  
  
  public bool IsDrumNote(int noteNumber) // check what drum part plays this note, each number is a different drum part
  {
      // check if the drum note has one of these note numbers 
      int[] drumNotes = { 35, 36, 38, 40, 41, 42, 43, 46, 47, 48, 49, 50 };  
        
      // 35/36 = Bass/kick, 38/40 = Snare, 41/43 = Floor Tom, 42 = Hi-Hat closed,
      // 46 = Hi-Hat open, 47/48 = Mid tom, 49 = Crash cymbal, 50 = high tom,  
      
      return Array.Exists(drumNotes, n => n == noteNumber);
  }

  // public enum DrumNoteType : byte
  // {
  //   bass = 35,
  //   kick = 36,
  //   snare = 38,
  //   floorTom = 40
  // }
  
  #endregion

  public static double GetAudioSourceTime() // get time of audioSource used for spawning notes
  {
      return (double)Instance.songAudioSource.timeSamples / Instance.songAudioSource.clip.frequency;
  }

  public void PlaySong(SongData songData)
  {
      songAudioSource.clip = songData.SongAudioClip;
      
      songAudioSource.Play();
      
      ReadMidiFile(_song.midiFile);
  }
  
}

[Serializable]
public class DrumHits
{
    public double Time { get; set; } 
    public int NoteNumber { get; set; }
}