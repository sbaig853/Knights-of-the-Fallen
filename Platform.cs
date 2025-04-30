using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    public Transform posA, posB;
    public float speed;
    Vector3 targetPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = posB.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < 0.05);
        {
            targetPosition = posB.position;
        }

        if (Vector2.Distance(transform.position, posB.position) < 0.05);
        {
            targetPosition = posA.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
