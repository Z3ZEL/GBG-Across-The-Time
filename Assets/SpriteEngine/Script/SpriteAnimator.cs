using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

public class SpriteAnimator : MonoBehaviour
{
    [Header("Sprite Catalogue")]
    [SerializeField]
    private Sprite[] sprites = null;

    [Header("Animation Duration")]
    [Range(0, 20)]
    [SerializeField]
    float duration = 2;

    //EVENTS
    [Header("Animation Basic Events")]
    public CustomTimeEvent OnStart = new CustomTimeEvent();
    public CustomTimeEvent OnEndFrame = new CustomTimeEvent();
    public CustomTimeEvent OnEnd = new CustomTimeEvent();

    [SerializeField]
    bool playOnAwake=true,loop=false;

    //HANDLE CUSTOM TIMING
    [HideInInspector,SerializeField]
    List<CustomTime> customTimes = new List<CustomTime>();
    private bool isSpriteRenderer;

    //HANDLE RENDERING
    SpriteRenderer Srenderer = null;
    Image Irenderer = null;



    //ENGINE
    SpriteEngine engine;

    private void OnValidate()
    {
        if(Application.isPlaying) Apply();
    }

    public void Apply()
    {
        if (engine == null) return;
        engine.Sprites = sprites;
        engine.Duration = duration;
        engine.Loop = loop;
        engine.CustomTimes = customTimes;
        engine.OnEnd = OnEnd;
        engine.OnEndFrame = OnEndFrame;
        engine.OnStart = OnStart;
    }


 




    //GETTER AND SETTER
    public Sprite[] Sprites { get => sprites; set => sprites = value; }
    public bool PlayOnAwake { get => playOnAwake; set => playOnAwake = value; }
    public bool Loop { get => loop; set => loop = value; }
    public float Duration { get => duration; set => duration = value; }
    internal List<CustomTime> CustomTimes { get => customTimes; set => customTimes = value; }
    public SpriteEngine Engine { get => engine; set => engine = value; }

    private void Awake()
    {
        //GET COMPONENT
        if (gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
        {
            isSpriteRenderer = true;
            Srenderer = renderer;
        }
        else if (gameObject.TryGetComponent<Image>(out Image image))
        {
            isSpriteRenderer = false;
            Irenderer = image;
        }
        else
        {
            Debug.LogError("No Image or SpriteRenderer found");
            return;
        }

        if (isSpriteRenderer) engine = new SpriteEngine(Srenderer, sprites, duration, loop);
        else engine = new SpriteEngine(Irenderer, sprites, duration, loop);

        Apply();

        if (playOnAwake) engine.PlaySpriteAnim();
    }


}

