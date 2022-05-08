using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kovnir.FastTweener;

public class CharacterMenu : MonoBehaviour
{
    [SerializeField]
    float floatingShift = 2,speed;

    private static bool hasBeenInit =   false;
    void Awake()
    {
        // if(!hasBeenInit){
        //     FastTweenerSettings settings = new FastTweenerSettings();
        //     settings.DefaultEase = Ease.OutExpo;
        //     FastTweener.Init(settings);
        //     hasBeenInit = true;
        // }
    }

    // Start is called before the first frame update
    void Start()
    {
        MoveY(floatingShift);
    }
    FastTween tween;
    public void KillProcess(){
        FastTweener.Kill(tween);
    }

    void MoveY(float shift){
       tween = FastTweener.Float(transform.position.y, transform.position.y + shift, speed, (value) => {
            if(this == null) return;
            transform.position = new Vector3(transform.position.x, value, transform.position.z);
        }, Ease.Linear, false,new System.Action(() => {
            MoveY(-shift);
        }));
    }

}
