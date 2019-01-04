using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualGravity : MyUpdatableBehaviour
{
    
    public float earthG = 9.807f;
    public bool gravitySwitch;
    
    // used to actually control pausing gravity by freezing the body
    public bool gravityPaused = false;
    Rigidbody objectBody;
    
    // Start is called before the first frame update
    void Start()
    {
        objectBody = this.GetComponent<Rigidbody> ();
        objectBody.isKinematic = false;
        gravitySwitch = false;
    }

    void FixedUpdate () {

        if (gravitySwitch)
        {
            gravityPaused = !gravityPaused;
            gravitySwitch = false;
            objectBody.isKinematic = !objectBody.isKinematic;
        }

        if (gravityPaused)
        {
            return;
        }
        
        objectBody.AddForce (Vector3.down * earthG);
    }

    public override void MyUpdate()
    {
        
    }
}
