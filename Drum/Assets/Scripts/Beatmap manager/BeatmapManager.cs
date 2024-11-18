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
  [Header("TimeStamps")] 
  public float timeStamp;
 
  private MidiFile _midiFile = MidiFile.Read(" *Insert MidiFile name*"); //string name is the file it is gonna read
  
  
}
