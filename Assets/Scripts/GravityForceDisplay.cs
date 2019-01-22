using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityForceDisplay : MonoBehaviour
{

    [SerializeField] private LineRenderer _gravityForceRenderer;
    [SerializeField] private Transform _otherObject;

    private Vector3 _directionToOtherObject;

    public Vector3 DirectionToOtherObject
    {
        get { return _directionToOtherObject; }
        private set { _directionToOtherObject = value; }
    }

    private Vector3 _normalizedDirection;
    private Vector3 _normalizedDirectionUnit;
    private float distance;
    
    private Transform _thisTransform;

    private float x, y, z;
    private float _otherObjectMass;

    private Vector3 _minValue;
    public float gravitationalPull = 0f;
    
    // Start is called before the first frame update
    public void Initialize()
    {
        _thisTransform = this.gameObject.GetComponent<Transform>();
        _otherObjectMass = _otherObject.GetComponent<Rigidbody>().mass;
    }

    // Update is called once per frame
    public void UpdateForce()
    {
        _directionToOtherObject = _otherObject.position - _thisTransform.position;
        distance = _directionToOtherObject.magnitude;
        _normalizedDirection = _directionToOtherObject / distance;

        gravitationalPull = (_otherObjectMass / Mathf.Pow(distance, 2) / 1f);

        _minValue = _normalizedDirection * 3f;
        
        _gravityForceRenderer.SetPosition(1, _normalizedDirection * gravitationalPull);
        _gravityForceRenderer.SetPosition(2, _normalizedDirection * gravitationalPull  + _normalizedDirection * 2f);
    }
}
