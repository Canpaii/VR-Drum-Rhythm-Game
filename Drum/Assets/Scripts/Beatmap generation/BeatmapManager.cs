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
    public GameObject notePrefab;
    
    [Header("Paths")]
    public Path[] paths;
    
    static MidiFile midiFile; // reference to midi file it needs to read 
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
      
      DistributeNotesToPaths();
  }
  private void DistributeNotesToPaths() // distributes the notes to their designed drum kit 
  {
      TempoMap tempoMap = midiFile.GetTempoMap();

      foreach (var note in midiFile.GetNotes())
      {
          // Check if the note is in the drum channel 
          if (note.Channel == 9)
          {
              // Convert ticks to seconds
              MetricTimeSpan metricTimeSpan = note.TimeAs<MetricTimeSpan>(tempoMap);
              double seconds = metricTimeSpan.TotalSeconds;

              // Create the DrumHits object
              var drumHit = new DrumHits { Time = seconds, NoteNumber = note.NoteNumber };

              // Add the note to the matching path
              bool added = false;
              foreach (var path in paths)
              {
                  if (path.CheckNoteNumber(note.NoteNumber))
                  {
                      path.AddNote(drumHit);
                      added = true;
                      break;
                  }
              }

              if (!added)
              {
                  Debug.LogWarning($"Note {note.NoteNumber} does not match any path!");
              }
          }
      }
  }
  
  private void Update()
  {
      double currentTime = GetAudioSourceTime();

      foreach (var path in paths)
      {
          for (int i = 0; i < path.notes.Count; i++)
          {
              if (path.notes[i].Time <= currentTime)
              {
                  SpawnNoteAtPath(path);
                  path.notes.RemoveAt(i);
                  i--; // Adjust index after removal
              }
          }
      }
  }

  private void SpawnNoteAtPath(Path path)
  {
      // Use the Path transform to instantiate the note
      GameObject note = Instantiate(notePrefab, path.transform.position, Quaternion.identity);
      
      // set the noteSpeed 
      note.GetComponent<Note>().Initialize(noteSpeed); 
  }
  
  #endregion

  public static double GetAudioSourceTime() // get time of the song in seconds
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