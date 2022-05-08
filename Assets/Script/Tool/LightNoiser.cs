using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightNoiser : MonoBehaviour
{
    [SerializeField]
    float flashSpeed = 2,floorValue = 0.5f;
   Light2D main;

   [SerializeField]
   bool randomFloorValue = false;
   [SerializeField]
   Vector2 floorValueRange = new Vector2(0.5f,1),floorSpeedRange = new Vector2(0.5f,1);
   private void Awake(){
        main = this.GetComponent<Light2D>();
        if(randomFloorValue){
            floorValue = Random.Range(floorValueRange.x,floorValueRange.y);
            flashSpeed = Random.Range(floorSpeedRange.x,floorSpeedRange.y);
        }
   }

    private void Update(){
        //ADD SOME NOISE TO THE LIGHT
        if(!randomFloorValue){
        main.intensity = Mathf.PerlinNoise(Time.time * flashSpeed,0) * floorValue;
        }else{
            main.intensity = Mathf.PerlinNoise(Time.time * flashSpeed,0) * floorValue;
        }
    }

}
