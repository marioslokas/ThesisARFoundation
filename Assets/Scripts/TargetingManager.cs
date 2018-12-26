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

    private Vector2 initialTouchPosition;
    

    public override void MyStart()
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
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint( touch.position );
            
                // check which panel is being touched
                if (IsTouchingUiImage(xForceImageArea.gameObject, worldPoint))
                {
                    adjustingXForce = true;
                }
                else if (IsTouchingUiImage(yForceImageArea.gameObject, worldPoint))
                {
                    adjustingYForce = true;
                }

                // store the first finger position
                initialTouchPosition = touch.position;
            }
            else if (touch.phase.Equals((TouchPhase.Moved)))
            {
                if (adjustingXForce)
                {
                    // divide by the screen width
                    Vector2 xDifference = new Vector2(Mathf.Abs(initialTouchPosition.x - touch.position.x) / xForceImageArea.sizeDelta.x, 0);
                    AdjustLineRendererPosition(xForceRenderer, xDifference, Vector2.right);
                }
                else if (adjustingYForce)
                {
                    Vector2 yDifference = new Vector2(0,Mathf.Clamp((initialTouchPosition.y - touch.position.y) / yForceImageArea.sizeDelta.y, 0f, 1f));
                    AdjustLineRendererPosition(yForceRenderer, yDifference, Vector2.up);
                }
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
            
            if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0) { // rotate
                Vector3 rotationDeg = Vector3.zero;
                rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
                desiredRotation *= Quaternion.Euler(rotationDeg);
            }
            
            transform.rotation = desiredRotation;
        }
 
        
    }

    private void AdjustLineRendererPosition(LineRenderer lineRenderer, Vector2 difference, Vector2 direction)
    {
        lineRenderer.SetPosition(1, difference);
        lineRenderer.SetPosition(2, difference + direction);
    }

    private bool IsTouchingUiImage(GameObject uiImage, Vector2 screenPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(eventDataCurrentPosition, results);
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.GetInstanceID().Equals(uiImage.GetInstanceID()))
            {
                return true;
            }
        }

        return false;
    }
}
