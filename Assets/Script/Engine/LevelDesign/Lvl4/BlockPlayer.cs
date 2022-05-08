using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlayer : MonoBehaviour
{

    [SerializeField]
    Vector2 shift;
    [SerializeField]
    float shiftforce = 1;
   
    void OnTriggerEnter2D(Collider2D other){
        //BLOCK PLAYER MOVEMENT IN VECTOR STOPDIR
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position += new Vector3(shift.x* shiftforce, shift.y* shiftforce, 0) ;
        }
    }
}
