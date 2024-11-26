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
    
    [Header("Paths")]
    public Path[] paths;

    [Header("Note Spawn Details")] 
    public float distance; //distance from spawnpoint
    private float _leadInTime; // time it takes for the first note to hit the drum 
    public float globalTime; // used to calculate when to spawn the notes
    public int startDelay;
    public float missMargin;
    
    [Header("Audio references")]
    public SongData _song;
    public AudioSource songAudioSource;
    private MidiFile _midiFile; // reference to midi file it needs to read 
    
    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _leadInTime = distance / noteSpeed;
        globalTime = -_leadInTime - startDelay;
        ReadMidiFile(_song.midiFile);
        songAudioSource.clip = _song.SongAudioClip;
        //AdjustNoteTimesForLeadIn(_song);
        StartCoroutine(PlaySongWithLeadIn());
    }

    private IEnumerator PlaySongWithLeadIn()
    {
        yield return new WaitForSeconds(_leadInTime + startDelay); // Wait for lead-in and delay
        
        songAudioSource.Play(); // Start playing the song
    }
    
  #region Extract Drum notes

  public void ReadMidiFile(string midiFilePath)
  {
      _midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + midiFilePath);
      
      DistributeNotesToPaths();
  }
  private void DistributeNotesToPaths() // distributes the notes to their designed drum kit 
  {
      TempoMap tempoMap = _midiFile.GetTempoMap();

      foreach (var note in _midiFile.GetNotes())
      {
          // Check if the note is in the drum channel 
          if (note.Channel == 9)
          {
              // Convert ticks to seconds
              MetricTimeSpan metricTimeSpan = note.TimeAs<MetricTimeSpan>(tempoMap);
              double seconds = metricTimeSpan.TotalSeconds;

              // Create the DrumHits object
              var drumHit = new DrumHits(seconds, note.NoteNumber);

              // Add the note to the matching path
              bool added = false;
              foreach (var path in paths)
              {
                  if (path.CheckNoteNumber(note.NoteNumber))
                  {
                      path.AddNote(drumHit);
                      ScoreManager.Instance.noteCount++;
                      
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
      globalTime += Time.deltaTime;
      
      SpawnNotesBasedOnGlobalTime();
  }

  private void SpawnNotesBasedOnGlobalTime() // Spawns notes prematurely to compensate with the song lead in time
  {
      foreach (var path in paths)
      {
          for (int i = 0; i < path.notes.Count; i++)
          {
              double adjustedSpawnTime = path.notes[i].time - _leadInTime; // Calculate when the notes should be spawned to be in time with the beat

              // Spawn the note if the global time matches the adjusted spawn time
              if (globalTime >= adjustedSpawnTime)
              {
                  SpawnNoteAtPath(path);
                  path.notes.RemoveAt(i);
                  i--; 
              }
          }
      }
  }


  private void SpawnNoteAtPath(Path path)
  {
      // Use the Path transform to instantiate the note
      GameObject note = Instantiate(path.notePrefab, path.transform.position, path.transform.rotation);
      
      // set the noteSpeed 
      note.GetComponent<Note>().Initialize(noteSpeed, distance, missMargin); 
  }
  
  #endregion

  public static double GetAudioSourceTime() // get time of the song in seconds
  {
      return (double)Instance.songAudioSource.timeSamples / Instance.songAudioSource.clip.frequency;
  }
  
}

[System.Serializable]
 public struct DrumHits
 {
     public DrumHits(double times, int noteNumbers)
     {
         time = times; 
         noteNumber = noteNumbers;
     }
     public double time;
     public int noteNumber;
 }