using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OneHandTargetingManager : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    
    [Header("Projectile references")]
    [SerializeField] private Transform movableObject;
    [SerializeField] private LineRenderer directionRenderer;


    private Vector2 startingTouchPosition;
    [SerializeField] private float touchDelta = 20f;
    private bool adjustingSet = false;
    private bool adjustingDirection = false;
    private bool adjustingMagnitude = false;
    
    // force values for the object
    private Vector2 xForceToObject = Vector2.one;
    
    private Vector3 movableObjectStartingPosition;
    private Quaternion lastAdjustedRotation;

    public float pushForce = 1000f;

    void Start()
    {
        movableObjectStartingPosition = movableObject.position;
    }

    void Update()
    {

        if (Input.touchCount <= 0) return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase.Equals(TouchPhase.Began))
            {
                // store the first finger position
                startingTouchPosition = touch.position;
            }
            else if (touch.phase.Equals((TouchPhase.Moved)))
            {

                Vector2 touchDifferenceVector = touch.position - startingTouchPosition;
                
                if (adjustingMagnitude)
                {
                    
//                    Vector2 xDifference = new Vector2( adjustableValue / Screen.width, 0);
//                    AdjustLineRendererPosition(directionRenderer, xDifference, Vector2.right, out xForceToObject);
                }
                else if (adjustingDirection)
                {
                    float adjustableValue = Vector3.Cross(mainCamera.transform.forward, movableObject.right).y > 0
                        ? touchDifferenceVector.x
                        : -touchDifferenceVector.x;
                    
                    
                }
                
//                float adjustableValueX = Vector3.Cross(mainCamera.transform.forward, movableObject.right).y > 0
//                    ? touch.position.x - previousTouchPosition.x
//                    : previousTouchPosition.x - touch.position.x;
//                
//                Vector2 xDifference = new Vector2( adjustableValueX / xForceImageArea.sizeDelta.x, 0);
//                AdjustLineRendererPosition(xForceRenderer, xDifference, Vector2.right, out xForceToObject);
                
                if (touchDifferenceVector.magnitude > touchDelta && !adjustingSet)
                {
                    if (touchDifferenceVector.x > touchDifferenceVector.y)
                    {
                        adjustingDirection = true;
                        adjustingMagnitude = false;
                    }
                    else
                    {
                        adjustingDirection = false;
                        adjustingMagnitude = true;
                    }

                    adjustingSet = false;
                }
                
                startingTouchPosition = touch.position;
            }
            else if (touch.phase.Equals(TouchPhase.Ended))
            {
                adjustingSet = false;
                adjustingMagnitude = false;
                adjustingDirection = false;
            }

        }
//        else if (Input.touchCount == 2)
//        {
//
//            Quaternion desiredRotation = movableObject.rotation;
//            DetectTouchMovement.Calculate();
//
//            if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
//            {
//                // rotate
//                Vector3 rotationDeg = Vector3.zero;
//                rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
//                desiredRotation *= Quaternion.Euler(rotationDeg);
//            }
//            
//            movableObject.Rotate(Vector3.up, DetectTouchMovement.turnAngleDelta);
//            lastAdjustedRotation = movableObject.rotation;
//        }
        
    }


    public void FireObject()
    {
        movableObject.GetComponent<Rigidbody>().AddForce( (movableObject.rotation *  xForceToObject) * pushForce);
    }

    public void Restart()
    {
        movableObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        movableObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        movableObject.rotation = lastAdjustedRotation;
        
        movableObject.position = movableObjectStartingPosition;
    }

    private void AdjustLineRendererPosition(LineRenderer lineRenderer, Vector2 difference, Vector2 direction, out Vector2 forceValue)
    {
        Vector2 rendererPos1 = new Vector2(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y);
        
        lineRenderer.SetPosition(1, difference + rendererPos1);
        lineRenderer.SetPosition(2, direction + new Vector2(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y) );

        forceValue = difference + rendererPos1;
    }

}

