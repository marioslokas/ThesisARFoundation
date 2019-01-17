using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    /// <summary>
    /// Challenge class has references to all the objects that need to be enabled to start a challenge
    /// </summary>
    [Serializable]
    class Challenge
    {
        public bool startWithMessage = true;
        public GameObject _messageManager;
        public GameObject _targetingManager;
        
        [Header("Main game object for the simulation")]
        public GameObject mainGameObject;
        public GameObject mainObjectParent;
        public GameObject secondaryGameObject;

        public GameObject[] _additionalChallengeObjects;

        [Header("Messages to the player")] 
        public string[] mainMessages;
        public string[] secondaryMessages;
        
        public void EnableChallengeObjects(Vector3 centralGamePosition)
        {
            mainObjectParent.SetActive(true);
            mainObjectParent.transform.position = centralGamePosition;
            
            mainGameObject.SetActive(true);
            LineRenderer secondaryLineRenderer = null;
            Transform secondaryTransform = null;
            if (secondaryGameObject)
            {
                secondaryGameObject.SetActive(true);
                secondaryLineRenderer = secondaryGameObject.GetComponentInChildren<LineRenderer>();
                secondaryTransform = secondaryGameObject.transform;
            }
            
            // initialize targeting manager
            _targetingManager.GetComponent<OneHandTargetingManager>().Initialize(mainGameObject.transform.position,
                mainGameObject.transform,
                mainGameObject.GetComponentInChildren<LineRenderer>(),
                secondaryTransform,
                secondaryLineRenderer);
            
            // provide messages to the message manager
            _messageManager.GetComponent<MessagesManager>().LoadMessages(mainMessages, secondaryMessages);
            
            // enable managers accordingly with whether we are starting with a message or not
            _messageManager.SetActive(startWithMessage);
            if (startWithMessage)
            {
                _messageManager.GetComponent<MessagesManager>().NextMessage();
            }
            
            _targetingManager.SetActive(!startWithMessage);


            for (int i = 0; i < _additionalChallengeObjects.Length; i++)
            {
                _additionalChallengeObjects[i].SetActive(true);
            }
        }

        public void DisableChallengeObjects()
        {
            mainGameObject.SetActive(false);
            if (secondaryGameObject) secondaryGameObject.SetActive(false);
            
            _messageManager.SetActive(false);
            _targetingManager.SetActive(false);
            mainObjectParent.SetActive(false);

            for (int i = 0; i < _additionalChallengeObjects.Length; i++)
            {
                _additionalChallengeObjects[i].SetActive(false);
            }
        }
    }

    [SerializeField] private Challenge[] _challenges;
    [HideInInspector] public Vector3 centralGamePosition;
    
    private int _challengeCounter = 0;

    /// <summary>
    /// Start the next challenge. Disable the previous.
    /// </summary>
    public void NextChallenge()
    {
        if (_challengeCounter >= _challenges.Length)
        {
            Debug.LogError("No more challenges.");
            _challenges[_challengeCounter - 1].DisableChallengeObjects();
            return;
        }

        if (_challengeCounter == 0)
        {
            Challenge thisChallenge = _challenges[_challengeCounter];
            thisChallenge.EnableChallengeObjects(centralGamePosition);
        }
        else
        {
            _challenges[_challengeCounter - 1].DisableChallengeObjects();
            Challenge thisChallenge = _challenges[_challengeCounter];
            thisChallenge.EnableChallengeObjects(centralGamePosition);
        }

        _challengeCounter++;
    }

}
