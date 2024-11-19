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
  static SongData _song;
  
  public void StartSong()
  {
    GetDrumHits(_song.midiFile ,_song.bpm);
  }
  
  #region Extract Drum notes 
  
  public class DrumHits
  {
    public double Time { get; set; } 
    public int NoteNumber { get; set; }
  }
  
  public List<DrumHits> GetDrumHits(string midiFilePath, int bpm) // extracts the drum notes from the file 
  {
    List<DrumHits> drumHits = new List<DrumHits>();
    MidiFile midiFile = MidiFile.Read(midiFilePath);
    
    TempoMap tempoMap = midiFile.GetTempoMap();
    foreach (var note in midiFile.GetNotes())
    {
      //check if the note is in channel 10 also known for being the drum channel
      if (note.Channel == 10) 
      {
        // check if the note is playable on the virtual drum kit
        if (IsDrumNote(note.NoteNumber))
        {
          // convert ticks into seconds, 
          MetricTimeSpan metricTimeSpan = note.TimeAs<MetricTimeSpan>(tempoMap);
          double seconds = (int)metricTimeSpan.TotalSeconds;
          
          drumHits.Add(new DrumHits { Time = seconds, NoteNumber = note.NoteNumber });
        }
      }
    }
    
    return drumHits;
  }
  
  

  
  /// <summary>
  /// abfc
  /// </summary>
  /// <param name="noteNumber"></param>
  /// <returns></returns>
  public bool IsDrumNote(int noteNumber) // check what drum part plays this note, each number is a different drum part
  {
    // check if the drum note has one of these note numbers 
    int[] drumNotes = { 35, 36, 38, 40, 41, 42, 43, 46,47, 48, 49, 50 };  
    
    // 35/36 = Bass/kick, 38/40 = Snare, 41/43 = Floor Tom, 42 = Hi-Hat closed,
    // 46 = Hi-Hat open, 47/48 = Mid tom, 49 = Crash cymbal, 50 = high tom,  
    
    return System.Array.Exists(drumNotes, n => n == noteNumber);
  }

  // public enum DrumNoteType : byte
  // {
  //   bass = 35,
  //   kick = 36,
  //   snare = 38,
  //   floorTom = 40
  // }
  
  #endregion

  #region Lane management

  public GameObject notePrefabGO;
  private int _spawnIndex = 0;

  #endregion
  
  //DrumHits[] _drumHits;
}

// [Serializable]
// public class DrumHits
// {
//   public double Time { get; set; } 
//   public int NoteNumber { get; set; }
// }