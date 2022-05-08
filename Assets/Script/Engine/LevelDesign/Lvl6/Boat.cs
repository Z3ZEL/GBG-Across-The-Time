using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField]
    Transform controlPoint1, controlPoint2;
    [SerializeField]
    Transform boatTr,playerTr;
    [SerializeField]
    float speed = 3;
    bool isFollowingPlayer = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //CHECK IF PLAYER POSITION IS BETWEEN THE TWO CONTROL POINTS
            if (playerTr.position.x > controlPoint1.position.x && playerTr.position.x < controlPoint2.position.x)
            {
                isFollowingPlayer = true;
            }else {
                isFollowingPlayer = false;
            }
        }
    }


    void Update(){
        if(isFollowingPlayer){
            //FOLLOWING PLAYER POSITION X
            Vector3 pos = boatTr.position;
            pos.x = Mathf.Lerp(pos.x, playerTr.position.x, speed * Time.deltaTime);
            boatTr.position = pos;
        
        }
    }

}
