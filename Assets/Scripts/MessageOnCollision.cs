using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageOnCollision : MonoBehaviour
{

    [SerializeField] private UIController _uiController;
    

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("GameBall"))
        {
            _uiController.ShowNextMessage();
        }
    }
}
