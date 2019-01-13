using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class PrepareGame : MonoBehaviour
{

    [SerializeField] private Text informationText;

    [SerializeField] private string lookingForPlanesMsg;
    [SerializeField] private string planeFoundMsg;

    [SerializeField] private ARPlaneManager _planeManager;
    
    [SerializeField] private Button startButton;
    
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

    public void TogglePlaneDetection()
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
    
    void SetAllPlanesActive(bool value)
    {
        _planeManager.GetAllPlanes(s_Planes);
        foreach (var plane in s_Planes)
            plane.gameObject.SetActive(value);
    }
    
    static List<ARPlane> s_Planes = new List<ARPlane>();
}
