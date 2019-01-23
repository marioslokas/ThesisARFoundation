using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DisplayVelocity : MonoBehaviour {

    [SerializeField]
    private Rigidbody sphereRigidbody;

    [SerializeField]
    private LineRenderer velocityVector;

    private Vector3 velocity, direction;

    private Transform sphereTransform;

    private float minLengthMultiplier = 2f;

    void Start()
    {
        sphereTransform = sphereRigidbody.gameObject.transform;
    }

    // Update is called once per frame
    void Update () {

        velocity = sphereRigidbody.velocity;
//        direction = velocity.normalized * 2;
        
        velocityVector.SetPosition (1, velocity * minLengthMultiplier);
        velocityVector.SetPosition (2, velocity * minLengthMultiplier * 1.3f);
    }
}
