using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Kovnir.FastTweener;

public class IntroEvent : MonoBehaviour
{
    AsyncOperation op;
    bool ready = false;
    public void PRELOAD(){
        StartCoroutine(LoadYourAsyncScene());
    }

    public void LOAD(){
        FastTweener.Float(AudioController.instance.audioSource.volume,0,1,(value) => {
            AudioController.instance.audioSource.volume = value;
        },Ease.Linear,false,new System.Action(() => {
            ready = true;   
        }));
    }
    IEnumerator LoadYourAsyncScene(){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone && !ready)
        {
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}
