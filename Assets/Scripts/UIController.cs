using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private MessagesManager _messagesManager;
    
    [Header("Canvas references")] 
    [SerializeField] private GameObject _prepareGameCanvas;
    [SerializeField] private GameObject _messagesCanvas;
    [SerializeField] private GameObject _adjustForceCanvas;
    [SerializeField] private GameObject _toNextChallengeCanvas;

    private const int delayFrames = 1;

    public void StartGameUI()
    {
        _prepareGameCanvas.SetActive(false);
        
        ShowNextMessage();
    }

    public void ShowNextMessage()
    {
        if (_messagesManager.HasNextMessage())
        {
            _adjustForceCanvas.SetActive(false);   
            _messagesCanvas.SetActive(true);
            _messagesManager.NextMessage();
        }
        else
        {
            _toNextChallengeCanvas.SetActive(true);
        }
    }

    public void BackToGame()
    {
        _adjustForceCanvas.SetActive(true);
        _messagesCanvas.SetActive(false);
        
        UpdateManager.instance.ResumeUpdates();
    }

    
}
