using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalActiver : ButtonTrigger
{
    [SerializeField]
    private GameObject[] portals;

    void Start()
    {
        foreach(GameObject portal in portals){
                portal.SetActive(false);
        }
        tagName = "Golf";
        action += () => {
            AudioController.instance.PlaySound("Portal");
            AudioController.instance.PlaySoundEffect("GolfEnter",0.08f);
            foreach(GameObject portal in portals){
                portal.SetActive(true);
            }
        };   

    }


}
