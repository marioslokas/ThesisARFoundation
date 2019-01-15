using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessagesManager : MonoBehaviour
{

    public string[] mainMessages;
    public string[] secondaryMessages;
    
    [SerializeField] private Text messageTextArea;
    [SerializeField] private Text secondaryMessageArea;
    
    private int messageCounter;

    private const int delayFrames = 1;

    public bool HasNextMessage()
    {
        return messageCounter < mainMessages.Length;
    }

    public void NextMessage()
    {
        if (!HasNextMessage())
        {
            Debug.Log("Messages end");
            messageTextArea.text = "";
            return;
        }
        
        messageTextArea.text = mainMessages[messageCounter];
        secondaryMessageArea.text = secondaryMessages[messageCounter];
        messageCounter++; 
        
        StartCoroutine(PauseUpdatesDelayed());
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
