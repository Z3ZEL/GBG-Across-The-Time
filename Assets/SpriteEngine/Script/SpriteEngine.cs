using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Threading;


public class SpriteEngine 
{
 

    //PRIVATE PARAMETERS
    private Sprite[] sprites = null;
    private float duration = 2;
    private bool loop = false;

    //EVENTS
    public CustomTimeEvent OnStart = new CustomTimeEvent();
    public CustomTimeEvent OnEnd = new CustomTimeEvent();
    public CustomTimeEvent OnEndFrame = new CustomTimeEvent();

    //PRIVATE DATA
    int spriteIndex = 0;
    bool isSpriteRenderer = true, isAnimating = false, isPaused = false;
    private CoroutineHandler timeHandler;
    private IEnumerator currentAnimating;

    //HANDLE CUSTOM TIMING
    List<CustomTime> customTimes = new List<CustomTime>();
    Dictionary<int, float> customTimeRegister = new Dictionary<int, float>();
    Dictionary<int, CustomTimeEvent> customTimeEventRegister = new Dictionary<int, CustomTimeEvent>();


    //HANDLE RENDERING
    SpriteRenderer Srenderer = null;
    Image Irenderer = null;



    //CONSTRUCTORS
    public SpriteEngine(SpriteRenderer _renderer,Sprite[] _sprites,float _duration,bool _loop)
    {
        timeHandler = _renderer.gameObject.AddComponent<CoroutineHandler>();
        isSpriteRenderer = true;
        Srenderer = _renderer;
        sprites = _sprites;
        duration = _duration;
        loop = _loop;
    }
    public SpriteEngine(Image _renderer, Sprite[] _sprites, float _duration, bool _loop)
    {
        timeHandler = _renderer.gameObject.AddComponent<CoroutineHandler>();
        isSpriteRenderer = false;
        Irenderer = _renderer;
        sprites = _sprites;
        duration = _duration;
        loop = _loop;
    }
    public SpriteEngine(SpriteRenderer _renderer, Sprite[] _sprites, float _duration)
    {
        timeHandler = _renderer.gameObject.AddComponent<CoroutineHandler>();
        isSpriteRenderer = true;
        Srenderer = _renderer;
        sprites = _sprites;
        duration = _duration;
        loop = false;
    }
    public SpriteEngine(Image _renderer, Sprite[] _sprites, float _duration)
    {
        timeHandler = _renderer.gameObject.AddComponent<CoroutineHandler>();
        isSpriteRenderer = false;
        Irenderer = _renderer;
        sprites = _sprites;
        duration = _duration;
        loop = false;
    }



    

    //PUBLIC METHOD
    public void PlaySpriteAnim()
    {
        currentAnimating = StartAnimate();


        //CHECK IF PAUSED 
        if (isPaused) isAnimating = false;


        //CHECK DOUBLE INSTANCE
        if (isAnimating)
        {
            Debug.Log("The Sprite is Already animating");
            return;
        }
        //CHECK SPRITE NULL
        if (isSpriteNull())
        {
            Debug.LogError("No Sprite were found");
            return;
        }

        //CREATING THE CUSTOM TIME DICTIONNARY
        if (customTimes != null)
        {
            customTimeRegister = new Dictionary<int, float>();
            customTimeEventRegister = new Dictionary<int, CustomTimeEvent>();
            foreach (CustomTime time in customTimes)
            {
                if (customTimeRegister.ContainsKey(time.spriteIndex)) Debug.LogWarning("There are two custom sprite timing for one same sprite index, one will be ignored");
                else
                {
                    customTimeRegister.Add(time.spriteIndex, time.timeGapPerCent / 100);
                    if (time.Event != null) customTimeEventRegister.Add(time.spriteIndex, time.Event);
                }

            }
        }

        isAnimating = true;
        isPaused = false;
        timeHandler.StartCoroutine(currentAnimating);
    }
    public void StopSpriteAnim()
    {
        if (isAnimating == false) return;
        isAnimating = false;
        if (isSpriteRenderer) Srenderer.sprite = sprites[0];
        else Irenderer.sprite = sprites[0];
        spriteIndex = 0;
        timeHandler.StopCoroutine(currentAnimating);
    }
    public void PauseSpriteAnim()
    {
        if (isAnimating == false) return;
        isPaused = true;
        timeHandler.StopCoroutine(currentAnimating);
    }
    public void Restart()
    {
        StopSpriteAnim();
        PlaySpriteAnim();
    }

    //CUSTOM TIMING AND EVENT
    public void AddCustomSpriteTiming(int _spriteIndex, float _timeGapPerCent)
    {
        if (isSpriteNull()) return;
        if (isAlreadyCustomTimeAtSpriteIndex(_spriteIndex))
        {
            GetCustomTimeAtSpriteIndex(_spriteIndex).timeGapPerCent = _timeGapPerCent;
            return;
        }

        CustomTime _newTime = new CustomTime(duration, sprites.Length, _spriteIndex);
        _newTime.timeGapPerCent = _timeGapPerCent;
        customTimes.Add(_newTime);

        //CHECK IF UNDER 99%
        if (checkUnder99()) Debug.LogWarning("Warning : All the custom time gap added are over 100, some weird behaviors can happen");

    }
    public void AddCustomSpriteTiming(int _spriteIndex, float _timeGapPerCent, Action _customEvent)
    {
        if (isSpriteNull()) return;
        if (isAlreadyCustomTimeAtSpriteIndex(_spriteIndex))
        {
            GetCustomTimeAtSpriteIndex(_spriteIndex).timeGapPerCent = _timeGapPerCent;
            GetCustomTimeAtSpriteIndex(_spriteIndex).newEvent(new UnityAction(_customEvent));
            return;
        }

        CustomTime _newTime = new CustomTime(duration, sprites.Length, _spriteIndex);
        _newTime.timeGapPerCent = _timeGapPerCent;

        UnityAction tempAction = new UnityAction(_customEvent);
        _newTime.newEvent(tempAction);
        customTimes.Add(_newTime);

        //CHECK IF UNDER 99%
        if (checkUnder99()) Debug.LogWarning("Warning : All the custom time gap added are over 100, some weird behaviors can happen");
    }

