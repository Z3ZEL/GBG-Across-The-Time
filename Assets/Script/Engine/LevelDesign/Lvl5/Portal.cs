using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kovnir.FastTweener;

public class Portal : MonoBehaviour
{
    [SerializeField]
    float duration=8,sizeReducer=0.5f;
    [SerializeField]
    public bool isEnterPortal=false;
    void Start()
    {
        RotateInfinitely();
    }

    public void RotateInfinitely()
    {
        FastTweener.Float(transform.localEulerAngles.z, transform.localEulerAngles.z + 360, duration, (value) => {
            if (this == null || transform == null) return;
            transform.localEulerAngles = new Vector3(0, 0, value);
        },Ease.Linear,false,new System.Action(() => {
            RotateInfinitely();
        }));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isEnterPortal)
        {
           Portal[] portals = FindObjectsOfType<Portal>();
           //FIND NOT ENTER PORTAL
           foreach(Portal portal in portals)
           {
               if(!portal.isEnterPortal)
               {
                   //TELEPORT
                   other.gameObject.transform.position = portal.transform.position;
                   //REDUCE SIZE
                   other.gameObject.transform.localScale *= sizeReducer;

               }
           }
           
        }
    }
}
