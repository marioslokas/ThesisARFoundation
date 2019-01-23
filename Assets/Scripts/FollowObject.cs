using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    [SerializeField]
    private Transform TransformToFollow;

    private Transform myTransform;

    private Vector3 initialDisplacement;

    void Start()
    {
        myTransform = this.transform;
        initialDisplacement = new Vector3 (myTransform.position.x - TransformToFollow.position.x, 
            myTransform.position.y - TransformToFollow.position.y, 
            myTransform.position.z - TransformToFollow.position.z);
    }

    // Update is called once per frame
    void Update () {
        myTransform.position = TransformToFollow.position + initialDisplacement;
    }
}
