using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelect : MonoBehaviour
{
   public SongData[] songs;
   public int currentSong;

   public void SelectSong()
   {
      BeatmapManager.Instance.song = songs[currentSong];
   }

   public void NextSong()
   {
      currentSong++;
   }

   public void PreviousSong()
   {
      currentSong--;
   }
}
