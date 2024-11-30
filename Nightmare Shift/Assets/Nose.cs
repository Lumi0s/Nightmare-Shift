using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nose : MonoBehaviour
{

void OnMouseOver()
{
   if(Input.GetMouseButtonDown(0))
   {
      SoundManager.Instance.PlaySound("Nose");
   }
}
}
