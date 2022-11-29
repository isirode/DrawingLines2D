using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxXDisplayer : MonoBehaviour
{
    public MaxXLimiter maxXLimiter;
    public VerticalSegment verticalSegment;

    public bool overrideYPos = false;

    void Start()
    {
        if (verticalSegment == null)
        {
            verticalSegment = GetComponent<VerticalSegment>();
        }
        Setup();
    }

    void Setup()
    {
        var xPos = maxXLimiter.maxX;

        verticalSegment.xPos = xPos;

        if (overrideYPos)
        {
            var yPos = this.transform.position.y;
            verticalSegment.yPosCenter = yPos;
        }

        verticalSegment.Setup();
    }
}