    public void RemoveCustomTimeAtSpriteIndex(int index)
    {
        for(int i = 0; i < customTimes.Count; i++)
        {
            if (customTimes[i].spriteIndex == index) customTimes.RemoveAt(i);
        }
    }
    public bool isAlreadyCustomTimeAtSpriteIndex(int index)
    {
        foreach (CustomTime time in customTimes)
        {
            if (time.spriteIndex == index) return true;
        }

        return false;
    }
    public CustomTime GetCustomTimeAtSpriteIndex(int index)
    {
        foreach(CustomTime time in customTimes)
        {
            if (time.spriteIndex == index) return time;
        }
        Debug.Log("No custom time was found");
        return null;
    }


    //PRIVATE METHOD
    private IEnumerator StartAnimate()
    {


        //CHECK INDEX OUT OF RANGE
        if (spriteIndex >= sprites.Length) spriteIndex = 0;



        //VAR INIT
        int maxSprite = sprites.Length;
        float timeGap = (duration / maxSprite);
        float regularTimeGap = timeGap * (GetRegularTimeGap() / 100);
        float initDuration = duration;

        //CALLING UNITY ACTION
        OnStart.Invoke();

        //PLAY THE ANIMATION
        for (int i = spriteIndex; i < maxSprite; i++)
        {
            float tempTimeGap = regularTimeGap;

            //CHANGE SPRITE
            if (isSpriteRenderer) Srenderer.sprite = sprites[spriteIndex];
            else Irenderer.sprite = sprites[spriteIndex];

            //STOP AND PAUSE AND VALUE CHANGE ANIMATING WATCHER
            if (initDuration != duration)
            {
                i = maxSprite;
                spriteIndex = 0;
                yield return null;
            }
  
            //CHECK FOR CUSTOM TIME MODIFIER
            if (customTimeRegister.ContainsKey(spriteIndex))
            {
                tempTimeGap = duration * customTimeRegister[spriteIndex];
                if (customTimeEventRegister.ContainsKey(spriteIndex)) customTimeEventRegister[spriteIndex].Invoke();
            }

            //CHECK INDEX OUT OF RANGE
            spriteIndex++;
            if (spriteIndex >= sprites.Length) spriteIndex = 0;

            //CALL EVENT
            OnEndFrame.Invoke();

            //INCREMENT TIME         
            yield return new WaitForSeconds(tempTimeGap);
        }



        //CHECK STATE OF ANIMATION
        if (loop && isAnimating && isPaused == false)
        {
            OnEnd.Invoke();
            timeHandler.StopCoroutine(currentAnimating);
            currentAnimating = StartAnimate();
            timeHandler.StartCoroutine(currentAnimating);
        }
        else if (loop == false && isPaused == false && isAnimating)
        {
            isAnimating = false;
            timeHandler.StopCoroutine(currentAnimating);
            OnEnd.Invoke();
        }

    }
    private float GetRegularTimeGap()
    {
        float regularTimeGap = 100;


        if (customTimes != null)
        {
            foreach (CustomTime time in customTimes)
            {
                regularTimeGap -= time.timeGapPerCent;
            }
        }

        return regularTimeGap;
    }
    private bool checkUnder99()
    {
        float starter = 99;
        foreach(CustomTime time in customTimes)
        {
            starter -= time.timeGapPerCent;
        }
        if (starter >= 0) return false;
        else return true;
    }
    private bool isSpriteNull()
    {
        if (sprites == null) return true;
        else if (sprites.Length == 0) return true;
        else return false;
    }



    //GET DATA


    //READ ONLY
    public bool IsPlaying { get => isAnimating;}
    public bool IsPaused { get => isPaused; }


    //GETTER AND SETTER
    public Sprite[] Sprites { get => sprites; set => sprites = value; }
    public bool Loop { get => loop; set => loop = value; }
    public float Duration { get => duration; set => duration = value; }
    internal List<CustomTime> CustomTimes { get => customTimes; set => customTimes = value; }



}

//CUSTOM TIME CLASS

[Serializable]
public class CustomTime
{
    public float timeGapPerCent;
    public int spriteIndex;
    [SerializeField]
    public CustomTimeEvent Event;



    public CustomTime(float duration, float spriteLength, int index)
    {
        timeGapPerCent = (duration / spriteLength) * 100;
        if (timeGapPerCent > 99) timeGapPerCent = 99;
        spriteIndex = index;
        Event = new CustomTimeEvent();
    }

    public void newEvent(UnityAction action)
    {
        Event.AddListener(action);
    }



}
[Serializable]
public class CustomTimeEvent : UnityEvent
{

}
class CoroutineHandler : MonoBehaviour
{

}