using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinYDisplayer : MonoBehaviour
{
    public MinYLimiter minYLimiter;
    public HorizontalSegment horizontalSegment;

    public bool overrideXPos = false;

    void Start()
    {
        if (horizontalSegment == null)
        {
            horizontalSegment = GetComponent<HorizontalSegment>();
        }
        Setup();
    }

    void Setup()
    {
        var yPos = minYLimiter.minY;

        horizontalSegment.yPos = yPos;

        if (overrideXPos)
        {
            var xPos = this.transform.position.x;
            horizontalSegment.xPosCenter = xPos;
        }

        horizontalSegment.Setup();
    }
}
