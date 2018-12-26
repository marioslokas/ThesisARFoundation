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
    [SerializeField] private GameObject xForceImageArea;
    [SerializeField] private GameObject yForceImageArea;

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
                if (IsTouchingUiImage(xForceImageArea, worldPoint))
                {
                    adjustingXForce = true;
                }
                else if (IsTouchingUiImage(yForceImageArea, worldPoint))
                {
                    adjustingYForce = true;
                }

                // store the first finger position
                initialTouchPosition = touch.position;
            }
            
            if (adjustingXForce)
            {
                // divide by the screen width
                float xDifference = Mathf.Abs(initialTouchPosition.x - touch.position.x) / Screen.width;
                AdjustLineRendererPosition(xForceRenderer, xDifference);
            }
            else if (adjustingYForce)
            {
                float yDifference = Mathf.Clamp((initialTouchPosition.y - touch.position.y) / Screen.height, 0f, 1f);
                AdjustLineRendererPosition(yForceRenderer, yDifference);
            }
           
        }

        
        
        
        
    }

    private void AdjustLineRendererPosition(LineRenderer lineRenderer, float difference)
    {
        
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
