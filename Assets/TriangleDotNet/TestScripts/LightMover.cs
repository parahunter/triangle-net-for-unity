using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightMover : MonoBehaviour 
{
    

    void Update()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = transform.position.z;

        transform.position = worldPos;
    }
}
