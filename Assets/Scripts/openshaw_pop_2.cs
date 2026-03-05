using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.
using System.Collections.Generic;

public class openshaw_pop_2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  public float sizeX;
  public float sizeY;
  public float sizeZ;

  public void Start()
  {
    sizeX = gameObject.transform.localScale.x;
    sizeY = gameObject.transform.localScale.y;
    sizeZ = gameObject.transform.localScale.z;
  }

  // This triggers when the mouse exits the area of this script's gameObject
  public void OnPointerExit(PointerEventData eventData2)
  {
    Debug.Log("mouse exit");
    gameObject.transform.localScale = new Vector3(sizeX, sizeY, sizeZ);
  }

  // This triggers when the mouse enters or goes over the area of this script's gameObject
  public void OnPointerEnter(PointerEventData eventData)
  {
    //Debug.Log sends messages we can read to the console window in Unity
    Debug.Log("mouse enter");
    // Multiplies the x, y, and z scale value of the gameObject this script is attached to by 1.5
    // The number is written as " 1.5f " because it is a float,
    // and needs to be written with an f attached to work properly
    gameObject.transform.localScale =
    new Vector3( sizeX * 1.5f,
                 sizeY * 1.5f,
                 sizeZ * 1.5f );
    //cube1.transform.localScale = new Vector3(sizeX * 1.5f, sizeY * 1.5f, sizeZ * 1.5f);
  }
}

