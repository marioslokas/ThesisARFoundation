using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTest : MyUpdatableBehaviour
{

    public override void MyUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
        }
    }

}
