using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatInFrontOfCamera : MonoBehaviour
{
    public Camera myCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = myCamera.transform.position + myCamera.transform.forward;
    }
}
