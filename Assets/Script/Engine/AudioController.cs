using System;
using UnityEngine;
using Kovnir.FastTweener;

public class AudioController : MonoBehaviour
{

    public static AudioController instance;

    [Serializable]
    class ClipSong
    {
        public string name;
        public AudioClip clip;
    }
    [SerializeField]
    ClipSong[] clips;

    [SerializeField]
    string defaultSong;
    [SerializeField]
    float transitionSpeed=0.75f;
    [SerializeField,Range(0,1)]
    float volume=1;

    public AudioSource audioSource;

    [SerializeField]
    public AudioSource soundEffectSource;

    public string currentSoundEffect="None";

    private void Awake(){
        audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    private void Start()
    {
        audioSource.clip = GetClip(defaultSong);
        audioSource.volume = 0;
        audioSource.Play();
        TargetVolume(volume,1,Ease.OutExpo);
    
    }

    public bool isPlaying{
        get {return audioSource.isPlaying;}
    }

    public void PlaySound(string clipName)
    {
        //MAKE SMOOTH TRANSITION BETWEEN SOUNDS
        FastTweener.Float(volume, 0, transitionSpeed, (value) => {
            if (this == null) return;
            audioSource.volume = value;
        }, Ease.InOutExpo, false, new System.Action(() => {
            audioSource.clip = GetClip(clipName);
            audioSource.Play();
            FastTweener.Float(0, volume, transitionSpeed, (value) => {
                if (this == null) return;
                audioSource.volume = value;
            }, Ease.Linear, false);
        }));

    }
    public void PlaySoundEffect(string clipName)
    {
        currentSoundEffect = clipName;
        soundEffectSource.volume = volume;
        soundEffectSource.clip = GetClip(clipName);
        soundEffectSource.Play();
    
    }

     public void PlaySoundEffect(string clipName,float _volume)
    {
        currentSoundEffect = clipName;
        soundEffectSource.volume = _volume;
        soundEffectSource.clip = GetClip(clipName);
        soundEffectSource.Play();
    
    }
    public void TargetVolume(float targetVolume, float duration,Ease ease)
    {
        FastTweener.Float(audioSource.volume, targetVolume, duration, (value) => {
            if (this == null) return;
            audioSource.volume = value;
        },ease);
    }
    private AudioClip GetClip(string clipName)
    {
        foreach (ClipSong clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.clip;
            }
        }
        return null;
    }
}
