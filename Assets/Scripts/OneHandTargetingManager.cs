using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class OneHandTargetingManager : MonoBehaviour
{

    private Transform movableObject;
    private LineRenderer directionRenderer;
    private Rigidbody _movableObjectRigidbody;


    private Vector2 startingTouchPosition;
    [SerializeField] private float touchDelta = 20f;
    private bool adjustingSet = false;
    private bool adjustingDirection = false;
    private bool adjustingMagnitude = false;
    
    // force values for the object
    private Vector2 _forceToObject = Vector2.one;
    
    // Variables used when restarting
    private Vector3 movableObjectStartingPosition;
    private Quaternion lastAdjustedRotation;

    public float pushForce = 1000f;

    [Header("UI references")] 
    [SerializeField] private GameObject _fireButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private Text forceValueText;

    void Start()
    {
        _movableObjectRigidbody = movableObject.GetComponent<Rigidbody>();
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
                    forceValueText.text = "Force: " + (_forceToObject.x * pushForce).ToString("F1") + " N";
                }
                else if (adjustingDirection)
                {
                    movableObject.Rotate(Vector3.up, touchDifferenceVector.x / 2);
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

    public void Initialize(Vector3 centralGamePosition, Transform projectile, LineRenderer forceLineRenderer )
    {
        movableObjectStartingPosition = centralGamePosition;
        movableObject = projectile;
        directionRenderer = forceLineRenderer;
        _movableObjectRigidbody = projectile.GetComponent<Rigidbody>();
        
        InitializeUI();
    }

    public void FireObject()
    {
        movableObject.GetComponent<Rigidbody>().AddForce( (movableObject.rotation *  _forceToObject) * pushForce, ForceMode.Force);
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
        movableObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        movableObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        movableObject.rotation = lastAdjustedRotation;
        
        movableObject.position = movableObjectStartingPosition;
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

