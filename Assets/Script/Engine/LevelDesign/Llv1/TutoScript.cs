using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoScript : MonoBehaviour
{
    [SerializeField]
    GameObject item,arrow,arrowInter;

    void Awake()
    {
        item.SetActive(false);
        arrow.SetActive(false);
        arrowInter.SetActive(false);
    }
    public void SHOWITEM(){
        arrowInter.SetActive(false);
        item.SetActive(true);
        arrow.SetActive(true);
    }
    public void SHOWINTERACTABLE(){
        arrowInter.SetActive(true);
    }
}
