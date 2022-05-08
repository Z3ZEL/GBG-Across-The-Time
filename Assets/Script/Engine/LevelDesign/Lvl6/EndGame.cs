using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kovnir.FastTweener;

public class EndGame : MonoBehaviour
{

    [SerializeField]
    private GameObject Txt;

    [SerializeField]
    float duration = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Level End");
            TriggerWin();


        }
    }

    void TriggerWin(){
        Progression.currentLvl++;

        Camera.main.GetComponent<Animator>().enabled = true;
        Camera.main.GetComponent<Animator>().SetTrigger("end");

        GameObject blackScreen = GameObject.FindWithTag("Fading");
        SpriteRenderer sr = blackScreen.GetComponent<SpriteRenderer>();
        
        FastTweener.Float(0,1,duration,(value) => {
            if(sr != null){
                sr.color = new Color(0,0,0,value);
            }

        },Ease.InOutExpo,false,new System.Action(() => {
            
            Txt.GetComponent<TxtDisplayer>().Play();


        }));

    }


    public void QUIT(){
        Application.Quit();
    }

}
