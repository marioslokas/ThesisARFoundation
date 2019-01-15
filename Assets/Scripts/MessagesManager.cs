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
    }

}
