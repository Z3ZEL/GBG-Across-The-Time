using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDetection : MonoBehaviour
{

 

    bool isDetected = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" ) isDetected = true;

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" ) isDetected = false;
    }


    private void Update()
    {
        if(!FindObjectOfType<CharacterControl>().isFlashing && isDetected && Input.GetKeyDown(KeyCode.F)){
            FindObjectOfType<ImageDetectionManager>().AddObjectFound();
            gameObject.SetActive(false);
        }
    }   
}
