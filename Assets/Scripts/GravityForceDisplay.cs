using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityForceDisplay : MonoBehaviour
{

    [SerializeField] private LineRenderer _gravityForceRenderer;
    [SerializeField] private Transform _otherObject;

    private Vector3 _directionToOtherObject;
    private Vector3 _normalizedDirection;
    private float distance;
    
    private Transform _thisTransform;

    private float x, y, z;
    
    // Start is called before the first frame update
    void Start()
    {
        _thisTransform = this.GetComponent<Transform>();
        
        _directionToOtherObject = _thisTransform.position - _otherObject.position;
        distance = _directionToOtherObject.magnitude;
        _normalizedDirection = _directionToOtherObject / distance;
    }

    // Update is called once per frame
    void Update()
    {
        x = _normalizedDirection.x != 0f ? 1 : 0;
        y = _normalizedDirection.y != 0f ? 1 : 0;
        z = _normalizedDirection.z != 0f ? 1 : 0;
        
        _directionToOtherObject = _thisTransform.position - _otherObject.position;
        distance = _directionToOtherObject.magnitude;
        _normalizedDirection = _directionToOtherObject / distance;
        
        _gravityForceRenderer.SetPosition(1, _normalizedDirection);
        _gravityForceRenderer.SetPosition(2, _normalizedDirection * 1.5f);
    }
}
