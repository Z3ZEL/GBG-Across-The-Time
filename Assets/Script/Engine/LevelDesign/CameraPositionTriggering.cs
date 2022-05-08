using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionTriggering : MonoBehaviour
{

    [SerializeField]
    int index;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Camera.main.GetComponent<CameraPosition>().GoTo(index);
        }

    }
}
