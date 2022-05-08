using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Kovnir.FastTweener;

public class Fishing : MonoBehaviour
{
    Light2D main;
    [SerializeField]
    float flashSpeed = 2,flashIntensity = 3f,timeLimit=5;
    bool isIn = false,flashing = false,hasfound = false;

    [SerializeField]
    GameObject key;
    void Awake(){
        main = this.GetComponent<Light2D>();
        main.intensity = 0;
        key.SetActive(false);

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Detection" && Input.GetKeyDown(KeyCode.F))
        {
            FlashLight();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isIn = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isIn = false;
        }
    }

    void FlashLight()
    {
        flashing = true;
        //BLINK
        FastTweener.Float(0,flashIntensity, flashSpeed, (value) => { main.intensity = value; }
        ,Ease.InExpo,false,new System.Action(() => {
            FastTweener.Float(flashIntensity,0, flashSpeed, (value) => { main.intensity = value; },Ease.InExpo,false);
            flashing = false;
        }));
    }

    [SerializeField]
    float timer = 0;
    void Update()
    {
        if(hasfound)return;

        if(isIn && !flashing){
            timer+=Time.deltaTime;
            main.intensity = (timer/timeLimit) * flashIntensity;
        }else if (!isIn && !flashing){
            if(timer>0){
            timer -= Time.deltaTime;
            }
            main.intensity = (timer/timeLimit) * flashIntensity;
        }
        if(timer>=timeLimit){
            TriggerKey();
            hasfound = true;
        }

    }

    void TriggerKey(){
        key.SetActive(true);
    }
}
