using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ButtonTrigger : MonoBehaviour
{

    public UnityAction action;
    public string tagName="Player";
    public bool hasBeenTriggered = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == tagName && !hasBeenTriggered)
        {
            Debug.Log("Button Triggered");
            hasBeenTriggered = true;
            action.Invoke();
        }
    }



}
