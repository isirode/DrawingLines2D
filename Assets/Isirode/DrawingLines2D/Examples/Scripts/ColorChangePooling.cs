using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangePooling : MonoBehaviour
{
    /// <summary>
    /// Increment the counter and pick a Color to add a Color
    /// WARNING : the Color will be black, and the Alpha channel will be transparent
    /// So, you might not see it if you are not careful
    /// </summary>
    public List<Color> colors;
    // Info : we are using a queue since it is easier to use for a changing pool
    // We are also using a list because there is no editors for the C# Queue in Unity
    private Queue<Color> colorQueue = new Queue<Color>();
    public int colorIndex;

    public LegacyInputController legacyInputController;

    // Start is called before the first frame update
    void Start()
    {
        if (legacyInputController == null)
        {
            // attempt to load it from the current game object
            legacyInputController = GetComponent<LegacyInputController>();
        }
        if (legacyInputController != null)
        {
            legacyInputController.LineAdded += ChangeColor;
        }
        else
        {
            Debug.LogWarning($"{nameof(legacyInputController)} is null.");
        }

        if (colors.Count != 0)
        {
            colors.ForEach(color => colorQueue.Enqueue(color));
        }
    }

    private void ChangeColor(List<Vector3> points, GameObject gameObject)
    {
        if (colorQueue.Count == 0)
        {
            Debug.LogWarning("Your color list is empty.");
            return;
        }

        // FIXME : it is probably less optimize to do it this instead of changing an index
        var nextColor = colorQueue.Dequeue();

        legacyInputController.lineColor = nextColor;

        colorQueue.Enqueue(nextColor);
    }
}
