using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWindowLvl3 : ButtonTrigger
{

    [SerializeField]
    GameObject item;

    [SerializeField]
    SpriteRenderer windowsRenderer,buttonRenderer;
    [SerializeField]
    Sprite windowsOpenSprite,ButtonPressedSprite;

    // Start is called before the first frame update
    void Start()
    {
        action += () => {
            windowsRenderer.sprite = windowsOpenSprite;
            buttonRenderer.sprite = ButtonPressedSprite;
            item.SetActive(true);
        };
    }

    
}
