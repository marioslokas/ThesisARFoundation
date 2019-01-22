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
        public bool playMessageOnFire = true; // show a message when firing the object. if false, let the collisions take care of the messages
        public GameObject _messageManager;
        public GameObject _targetingManager;
        
        [Header("Main game object for the simulation")]
        public GameObject[] mainGameObjects;
        public GameObject mainObjectParent;

        public GameObject[] _additionalChallengeObjects;

        [Header("Messages to the player")] 
        public string[] mainMessages;
        public string[] secondaryMessages;

        private Vector3[] mainGameObjectPositions;
        private Transform[] mainGameObjectTransforms;
        private LineRenderer[] mainGameObjectLineRenderers;
        private Rigidbody[] mainGameObjectsRigidbodies;
        
        public void EnableChallengeObjects(Vector3 centralGamePosition)
        {
            
            mainObjectParent.SetActive(true);
            mainObjectParent.transform.position = centralGamePosition;
            
            mainGameObjectPositions = new Vector3[mainGameObjects.Length];
            mainGameObjectTransforms = new Transform[mainGameObjects.Length];
            mainGameObjectLineRenderers = new LineRenderer[mainGameObjects.Length];
            mainGameObjectsRigidbodies = new Rigidbody[mainGameObjects.Length];
            
            for (int i = 0; i < mainGameObjects.Length; i++)
            {
                mainGameObjectPositions[i] = mainGameObjects[i].transform.position;
                mainGameObjectTransforms[i] = mainGameObjects[i].transform;
                mainGameObjectLineRenderers[i] = mainGameObjects[i].GetComponentInChildren<LineRenderer>();
                mainGameObjectsRigidbodies[i] = mainGameObjects[i].GetComponent<Rigidbody>();
                // activate main game objects
                mainGameObjects[i].SetActive(true);
            }
            
            
            // initialize targeting manager
            _targetingManager.GetComponent<OneHandTargetingManager>().Initialize(mainGameObjectPositions,
                mainGameObjectTransforms,
                mainGameObjectLineRenderers,
                mainGameObjectsRigidbodies,
                playMessageOnFire);
            
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
            for (int i = 0; i < mainGameObjects.Length; i++)
            {
                mainGameObjects[i].SetActive(false);  
            }
            
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
