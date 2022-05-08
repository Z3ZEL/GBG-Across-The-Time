using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{

    CharacterControl control;
    void Awake(){
        control = GetComponentInParent<CharacterControl>();
    }

    void JUMP(){
        control.Jump();
    }

}
