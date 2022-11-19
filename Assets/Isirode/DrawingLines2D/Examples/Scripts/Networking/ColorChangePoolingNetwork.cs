using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Duplicated class of <see cref="ColorChangePooling"/> but use <see cref="NetworkController"/> instead of <see cref="BasicLineController"/>
/// </summary>
public class ColorChangePoolingNetwork : MonoBehaviour
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

    public NetworkController controller;

    // Start is called before the first frame update
    void Start()
    {
        if (controller == null)
        {
            // attempt to load it from the current game object
            controller = GetComponent<NetworkController>();
        }
        if (controller != null)
        {
            controller.ClientLineAdded += ChangeColor;
        }
        else
        {
            Debug.LogWarning($"{nameof(controller)} is null.");
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

        Debug.Log($"Changing the color to {nextColor}");
        controller.lineColor = nextColor;

        colorQueue.Enqueue(nextColor);
    }
}
