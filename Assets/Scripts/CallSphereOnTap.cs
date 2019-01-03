using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class CallSphereOnTap : MyUpdatableBehaviour
{

    [SerializeField] private GameObject placedObject;
    
    [SerializeField] private GameObject controlCanvas;
    
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARSessionOrigin m_SessionOrigin;

    private bool endPlacement = false;

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }


    public override void MyUpdate()
    {
        if (Input.touchCount > 3 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            endPlacement = !endPlacement;
            controlCanvas.active = !controlCanvas.active;
        }
        
        if (endPlacement)
        {
            return;
        }
        
        if (Input.touchCount == 0)
            return;
        

        var touch = Input.GetTouch(0);

        if (m_SessionOrigin.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            placedObject.SetActive(true);
            placedObject.transform.position = hitPose.position;
//            if (spawnedObject == null)
//            {
//                spawnedObject = Instantiate(m_SpherePrefab, hitPose.position + Vector3.up, hitPose.rotation);
//            }
//            else
//            {
//                spawnedObject.transform.position = hitPose.position + new Vector3(0,0.5f,0);
//            }
        }
    }
}
