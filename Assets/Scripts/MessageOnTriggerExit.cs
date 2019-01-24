using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageOnTriggerExit : MonoBehaviour
{
    [SerializeField] private UIController _uiController;


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("GameBall"))
        {
            _uiController.ShowNextMessage();
        }
    }

}
