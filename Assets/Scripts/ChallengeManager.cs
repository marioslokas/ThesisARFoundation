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
        public GameObject firstEnabledUI;
        public GameObject secondEnabledUI;
        public GameObject mainGameObject;

        public GameObject[] _additionalChallengeObjects;

        public void EnableChallengeObjects(Vector3 centralGamePosition)
        {
            firstEnabledUI.SetActive(true);
            secondEnabledUI.SetActive(false);
        
            mainGameObject.SetActive(true);
            mainGameObject.transform.position = centralGamePosition;

            for (int i = 0; i < _additionalChallengeObjects.Length; i++)
            {
                _additionalChallengeObjects[i].SetActive(true);
            }
        }

        public void DisableChallengeObjects()
        {
            firstEnabledUI.SetActive(false);
            secondEnabledUI.SetActive(false);
        
            mainGameObject.SetActive(false);

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
            Challenge thisChallenge = _challenges[_challengeCounter];
            thisChallenge.EnableChallengeObjects(centralGamePosition);
            _challenges[_challengeCounter - 1].DisableChallengeObjects();
        }
        
        
    }

}
