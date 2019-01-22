using System.Collections;
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
    
    [SerializeField] private Text _gravityValueFirstPlanetText;
    [SerializeField] private Text _gravityValueSecondPlanetText;
    [SerializeField] private Text _debugText;

    [Range(5f,15f)]
    [SerializeField] private float distanceLimitsPercentageMin;
    [Range(15f,50f)]
    [SerializeField] private float distanceLimitsPercentageMax;

    private Vector3 firstPlanetMaxPosition, firstPlanetMinPosition;
    private Vector3 secondPlanetMaxPosition, secondPlanetMinPosition;

    private float adjustPositionValue = 0f, adjustPositionValueNormalized;
    
    void Start()
    {
        
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
                
                adjustPositionValue += touchDifferenceVector.y;
                adjustPositionValue = Mathf.Clamp(adjustPositionValue, 0f, Screen.height);
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

    public void Initialize(Vector3[] centralGamePositions, Transform[] objectTransforms, LineRenderer[] forceLineRenderers,
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
