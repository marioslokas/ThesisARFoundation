using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityForceDisplay : MonoBehaviour
{

    [SerializeField] private LineRenderer _gravityForceRenderer;
    [SerializeField] private Transform _otherObject;

    public Vector3 DirectionToOtherObject { get; private set; }

    public Vector3 NormalizedDirection
    {
        get { return _normalizedDirection; }
        private set { _normalizedDirection = value; }
    }

    private Vector3 _normalizedDirection;
    private Vector3 _normalizedDirectionUnit;
    private float distance;
    
    private Transform _thisTransform;

    private float x, y, z;
    private float _otherObjectMass;

    private Vector3 _minValue;
    public float gravitationalPull = 0f;

    void Start()
    {
        _thisTransform = this.gameObject.GetComponent<Transform>();
        _otherObjectMass = _otherObject.GetComponent<Rigidbody>().mass;
    }

    public void Initialize()
    {
        _thisTransform = this.gameObject.GetComponent<Transform>();
        _otherObjectMass = _otherObject.GetComponent<Rigidbody>().mass;
    }

    public void UpdateForce()
    {
        
        DirectionToOtherObject = _otherObject.position - _thisTransform.position;
        distance = DirectionToOtherObject.magnitude;
        _normalizedDirection = DirectionToOtherObject / distance;

        gravitationalPull = (_otherObjectMass / Mathf.Pow(distance, 2) / 1f);
        
//        Debug.DrawRay(this.transform.position, DirectionToOtherObject, Color.red);

        _minValue = _normalizedDirection * 3f;
        
        _gravityForceRenderer.SetPosition(1, _normalizedDirection * gravitationalPull);
        _gravityForceRenderer.SetPosition(2, _normalizedDirection * gravitationalPull  + _normalizedDirection * 2f);
    }
}
