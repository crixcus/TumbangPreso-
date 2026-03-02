using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAim : MonoBehaviour
{
    [SerializeField] private float maxAngle = 45f;  
    [SerializeField] private float rockSpeed = 2f;

    void Update()
    {
        
        float sineValue = Mathf.Sin(Time.time * rockSpeed);

        float zRotation = sineValue * maxAngle;

        transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }
}
