using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class CallSphereOnTap : MyUpdatableBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_SpherePrefab;
    
    public GameObject spherePrefab
    {
        get { return m_SpherePrefab; }
        set { m_SpherePrefab = value; }
    }
    
    public GameObject spawnedObject { get; private set; }
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARSessionOrigin m_SessionOrigin;

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    public override void MyStart()
    {
        throw new System.NotImplementedException();
    }

    public override void MyUpdate()
    {
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);

        if (m_SessionOrigin.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(m_SpherePrefab, hitPose.position + Vector3.up, hitPose.rotation);
            }
            else
            {
                spawnedObject.transform.position = hitPose.position + Vector3.up;
            }
        }
    }
}
