using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class AlertPlayer : MonoBehaviour
{

    [SerializeField] private Text informationText;

    [SerializeField] private string lookingForPlanesMsg;
    [SerializeField] private string planeFoundMsg;

    [SerializeField] private ARPlaneManager _planeManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _planeManager.planeAdded += OnPlaneAdded;
        informationText.text = lookingForPlanesMsg;
    }

    void OnPlaneAdded(ARPlaneAddedEventArgs eventArgs)
    {
        informationText.text = planeFoundMsg;
    }

    private void OnDisable()
    {
        _planeManager.planeAdded -= OnPlaneAdded;
    }
}
