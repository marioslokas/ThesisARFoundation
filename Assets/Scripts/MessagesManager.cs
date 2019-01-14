using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessagesManager : MonoBehaviour
{

    private string[] messages = {
        "Welcome to this little simulation! Here you see a little ball floating in space. Use your thumb to change the direction and magnitude of a force to be applied on it. When you are ready, press Fire.",
        "What will happen to this ball after a force has been applied to it? Remember that there is no gravity or air in space."
    };
    
    [SerializeField] private Text messageTextArea;

    private int messageCounter;

    private const int delayFrames = 3;


    public void NextMessage()
    {
        if (messageCounter >= messages.Length)
        {
            Debug.Log("Messages end");
            messageTextArea.text = "";
            return;
        }
        
        messageTextArea.text = messages[messageCounter];
        messageCounter++; 
    }

}
