using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * From https://stackoverflow.com/questions/13708395/how-can-i-draw-a-circle-in-unity3d answer https://stackoverflow.com/a/70672415
 * Credit to Salvatore Ambulando (https://stackoverflow.com/users/2647106/salvatore-ambulando) and others which contributed in the SO thread
 * 
 * The license is CC BY-SA 4.0 https://creativecommons.org/licenses/by-sa/4.0/
 * 
 */
// TODO : maybe use another way to draw a circle
[RequireComponent(typeof(LineRenderer))]
public class DrawRing : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [Range(6, 60)]   //creates a slider - more than 60 is hard to notice
    public int lineCount;       //more lines = smoother ring
    public float radius;
    public float width;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
        this.gameObject.SetActive(false);

        Draw();
    }

    public void Draw() //Only need to draw when something changes
    {
        lineRenderer.positionCount = lineCount;
        lineRenderer.startWidth = width;
        float theta = (2f * Mathf.PI) / lineCount;  //find radians per segment
        float angle = 0;
        for (int i = 0; i < lineCount; i++)
        {
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            // Info : I switched it
            //switch 0 and y for 2D games
            lineRenderer.SetPosition(i, new Vector3(x, y, 0.5f));

            angle += theta;
        }
    }
}
