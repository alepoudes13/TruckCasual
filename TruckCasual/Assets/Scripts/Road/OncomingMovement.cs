using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OncomingMovement : MonoBehaviour
{
    private float speed = 25;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
