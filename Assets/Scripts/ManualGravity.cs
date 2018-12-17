using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualGravity : MonoBehaviour
{
    
    public float earthG = 9.807f;
    public bool pauseGravity = false;
    Rigidbody objectBody;
    
    // Start is called before the first frame update
    void Start()
    {
        objectBody = this.GetComponent<Rigidbody> ();
    }

    void FixedUpdate () {
        
        if (pauseGravity) { return; }
        
        objectBody.AddForce (Vector3.down * earthG);
    }
}
