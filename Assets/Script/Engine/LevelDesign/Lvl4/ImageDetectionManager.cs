using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDetectionManager : MonoBehaviour
{
    [SerializeField]
    GameObject item;
    [SerializeField]
    int objectToDetect=7,currentObjectFound=0;

    public void AddObjectFound(){
        currentObjectFound++;
        if(currentObjectFound==objectToDetect){
            item.SetActive(true);
        }
    }
}
