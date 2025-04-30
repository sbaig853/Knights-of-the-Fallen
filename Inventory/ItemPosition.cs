using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHover : MonoBehaviour
{
    public float hoverSpeed = 1f; // Speed of the hover motion
    public float hoverAmount = 0.5f; // Amplitude of the hover motion

    private Vector3 startPos;
    private Transform player;


    void Start()
    {
        startPos = transform.localPosition; // Initial position of the item
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        Hover();

    }

    private void Hover()
    {
        // Hover the item itself
        Vector3 hoverPosition = startPos;
        hoverPosition.y += Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.localPosition = hoverPosition;
    }
}