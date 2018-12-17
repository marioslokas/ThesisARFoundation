using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTest : MyUpdatableBehaviour
{
    // Start is called before the first frame update
    public override void MyStart()
    {
        Debug.Log("MyStart");
    }

    public override void MyUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
        }
    }

}
