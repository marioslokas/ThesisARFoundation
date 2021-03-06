﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneHandPositionChangeManager : MonoBehaviour, ITransformHandler
{
    // class used to adjust the position between 2 objects
    private Transform _firstPlanetTransform;
    private Transform _secondPlanetTransform;

    private Vector3 _firstPlanetPosition;
    private Vector3 _secondPlanetPosition;

    private GravityForceDisplay firstPlanetGravityForce;
    private GravityForceDisplay secondPlanetGravityForce;
    
    private Vector2 startingTouchPosition;
    [SerializeField] private float touchDelta = 20f;

    [SerializeField] private Text _debugText;

    [Range(5f,15f)]
    [SerializeField] private float distanceLimitsPercentageMin;
    [Range(15f,50f)]
    [SerializeField] private float distanceLimitsPercentageMax;

    private Vector3 firstPlanetMaxPosition, firstPlanetMinPosition;
    private Vector3 secondPlanetMaxPosition, secondPlanetMinPosition;

    private float adjustPositionValue = 0f, adjustPositionValueNormalized;

    [SerializeField] private UIController _uiController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform _arrowPointer;
    
    private bool pauseUpdating = false;
    
    private void CalculateGuidingArrowPosition()
    {
        Vector3 targetPosition = (_firstPlanetTransform.position);
        
        // is the object visible?
        Vector3 targetViewportPosition = mainCamera.WorldToViewportPoint(targetPosition);

        if (targetViewportPosition.x > 0f 
            && targetViewportPosition.x < 1f 
            && targetViewportPosition.y > 0f && targetViewportPosition.y < 1f && targetViewportPosition.z > 0f)
        {
            _arrowPointer.gameObject.SetActive(false);
        }
        else
        {
            _arrowPointer.gameObject.SetActive(true);
            var targetPosLocal = mainCamera.transform.InverseTransformPoint(targetPosition);
            var targetAngle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.y) * Mathf.Rad2Deg - 90;
            _arrowPointer.eulerAngles = new Vector3(0, 0, targetAngle);
        }
    }
    
    public void PauseUpdating()
    {
        pauseUpdating = true;
    }

    public void ResumeUpdating()
    {
        pauseUpdating = false;
    }

    void Update()
    {
        if (pauseUpdating) return;
        
        CalculateGuidingArrowPosition();
        
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
                
                adjustPositionValue += touchDifferenceVector.y;
                adjustPositionValue = Mathf.Clamp(adjustPositionValue, 0f, Screen.height);

                if (adjustPositionValue==Screen.height)
                {
                    // there won't be any message, just to enable the next challenge canvas
                    _uiController.ShowNextMessage();
                }
                
                adjustPositionValueNormalized = adjustPositionValue / Screen.height;

                _firstPlanetTransform.position = Vector3.Lerp(firstPlanetMinPosition, firstPlanetMaxPosition,
                    adjustPositionValueNormalized);
                
                _secondPlanetTransform.position = Vector3.Lerp(secondPlanetMinPosition, secondPlanetMaxPosition,
                    adjustPositionValueNormalized);
                
                firstPlanetGravityForce.UpdateForce();
                secondPlanetGravityForce.UpdateForce();
                
                startingTouchPosition = touch.position;
//                _debugText.text = Vector3.Distance(_firstPlanetTransform.position, _secondPlanetTransform.position).ToString();
            }
            else if (touch.phase.Equals(TouchPhase.Ended))
            {

            }
        }
    }

    public void Initialize(Vector3[] centralGamePositions, 
        Transform[] objectTransforms, 
        GameObject[] lineRendererGameObjects,
        LineRenderer[] forceLineRenderers,
        Rigidbody[] projectileRigidbodies, bool messageOnFire)
    {
        _firstPlanetTransform = objectTransforms[0];
        _secondPlanetTransform = objectTransforms[1];

        _firstPlanetPosition = centralGamePositions[0];
        _secondPlanetPosition = centralGamePositions[1];

        firstPlanetGravityForce = objectTransforms[0].GetComponent<GravityForceDisplay>();
        secondPlanetGravityForce = objectTransforms[1].GetComponent<GravityForceDisplay>();
        
        firstPlanetGravityForce.Initialize();
        secondPlanetGravityForce.Initialize();
        
        firstPlanetGravityForce.UpdateForce();
        secondPlanetGravityForce.UpdateForce();

        //calculate min-max positions
        firstPlanetMinPosition = _firstPlanetPosition + firstPlanetGravityForce.NormalizedDirection * (100f - distanceLimitsPercentageMin)/100f;
        firstPlanetMaxPosition = _firstPlanetPosition + firstPlanetGravityForce.NormalizedDirection * (100f - distanceLimitsPercentageMax)/100f;
        
        secondPlanetMinPosition = _secondPlanetPosition + secondPlanetGravityForce.NormalizedDirection * (100f - distanceLimitsPercentageMin)/100f;
        secondPlanetMaxPosition = _secondPlanetPosition + secondPlanetGravityForce.NormalizedDirection * (100f - distanceLimitsPercentageMax)/100f;

        _firstPlanetTransform.position = firstPlanetMinPosition;
        _secondPlanetTransform.position = secondPlanetMinPosition;
        
        firstPlanetGravityForce.UpdateForce();
        secondPlanetGravityForce.UpdateForce();
    }
}
