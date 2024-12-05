using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDebug : MonoBehaviour
{
   void Update()
       {
           foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
           {
               if (Input.GetKeyDown(key))
               {
                   Debug.Log("Key Pressed: " + key);
               }
           }
       }
}
