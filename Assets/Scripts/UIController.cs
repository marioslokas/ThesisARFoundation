using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private MessagesManager _messagesManager;
    
    [Header("Canvas references")] 
    [SerializeField] private GameObject _prepareGameCanvas;
    [SerializeField] private GameObject _messagesCanvas;
    [SerializeField] private GameObject _adjustSimulationCanvas;
    [SerializeField] private GameObject _toNextChallengeCanvas;
    [SerializeField] private string introSceneName;

    private const int delayFrames = 1;

    public void Initialize(GameObject messagesCanvas,  GameObject adjustSimulationCanvas)
    {
        _messagesCanvas = messagesCanvas;
        _adjustSimulationCanvas = adjustSimulationCanvas;
    }

    public void StartGameUI()
    {
        _prepareGameCanvas.SetActive(false);
        
        ShowNextMessage();
    }
    
    

    public void ShowNextMessage()
    {
        // play message if there are still messages
        if (_messagesManager.HasNextMessage())
        {
            _adjustSimulationCanvas.SetActive(false);   
            _messagesCanvas.SetActive(true);
            
            // raises the message counter
            _messagesManager.NextMessage();
        }
        else
        {
            _toNextChallengeCanvas.SetActive(true);
        }
        
    }

    public void EnableNextChallengeIfNoMessages()
    {
        if (!_messagesManager.HasNextMessage())
        {
            _toNextChallengeCanvas.SetActive(true);
        }
    }

    public void BackToGame()
    {
        _adjustSimulationCanvas.SetActive(true);
        _messagesCanvas.SetActive(false);
        
        UpdateManager.instance.ResumeUpdates();

    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(introSceneName);
    }


}
