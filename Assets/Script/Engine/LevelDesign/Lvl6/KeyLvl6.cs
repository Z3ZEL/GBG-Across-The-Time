using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLvl6 : ButtonTrigger
{
    [SerializeField]
    GameObject item;
    [SerializeField]
    SpriteRenderer doorRenderer;
    [SerializeField]
    Sprite doorOpen;

    void Awake(){
        item.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        action += () => {
            item.SetActive(true);
            doorRenderer.sprite = doorOpen;
            gameObject.SetActive(false);

        };
    }


}
