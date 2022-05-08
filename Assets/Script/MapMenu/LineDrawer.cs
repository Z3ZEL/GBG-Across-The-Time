using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{

    [SerializeField]
    Texture lineTex;
    [SerializeField]
    float width = 1,shift=0.05f;
    LineRenderer lRenderer;
    MapNav mapNav;
    // Start is called before the first frame update
    void Start()
    {
        lRenderer = GetComponent<LineRenderer>();
        mapNav = GetComponent<MapNav>();

        lRenderer.endWidth = width;
        lRenderer.startWidth = width;
        
        List<Transform> points = mapNav.getAllPoints();
        lRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 pos = points[i].position + new Vector3(0, shift, 0);
            lRenderer.SetPosition(i, pos);
        }

        lRenderer.alignment = LineAlignment.TransformZ;


    }

    // Update is called once per frame
    void Update()
    {
        


    }
}
