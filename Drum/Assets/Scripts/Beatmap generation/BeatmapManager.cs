using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine.Networking;
using System.IO;

public class BeatmapManager : MonoBehaviour
{
    public static BeatmapManager Instance;
    
    [Header("Note Details")] 
    public float noteSpeed; //the speed the note moves towards the drum
    public float noteDespawn;
    
    [Header("Paths")]
    public Path[] paths;

    [Header("Note Spawn Details")] 
    public float distance; //distance from spawnpoint
    public float globalTime; // used to calculate when to spawn the notes
    private float _leadInTime; // time it takes for the note to hit the drum 
    
    [Header("Song Details")]
    public int inputDelayInMilliseconds;
    [SerializeField] private int startDelay;
    [SerializeField] private int endDelay;
    
    [Header("Audio references")]
    public SongData songData;
    [SerializeField] private AudioSource songAudioSource;
    private MidiFile _midiFile; // reference to midi file it needs to read 
    
    [Header("Drum Roll Settings")]
    [SerializeField] private float drumRollThreshhold;
    [SerializeField] private DrumSticks[] drumSticks;
    
    public void Awake()
    {
        Instance = this;
    }

    public void StartSong() // should make this into a function you can call through UI instead of just a start 
    {
        StateManager.Instance.SetState(DrumState.InGame); // Set GameState to ingame
        
        globalTime = 0;
        _leadInTime = distance / noteSpeed; // Calculates how long it takes for the notes to reach the destination
        globalTime = -_leadInTime - startDelay; // Calculates any delays necessary  
        songAudioSource.clip = songData.SongAudioClip; // Change audio clip to the appropriate song
        StartCoroutine(PlaySongWithLeadIn()); // Starts the song after a delay so the notes can catch up 
        
          // Update the presetSelector in ParticleManage
        var particleManager = FindObjectOfType<ParticleManage>();
        particleManager.presetSelector = songData.environmentPreset; // deze dingetje veranderd de enviorment + particle preset :3 hoi can ik heb je script aangeraakt
        particleManager.isPresetActive = true;
        
        
        
        //ReadMidiFile(songData.midiFile); // Need to change this for the level selector later 
        StartCoroutine(ReadMidiFile(songData.midiFile));
        StartCoroutine(EndSong()); // Starts coroutine to stop song
    }

    private IEnumerator PlaySongWithLeadIn()
    {
        yield return new WaitForSeconds(_leadInTime + startDelay); // Wait for lead-in and delay
        
        songAudioSource.Play(); // Start playing the song
    }

    private IEnumerator EndSong()
    {
        yield return new WaitForSeconds(songAudioSource.clip.length + endDelay + startDelay + _leadInTime);
        ScoreManager.Instance.SetHighScore(songData.songName);
        // Need to do some funky stuff with the end screen slider
        EndScreen.Instance.ChangeUI();
        StateManager.Instance.SetState(DrumState.EndOfSong);
    }
    
  #region Extract Drum notes

  /*
  public void ReadMidiFile(string midiFilePath)
  {
      _midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + midiFilePath);
      
      DistributeNotesToPaths();
  }
  */
  private IEnumerator ReadMidiFile(string midiFilePath)
  {
      string path = System.IO.Path.Combine(Application.streamingAssetsPath, midiFilePath);
      
      Debug.Log($"Loading MIDI file from: {path}");

      UnityWebRequest request = UnityWebRequest.Get(path);
      yield return request.SendWebRequest();

      if (request.result == UnityWebRequest.Result.Success)
      {
          try
          {
              byte[] midiData = request.downloadHandler.data;
              using (var stream = new MemoryStream(midiData))
              {
                  _midiFile = MidiFile.Read(stream);
                  Debug.Log("MIDI file successfully read.");
                  DistributeNotesToPaths(); // Process the notes from the MIDI file
              }
          }
          catch (System.Exception ex)
          {
              Debug.LogError($"Error reading MIDI file: {ex.Message}");
          }
      }
      else
      {
          Debug.LogError($"Failed to load MIDI file: {request.error}");
      }
  }
  private void DistributeNotesToPaths() // distributes the notes to their designed drum kit 
  {
      TempoMap tempoMap = _midiFile.GetTempoMap();
      
      Dictionary<int, double> lastNoteTimes = new Dictionary<int, double>(); // dictionary to keep a reference to the last notes timer. 
      
      foreach (var note in _midiFile.GetNotes())
      {
          if (note.Channel != 9) continue; // Check if the note is in the drum channel 
          
          // Convert ticks to seconds
          MetricTimeSpan metricTimeSpan = note.TimeAs<MetricTimeSpan>(tempoMap);
          double seconds = metricTimeSpan.TotalSeconds;
              
          bool added = false; 
          // Add the note to the matching path
          foreach (var path in paths)
          {
              if (!path.CheckNoteNumber(note.NoteNumber)) continue;
              
              // Spawn the note object to enable later
              GameObject noteObject = Instantiate(path.notePrefab, path.transform);
              Note noteComponent = noteObject.GetComponent<Note>();

              if (lastNoteTimes.TryGetValue(note.NoteNumber, out double lastTime)) 
              {
                  // Check the timestamp of the previous note in that path
                  if (seconds - lastTime <= drumRollThreshhold)
                  {
                      // Checks if the time between notes is within a certain threshold 
                      noteComponent.drumRollable = true;
                  }
              }
                      
              lastNoteTimes[note.NoteNumber] = seconds; // sets the new timer for 

              // Initialize the note component with relevant data
              noteComponent.Initialize(noteSpeed, distance, seconds, noteDespawn);

              path.AddNoteObject(noteObject); // Store the reference to the spawned note object

              ScoreManager.Instance.noteCount++;
              added = true;
              break;
          }

          if (!added)
          {
              Debug.LogWarning($"Note {note.NoteNumber} does not match any path!");
          }
      }
      ScoreManager.Instance.NoteScoreCalculations();
  }
  
  private void Update() // starts the global time and looks for notes that should be spawned
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

              double adjustedSpawnTime = note.GetComponent<Note>().timeStamp - _leadInTime; // Calculate when note should be spawned
              if (globalTime >= adjustedSpawnTime) // spawn at adjusted time
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