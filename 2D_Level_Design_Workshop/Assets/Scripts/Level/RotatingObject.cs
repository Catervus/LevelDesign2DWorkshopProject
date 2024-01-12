using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField]
    Vector3 rotationDirection;

    [SerializeField]
    ScriptableFloat rotationSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(rotationDirection.normalized * rotationSpeed.RuntimeValue * Time.deltaTime);
    }
}
