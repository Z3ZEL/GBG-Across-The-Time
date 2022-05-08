using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthEffect : MonoBehaviour
{
    [SerializeField]
    float a,b;
    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;
        float size = (a * x + b )* Mathf.Sign(transform.localScale.x);
        transform.localScale = new Vector3(size, Mathf.Abs(size),Mathf.Abs(size));
    }
}
