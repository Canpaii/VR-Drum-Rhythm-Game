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
    public float normalHitMargin;
    public float noteDespawn;
    
    [Header("Paths")]
    public Path[] paths;

    [Header("Note Spawn Details")] 
    public float distance; //distance from spawnpoint
    public float globalTime; // used to calculate when to spawn the notes
    public int startDelay;
    
    private float _leadInTime; // time it takes for the note to hit the drum 
    
    [Header("Song Details")]
    public int inputDelayInMilliseconds;
    
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
              
              bool added = false; 
              // Add the note to the matching path
              foreach (var path in paths)
              {
                  if (path.CheckNoteNumber(note.NoteNumber))
                  {
                      // Spawn the note object to enable later
                      GameObject noteObject = Instantiate(path.notePrefab, path.transform.position, path.transform.rotation);
                      Note noteComponent = noteObject.GetComponent<Note>();

                      // Initialize the note component with relevant data
                      noteComponent.Initialize(noteSpeed, distance, normalHitMargin, seconds, noteDespawn);

                      path.AddNoteObject(noteObject); // Store the reference to the spawned note object

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

  private void SpawnNotesBasedOnGlobalTime()
  {
      foreach (var path in paths)
      {
          for (int i = 0; i < path.notes.Count; i++)
          {
              GameObject note = path.notes[i];

              if (note == null) 
              {
                  path.notes.RemoveAt(i); // Remove destroyed notes
                  i--;
                  continue;
              }

              double adjustedSpawnTime = note.GetComponent<Note>().timeStamp - _leadInTime;
              if (globalTime >= adjustedSpawnTime)
              {
                  note.SetActive(true);
              }
          }
      }
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