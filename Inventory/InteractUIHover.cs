using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUIHover : MonoBehaviour
{
    public float hoverSpeed = 1f; // Speed of the hover motion
    public float hoverAmount = 0.5f; // Amplitude of the hover motion

    private Vector3 startPos; // Initial position of the text

    void Start()
    {
        //startPos = transform.localPosition; // Get the initial position
    }

    void Update()
    {
        Hover();
    }

    private void Hover()
    {
        // Hover the text itself by applying a sine wave to the y-axis
        Vector3 hoverPosition = startPos;
        hoverPosition.y += Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.localPosition = hoverPosition;
    }
}
