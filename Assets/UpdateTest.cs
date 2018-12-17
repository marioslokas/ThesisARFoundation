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
        Debug.Log("MyUpdate");
    }

}
