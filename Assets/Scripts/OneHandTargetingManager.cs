using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class OneHandTargetingManager : MonoBehaviour
{
    //TODO: Instead of having multiple if checks, make the main game object a list
    // required references for a movable object
    private Transform movableObject;
    private LineRenderer directionRenderer;
    private Rigidbody _movableObjectRigidbody;
    // Variables used when restarting
    private Vector3 movableObjectStartingPosition;
    private Quaternion lastAdjustedRotation;

    // secondary object
    private Transform _secondaryMovableObject;
    private Rigidbody _secondaryObjectRigidbody;
    private Vector3 _secondaryObjectStartingPosition;
    private LineRenderer _secondaryObjectLineRenderer;

    private Vector2 startingTouchPosition;
    [SerializeField] private float touchDelta = 20f;
    private bool adjustingSet = false;
    private bool adjustingDirection = false;
    private bool adjustingMagnitude = false;
    
    // force values for the object
    private Vector2 _forceToObject = Vector2.one;

    public float pushForce = 1000f;

    [Header("UI references")] 
    [SerializeField] private GameObject _fireButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private Text forceValueText;

    void Start()
    {
        _movableObjectRigidbody = movableObject.GetComponent<Rigidbody>();
        
        if(_secondaryMovableObject) _secondaryObjectRigidbody = _secondaryMovableObject.GetComponent<Rigidbody>();
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
                    forceValueText.gameObject.SetActive(true);
                    // Y value is passed as X, to change magnitude
                    float adjustableValue =  2 * touchDifferenceVector.y / Screen.height;
                    Vector2 yDifference = new Vector2( adjustableValue, 0);
                    
                    AdjustLineRendererPosition(directionRenderer, yDifference, Vector2.right, out _forceToObject);

                    if (_secondaryMovableObject) AdjustLineRendererPosition(_secondaryObjectLineRenderer, yDifference, Vector2.right, out _forceToObject);
                    
                    
                    forceValueText.text = "Force: " + (_forceToObject.x * pushForce).ToString("F1") + " N";
                    
                }
                else if (adjustingDirection)
                {
                    movableObject.Rotate(Vector3.up, touchDifferenceVector.x / 2);

                    if (_secondaryMovableObject) _secondaryMovableObject.Rotate(Vector3.up, touchDifferenceVector.x / 2);

                    lastAdjustedRotation = movableObject.rotation;
                }

                
                if (touchDifferenceVector.magnitude > touchDelta && !adjustingSet)
                {
                    if (Mathf.Abs(touchDifferenceVector.x) > Mathf.Abs(touchDifferenceVector.y))
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
                forceValueText.gameObject.SetActive(false);
            }
        }
    }

    public void Initialize(Vector3 centralGamePosition, Transform projectile,
        LineRenderer forceLineRenderer,
        Transform secondaryProjectile = null,
        LineRenderer secondaryLineRenderer = null)
    {
        movableObjectStartingPosition = centralGamePosition;
        movableObject = projectile;
        directionRenderer = forceLineRenderer;
        _movableObjectRigidbody = projectile.GetComponent<Rigidbody>();

        if (secondaryProjectile && secondaryLineRenderer)
        {
            _secondaryMovableObject = secondaryProjectile;
            _secondaryObjectRigidbody = secondaryProjectile.GetComponent<Rigidbody>();
            _secondaryObjectStartingPosition = secondaryProjectile.position;
            _secondaryObjectLineRenderer = secondaryLineRenderer;
        }
        
        InitializeUI();
    }

    public void FireObject()
    {
        _movableObjectRigidbody.AddForce( (movableObject.rotation *  _forceToObject) * pushForce, ForceMode.Force);
        if (_secondaryObjectRigidbody) _secondaryObjectRigidbody.AddForce( (movableObject.rotation *  _forceToObject) * pushForce, ForceMode.Force);

    }

    // switch fire and restart buttons around
    public void ToggleFireUI()
    {
        _fireButton.SetActive(!_fireButton.activeSelf);
        _restartButton.SetActive(!_restartButton.activeSelf);
    }

    // for reseting the UI in between challenges
    private void InitializeUI()
    {
        _fireButton.SetActive(true);
        _restartButton.SetActive(false);
    }

    public void Restart()
    {
        _movableObjectRigidbody.velocity = Vector3.zero;
        _movableObjectRigidbody.angularVelocity = Vector3.zero;
        movableObject.rotation = lastAdjustedRotation;
        
        movableObject.position = movableObjectStartingPosition;

        if (_secondaryMovableObject)
        {
            _secondaryObjectRigidbody.velocity = Vector3.zero;
            _secondaryObjectRigidbody.angularVelocity = Vector3.zero;
            _secondaryMovableObject.rotation = lastAdjustedRotation;
        
            _secondaryMovableObject.position = _secondaryObjectStartingPosition; 
        }
    }

    private void AdjustLineRendererPosition(LineRenderer lineRenderer, Vector2 difference, Vector2 direction, out Vector2 forceValue)
    {
        Vector2 rendererPos1 = new Vector2(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y);
        
        // clamp values
        Vector2 pos1 = difference + rendererPos1;
        Vector2 pos2 = direction + new Vector2(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y);
      
        // the arrow heads are adjusted based on F = m * a, a = F/m
        pos1 = new Vector2( Mathf.Clamp(pos1.x, 1f, float.MaxValue), pos1.x) / new Vector2(_movableObjectRigidbody.mass, _movableObjectRigidbody.mass);
        pos2 = new Vector2( Mathf.Clamp(pos2.x, 2f, float.MaxValue), pos2.x) / new Vector2(_movableObjectRigidbody.mass, _movableObjectRigidbody.mass);
        
        lineRenderer.SetPosition(1, pos1);
        lineRenderer.SetPosition(2, pos2);

        forceValue = difference + rendererPos1;
    }

}

