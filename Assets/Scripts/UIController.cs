﻿using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private MessagesManager _messagesManager;
    
    [Header("Canvas references")] 
    [SerializeField] private GameObject prepareGameCanvas;
    [SerializeField] private GameObject messagesCanvas;
    [SerializeField] private GameObject adjustForceCanvas;


    private const int delayFrames = 1;

    public void StartGameUI()
    {
        prepareGameCanvas.SetActive(false);
        
        ShowNextMessage();
    }

    public void ShowNextMessage()
    {
        if (_messagesManager.HasNextMessage())
        {
            adjustForceCanvas.SetActive(false);   
            messagesCanvas.SetActive(true);
            _messagesManager.NextMessage();

            StartCoroutine(PauseUpdatesDelayed());
        }
    }

    public void BackToGame()
    {
        adjustForceCanvas.SetActive(true);
        messagesCanvas.SetActive(false);
        
        UpdateManager.instance.ResumeUpdates();
    }

    IEnumerator PauseUpdatesDelayed()
    {
        for (int i = 0; i < delayFrames; i++)
        {
            yield return null;
        }
        
        UpdateManager.instance.PauseUpdates();
    }
}
