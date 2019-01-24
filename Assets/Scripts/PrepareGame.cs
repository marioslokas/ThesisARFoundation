using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Profiling;

public class PrepareGame : MonoBehaviour
{

    [SerializeField] private Text informationText;

    [SerializeField] private string lookingForPlanesMsg;
    [SerializeField] private string planeFoundMsg;

    [SerializeField] private ARPlaneManager _planeManager;
    
    [SerializeField] private Button startButton;

    [SerializeField] private EyeRaycaster _eyeRaycaster;
    [SerializeField] private ChallengeManager _challengeManager;
    [SerializeField] private GameObject _prepareGameCanvas;

    [SerializeField] private string _helpMessage;
    [SerializeField] private Text _additionalhelpText;
    private bool surfaceFound;
    public float waitingTime = 8f;
    
    // Start is called before the first frame update
    void Start()
    {
        _planeManager.planeAdded += OnPlaneAdded;
        informationText.text = lookingForPlanesMsg;
        surfaceFound = false;
        StartCoroutine(DelayedHelpMessage());
    }

    void Update()
    {
        #if UNITY_EDITOR
        
        if (Input.touchCount > 3)
        {
            startButton.gameObject.SetActive(true);
            _eyeRaycaster.gameObject.SetActive(false);
        }
        
        #endif
    }

    IEnumerator DelayedHelpMessage()
    {
        float t = 0f;

        while (!surfaceFound)
        {
            t += Time.deltaTime;
            if (t >= waitingTime)
            {
                _additionalhelpText.gameObject.SetActive(true);
                _additionalhelpText.text = _helpMessage;
            }

            yield return null;
        }
        
        _additionalhelpText.gameObject.SetActive(false);
        
    }

    void OnPlaneAdded(ARPlaneAddedEventArgs eventArgs)
    {
        informationText.text = planeFoundMsg;
        startButton.gameObject.SetActive(true);
        surfaceFound = true;
    }

    private void OnDisable()
    {
        _planeManager.planeAdded -= OnPlaneAdded;
    }

    private void TogglePlaneDetection()
    {
        _planeManager.enabled = !_planeManager.enabled;

        if (_planeManager.enabled)
        {
            SetAllPlanesActive(true);
        }
        else
        {
            SetAllPlanesActive(false);
        }

    }
    
    public void StartGame()
    {
        _prepareGameCanvas.SetActive(false);
        _challengeManager.centralGamePosition = _eyeRaycaster.planeRaycastPoint;
        _challengeManager.NextChallenge();
        
        TogglePlaneDetection();
    }
    
    void SetAllPlanesActive(bool value)
    {
        _planeManager.GetAllPlanes(s_Planes);
        foreach (var plane in s_Planes)
            plane.gameObject.SetActive(value);
    }
    
    static List<ARPlane> s_Planes = new List<ARPlane>();
}
