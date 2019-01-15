using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessagesManager : MonoBehaviour
{

    private string[] mainMessages;
    private string[] secondaryMessages;
    
    [SerializeField] private Text messageTextArea;
    [SerializeField] private Text secondaryMessageArea;
    
    private int _messageCounter;

    private const int DelayFrames = 1;

    public void LoadMessages(string[] mainMessages, string[] secondaryMessages)
    {
        this.mainMessages = mainMessages;
        this.secondaryMessages = secondaryMessages;
        _messageCounter = 0;
    }

    public bool HasNextMessage()
    {
        return _messageCounter < mainMessages.Length;
    }

    public void NextMessage(bool pauseUpdates = true)
    {
        if (!HasNextMessage())
        {
            Debug.Log("Messages end");
            messageTextArea.text = "";
            return;
        }
        
        messageTextArea.text = mainMessages[_messageCounter];
        secondaryMessageArea.text = secondaryMessages[_messageCounter];
        _messageCounter++;

        if (pauseUpdates)
        {
            StartCoroutine(PauseUpdatesDelayed()); 
        }
        
    }
    
    IEnumerator PauseUpdatesDelayed()
    {
        for (int i = 0; i < DelayFrames; i++)
        {
            yield return null;
        }
        
        UpdateManager.instance.PauseUpdates();
    }

}
