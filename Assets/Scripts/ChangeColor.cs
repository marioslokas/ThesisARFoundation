using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{

    [SerializeField] private Color myColor;
    private Renderer myRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = this.GetComponent<Renderer>();
        myRenderer.material.color = myColor;
    }

}
