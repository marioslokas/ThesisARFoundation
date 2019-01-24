using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class OneHandTargetingManager : MonoBehaviour, ITransformHandler
{
    // required references for a movable object
    private Transform[] movableObjects;
    private GameObject[] directionRendererGameObjects;
    private LineRenderer[] directionRenderers;
    private Rigidbody[] _movableObjectRigidbodies;
    // Variables used when restarting
    private Vector3[] movableObjectStartingPositions;
    private Quaternion lastAdjustedRotation;

    private Vector2 startingTouchPosition;
    [SerializeField] private float touchDelta = 20f;
    private bool adjustingSet = false;
    private bool adjustingDirection = false;
    private bool adjustingMagnitude = false;

    private bool messageOnFire = false;
    
    // force values for the object
    private Vector2[] _forceToObjects;

    public float pushForce = 1000f;

    [Header("UI references")] 
    [SerializeField] private GameObject _fireButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private Text _forceValueText;
    [SerializeField] private UIController _uiController;

    [SerializeField] private RectTransform _arrowPointer;
    [SerializeField] private Camera mainCamera;

    void Start()
    {
//        for (int i = 0; i < _movableObjectRigidbodies.Length; i++)
//        {
//            _movableObjectRigidbodies[i] = movableObjects[i].GetComponent<Rigidbody>();
//        }
        
    }

    private void CalculateGuidingArrowPosition()
    {

        
        
        Vector3 targetPosition = Vector3.zero;
        if (movableObjects.Length == 1)
        {
            targetPosition = movableObjects[0].position;
        }
        else
        {
            for (int i = 0; i < movableObjects.Length; i++)
            {
                targetPosition += movableObjects[i].position;
            }

            targetPosition /= movableObjects.Length;

        }
        
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

    void Update()
    {
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
                
                if (adjustingMagnitude)
                {
                    _forceValueText.gameObject.SetActive(true);
                    // Y value is passed as X, to change magnitude
                    float adjustableValue =  2 * touchDifferenceVector.y / Screen.height;
                    Vector2 yDifference = new Vector2( adjustableValue, 0);

                    for (int i = 0; i < directionRenderers.Length; i++)
                    {
                        AdjustLineRendererPosition(directionRenderers[i], _movableObjectRigidbodies[i], yDifference, Vector2.right, out _forceToObjects[i]);
                    }
                    

//                    if (_secondaryMovableObject) AdjustLineRendererPosition(_secondaryObjectLineRenderer, yDifference, Vector2.right, out _forceToObject);
                    
                    
                    _forceValueText.text = "Force: " + (_forceToObjects[0].x * pushForce).ToString("F1") + " N";
                    
                }
                else if (adjustingDirection)
                {
                    for (int i = 0; i < movableObjects.Length; i++)
                    {
                        movableObjects[i].Rotate(Vector3.up, touchDifferenceVector.x / 2); 
                    }

                    lastAdjustedRotation = movableObjects[0].rotation;
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
                _forceValueText.gameObject.SetActive(false);
            }
        }
        
        
    }

    public void Initialize(Vector3[] centralGamePositions, 
        Transform[] objectTransforms,
        GameObject[] lineRendererGameObjects,
        LineRenderer[] forceLineRenderers,
        Rigidbody[] projectileRigidbodies,
        bool messageOnFire
        )
    {
        movableObjectStartingPositions = centralGamePositions;
        movableObjects = objectTransforms;
        directionRendererGameObjects = lineRendererGameObjects;
        directionRenderers = forceLineRenderers;

        _movableObjectRigidbodies = projectileRigidbodies;

        _forceToObjects = new Vector2[forceLineRenderers.Length];

        for (int i = 0; i < _forceToObjects.Length; i++)
        {
            // only way to find out if the renderer is pointing diagonally or straight
            if (directionRenderers[i].GetPosition(1).y > 0f)
            {
                _forceToObjects[i] = Vector2.one;
            }
            else
            {
                _forceToObjects[i] = new Vector2(1f, 0f);
            }
        }
        
        this.messageOnFire = messageOnFire;
        
        InitializeUI();
    }

    public void FireObject()
    {
        for (int i = 0; i < _movableObjectRigidbodies.Length; i++)
        {
            _movableObjectRigidbodies[i].AddForce( (movableObjects[0].rotation *  _forceToObjects[i]) * pushForce, ForceMode.Force); 
        }

        if (messageOnFire)
        {
            _uiController.ShowNextMessage();
        }
        
        // disable force renderer while the object travels
        for (int i = 0; i < directionRendererGameObjects.Length; i++)
        {
            directionRendererGameObjects[i].SetActive(false);
        }
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
        for (int i = 0; i < movableObjects.Length; i++)
        {
            _movableObjectRigidbodies[i].velocity = Vector3.zero;
            _movableObjectRigidbodies[i].angularVelocity = Vector3.zero;
            movableObjects[i].rotation = lastAdjustedRotation;
        
            movableObjects[i].position = movableObjectStartingPositions[i]; 
        }
        
        // re-enable line renderers
        for (int i = 0; i < directionRendererGameObjects.Length; i++)
        {
            directionRendererGameObjects[i].SetActive(true);
        }

    }

    private void AdjustLineRendererPosition(LineRenderer lineRenderer, Rigidbody myRigidbody, Vector2 difference, Vector2 direction, out Vector2 forceValue)
    {
        Vector2 rendererPos1 = new Vector2(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y);
        
        Vector2 pos1 = difference + rendererPos1;
        Vector2 pos2 = direction + new Vector2(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y);


        // the renderer is flat
        if (rendererPos1.y == 0f)
        {
            pos1 = new Vector2( Mathf.Clamp(pos1.x, 1f, float.MaxValue), pos1.y);
            pos2 = new Vector2(Mathf.Clamp(pos2.x, 2f, float.MaxValue), pos2.y);
        }
        else
        {
            // the arrow heads are adjusted based on F = m * a, a = F/m
            pos1 = new Vector2( Mathf.Clamp(pos1.x, 1f, float.MaxValue), pos1.x);
            pos2 = new Vector2( Mathf.Clamp(pos2.x, 2f, float.MaxValue), pos2.x);
        }
       
        
        lineRenderer.SetPosition(1, pos1);
        lineRenderer.SetPosition(2, pos2);

        forceValue = difference + rendererPos1;
    }
}

