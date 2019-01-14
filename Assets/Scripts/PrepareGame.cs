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

    [SerializeField] private GameObject sphereAndGround;
    [SerializeField] private GameObject spaceEnvironment;

    [SerializeField] private EyeRaycaster _eyeRaycaster;

    [SerializeField] private UIController uiController;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _planeManager.planeAdded += OnPlaneAdded;
        informationText.text = lookingForPlanesMsg;
    }

    void OnPlaneAdded(ARPlaneAddedEventArgs eventArgs)
    {
        informationText.text = planeFoundMsg;
        startButton.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _planeManager.planeAdded -= OnPlaneAdded;
    }

    private void TogglePlaneDetection()
    {
        _planeManager.enabled = !_planeManager.enabled;

        string planeDetectionMessage = "";
        if (_planeManager.enabled)
        {
            planeDetectionMessage = "Disable Plane Detection and Hide Existing";
            SetAllPlanesActive(true);
        }
        else
        {
            planeDetectionMessage = "Enable Plane Detection and Show Existing";
            SetAllPlanesActive(false);
        }

    }
    
    public void StartGame()
    {

        // set the environment
        sphereAndGround.SetActive(true);
        sphereAndGround.transform.position = _eyeRaycaster.planeRaycastPoint;
        
        spaceEnvironment.SetActive(true);
        
        uiController.StartGameUI();
        
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
