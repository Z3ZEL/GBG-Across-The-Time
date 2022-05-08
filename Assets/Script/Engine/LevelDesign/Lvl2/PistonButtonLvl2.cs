using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kovnir.FastTweener;

public class PistonButtonLvl2 : ButtonTrigger
{
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Sprite pressedSprite;
    [SerializeField]
    GameObject itemToShift;
    [SerializeField]
    float shift = -2, shiftSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        action += () => {
            itemToShift.GetComponent<Animator>().enabled = false;
            spriteRenderer.sprite = pressedSprite;
            FastTweener.Float(itemToShift.transform.position.y, itemToShift.transform.position.y + shift, shiftSpeed, (value) => {
                if (this == null) return;
                itemToShift.transform.position = new Vector3(itemToShift.transform.position.x, value, itemToShift.transform.position.z);
            }); 
        };
        
    }

   
}
