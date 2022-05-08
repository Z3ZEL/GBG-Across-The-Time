using System;
using UnityEngine;
using Kovnir.FastTweener;

public class CameraPosition : MonoBehaviour
{

    [Serializable]
    class CamPos{
        public Transform transform;
        public float size;
        public float duration;
    }

    [SerializeField]
    CamPos[] camPositions;

    public void GoTo(int index)
    {
        GetComponent<Animator>().enabled = false;


        float duration = camPositions[index].duration;
        Transform target = camPositions[index].transform;
        float size = camPositions[index].size;
        Camera cam = GetComponent<Camera>();

        //LERP THE CAMERA TO THE POSITION
        FastTweener.Vector3(transform.position,target.position, duration, (value) => {
            if (this == null) return;
            transform.position = value;
        });

        //LERP THE CAMERA SIZE

        FastTweener.Float(cam.orthographicSize, size, duration, (value) => {
            if (this == null) return;
            cam.orthographicSize = value;
        });

    }

    public void TryDefault()
    {
        if (camPositions.Length > 0)
        {
            GoTo(0);
        }
    }

}
