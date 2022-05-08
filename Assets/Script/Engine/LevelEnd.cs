using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Kovnir.FastTweener;

public class LevelEnd : MonoBehaviour
{
    [SerializeField]
    float duration = 1;

    [SerializeField]
    private Item itemToFind;

    bool hasWin = false;

    void Start(){
        if(itemToFind != null && itemToFind.itemSprite != null){
            GetComponent<SpriteRenderer>().sprite = itemToFind.itemSprite;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Level End");
            AudioController.instance.PlaySound("Victory");
            TriggerWin();


        }
    }

    void TriggerWin(){
        if(hasWin)return;
        hasWin = true;
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
            SceneManager.LoadScene(1);
        }));

    }

}
