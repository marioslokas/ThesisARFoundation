using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class GravityForceDisplay : MonoBehaviour
{

    [SerializeField] private LineRenderer _gravityForceRenderer;
    [SerializeField] private Transform _otherObject;

    private Vector3 _directionToOtherObject;
    private Vector3 _normalizedDirection;
    private Vector3 _normalizedDirectionUnit;
    private float distance;
    
    private Transform _thisTransform;

    private float x, y, z;
    private float _otherObjectMass;

    [SerializeField] private float _minDistance = 2f;
    private Vector3 _minValue;
    private float gravitationalPull = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _thisTransform = this.GetComponent<Transform>();
        _otherObjectMass = _otherObject.GetComponent<Rigidbody>().mass;
    }

    // Update is called once per frame
    void Update()
    {
        _directionToOtherObject = _otherObject.position - _thisTransform.position;
        distance = _directionToOtherObject.magnitude;
        _normalizedDirection = _directionToOtherObject / distance;

        gravitationalPull = (_otherObjectMass / Mathf.Pow(distance, 2) / 10f);

        _minValue = _normalizedDirection * 3f;
        
        _gravityForceRenderer.SetPosition(1, _normalizedDirection * gravitationalPull);
        _gravityForceRenderer.SetPosition(2, _normalizedDirection * gravitationalPull  + _normalizedDirection * 2f);
    }
}
