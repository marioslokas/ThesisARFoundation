using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetingManager : MyUpdatableBehaviour
{
    [SerializeField] private LineRenderer xForceRenderer;
    [SerializeField] private LineRenderer yForceRenderer;

    [SerializeField] private Transform rotatingObject;
    [SerializeField] private RectTransform xForceImageArea;
    [SerializeField] private RectTransform yForceImageArea;

    private GraphicRaycaster uiRaycaster;

    private bool adjustingXForce, adjustingYForce;

    private Vector2 previousTouchPosition;


    void Start()
    {
        uiRaycaster = this.GetComponent<GraphicRaycaster>();

        if (!yForceRenderer || !xForceRenderer || !xForceImageArea || !yForceImageArea || !rotatingObject)
        {
            Debug.LogError("References missing");
        }

        adjustingXForce = false;
        adjustingYForce = false;
    }

    public override void MyUpdate()
    {
        if (Input.touchCount <= 0) return;

        // calculate which panel is being pressed
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase.Equals(TouchPhase.Began))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);

                // store the first finger position
                previousTouchPosition = touch.position;
            }
            else if (touch.phase.Equals((TouchPhase.Moved)))
            {
                if (adjustingXForce)
                {
                    // divide by the screen width
                    Vector2 xDifference = new Vector2((touch.position.x - previousTouchPosition.x) / xForceImageArea.sizeDelta.x, 0);
                    AdjustLineRendererPosition(xForceRenderer, xDifference, Vector2.right);
                }
                else if (adjustingYForce)
                {
                    Vector2 yDifference = new Vector2(0, (touch.position.y - previousTouchPosition.y) / yForceImageArea.sizeDelta.y);
                    AdjustLineRendererPosition(yForceRenderer, yDifference, Vector2.up);
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

            Quaternion desiredRotation = rotatingObject.rotation;
            DetectTouchMovement.Calculate();

            if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0)
            {
                // rotate
                Vector3 rotationDeg = Vector3.zero;
                rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
                desiredRotation *= Quaternion.Euler(rotationDeg);
            }
            
            rotatingObject.Rotate(Vector3.up, DetectTouchMovement.turnAngleDelta);
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

    private void AdjustLineRendererPosition(LineRenderer lineRenderer, Vector2 difference, Vector2 direction)
    {
        Vector2 rendererPos1 = new Vector2(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y);
        
        lineRenderer.SetPosition(1, difference + rendererPos1);
        lineRenderer.SetPosition(2, direction + new Vector2(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y) );
        
    }

}

