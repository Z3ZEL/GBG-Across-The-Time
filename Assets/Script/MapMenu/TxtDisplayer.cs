using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class TxtDisplayer : MonoBehaviour
{
    [Serializable]
    class CustomTxt{
        public string txt;
        public float duration;
        public TxtEvent action = new TxtEvent();
    }
    

    [SerializeField]
    CustomTxt[] customTxts;
    [SerializeField]
    float fadingSpeed=0.8f,delay=2;

    int currentTxt=0;
    [SerializeField]
    bool playOnAwake = true;
    TextMeshProUGUI txtDp;

    private void Awake()
    {
        currentTxt = 0;
        txtDp = GetComponent<TextMeshProUGUI>();
        txtDp.alpha = 0;
        if(playOnAwake){
        StartCoroutine(txtDisplayer());
        }
    }

    public void Play(){
        StartCoroutine(txtDisplayer());
    }

    IEnumerator txtDisplayer(){
        yield return new WaitForSeconds(delay);
        while(currentTxt<customTxts.Length){
            txtDp.text = customTxts[currentTxt].txt;
            if(customTxts[currentTxt].action!=null){
                customTxts[currentTxt].action.Invoke();
            }
            //FADE TXT
            float t = 0;
            while(t<1){
                t+=Time.deltaTime*fadingSpeed;
                txtDp.alpha = t;
                yield return null;
            }
            yield return new WaitForSeconds(customTxts[currentTxt].duration);
            currentTxt++;
        }
    }

    


}
