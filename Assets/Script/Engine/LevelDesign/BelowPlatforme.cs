using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelowPlatforme : MonoBehaviour
{
    private BoxCollider2D bc;
    private Vector2 sizePlatform;
    private Vector3 taillePlatform;
    private Vector3 originPlatform;
    private float hautPlatform;
    
    private GameObject[] player;
    private Vector2 sizePlayer;
    private Vector3 taillePlayer;
    private Vector3 originPlayer;
    private float piedPlayer;



    void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        originPlatform = transform.position;
        taillePlatform = transform.localScale;
        sizePlatform = bc.size;
        hautPlatform = sizePlatform.y*taillePlatform.y;
        hautPlatform/=2;
        hautPlatform+=originPlatform.y;
 
        bc.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject g in player)
        {
            BoxCollider2D collider = g.GetComponent<BoxCollider2D>();
            sizePlayer = collider.size;
            taillePlayer = collider.gameObject.transform.localScale;
            originPlayer = collider.gameObject.transform.position;
            piedPlayer=0;
            piedPlayer = -sizePlayer.y*taillePlayer.y;
            piedPlayer /=2;
            piedPlayer += originPlayer.y; 
        }
        
        if( piedPlayer >= hautPlatform-0.1f)
        {
            bc.isTrigger = false;
        }
        else
        {
            bc.isTrigger = true;
        }
    }
    
}

