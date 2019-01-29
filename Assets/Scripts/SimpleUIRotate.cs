using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUIRotate : MonoBehaviour
{

    private RectTransform _rectTransform;

    [SerializeField] private float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        _rectTransform.RotateAround(_rectTransform.position, Vector3.forward, speed);
    }
}
