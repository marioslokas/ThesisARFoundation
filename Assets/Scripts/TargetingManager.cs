using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetingManager : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    
    [Header("Projectile references")]
    [SerializeField] private Transform movableObject;
    [SerializeField] private LineRenderer xForceRenderer;
    [SerializeField] private LineRenderer yForceRenderer;

    [Header("UI references")]
    [SerializeField] private RectTransform xForceImageArea;
    [SerializeField] private RectTransform yForceImageArea;
    private GraphicRaycaster uiRaycaster;

    private bool adjustingXForce, adjustingYForce;

    private Vector2 previousTouchPosition;

    // force values for the object
    private Vector2 xForceToObject = Vector2.one, yForceToObject = Vector2.one;
    
    private Vector3 movableObjectStartingPosition;
    private Quaternion lastAdjustedRotation;

    public float pushForce = 1000f;

    void Start()
    {
        uiRaycaster = this.GetComponent<GraphicRaycaster>();

        if (!yForceRenderer || !xForceRenderer || !xForceImageArea || !yForceImageArea || !movableObject)
        {
            Debug.LogError("References missing");
        }

        adjustingXForce = false;
        adjustingYForce = false;

        movableObjectStartingPosition = movableObject.position;
    }

    void Update()
    {
        
//        Debug.DrawRay(Vector3.zero, Vector3.Cross(mainCamera.transform.forward, movableObject.right),Color.blue);
        
        
        if (Input.touchCount <= 0) return;

        // calculate which panel is being pressed
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase.Equals(TouchPhase.Began))
            {
                Vector2 worldPoint = mainCamera.ScreenToWorldPoint(touch.position);

                // store the first finger position
                previousTouchPosition = touch.position;
            }
            else if (touch.phase.Equals((TouchPhase.Moved)))
            {
                if (adjustingXForce)
                {
                    // camera's look at direction determines where the finger must move to to adjust the x force

                    float adjustableValueX = Vector3.Cross(mainCamera.transform.forward, movableObject.right).y > 0
                        ? touch.position.x - previousTouchPosition.x
                        : previousTouchPosition.x - touch.position.x;
                    
                    Vector2 xDifference = new Vector2( adjustableValueX / xForceImageArea.sizeDelta.x, 0);
                    AdjustLineRendererPosition(xForceRenderer, xDifference, Vector2.right, out xForceToObject);
                }
                else if (adjustingYForce)
                {
                    Vector2 yDifference = new Vector2(0, (touch.position.y - previousTouchPosition.y) / yForceImageArea.sizeDelta.y);
                    AdjustLineRendererPosition(yForceRenderer, yDifference, Vector2.up, out yForceToObject);
                }

                previousTouchPosition = touch.position;
            }
            else if (touch.phase.Equals((TouchPhase.Ended)))
            {
                adjustingXForce = false;
                adjustingYForce = false;
            }
        }
        else if (Input.touchCount == 2)
        {

            adjustingXForce = false;
            adjustingYForce = false;

            Quaternion desiredRotation = movableObject.rotation;
            DetectTouchMovement.Calculate();

            if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
            {
                // rotate
                Vector3 rotationDeg = Vector3.zero;
                rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
                desiredRotation *= Quaternion.Euler(rotationDeg);
            }
            
            movableObject.Rotate(Vector3.up, DetectTouchMovement.turnAngleDelta);
            lastAdjustedRotation = movableObject.rotation;
        }
        
    }

    public void TouchingXPanel()
    {
        adjustingXForce = true;
    }

    public void EndTouchingXPanel()
    {
        adjustingXForce = false;
    }

    public void TouchingYPanel()
    {
        adjustingYForce = true;
    }

    public void EndTouchingYPanel()
    {
        adjustingYForce = false;
    }

    public void FireObject()
    {
        movableObject.GetComponent<Rigidbody>().AddForce( (movableObject.rotation *  xForceToObject) * pushForce);
        movableObject.GetComponent<Rigidbody>().AddForce( (movableObject.rotation * yForceToObject) * pushForce);
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

