using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    public float rotationSpeed = 30f;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
