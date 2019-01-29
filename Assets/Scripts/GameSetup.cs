using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    private enum ArEnvironment
    {
        Earth,
        Space,
        None
    }

    private enum BallProjectile
    {
        GolfBall,
        Meteor,
        None
    }
    
    struct GameOptions
    {
        public ArEnvironment _arEnvironment;
        public BallProjectile _ballProjectile;

        public GameOptions(ArEnvironment arEnvironment, BallProjectile ballProjectile)
        {
            _arEnvironment = arEnvironment;
            _ballProjectile = ballProjectile;
        }
    }

    [SerializeField] private GameObject startOptions;
    [SerializeField] private GameObject howToPlayInfo;
    [SerializeField] private GameObject startGameButton;

//    [SerializeField] private Dropdown environmentDropdown;
//    [SerializeField] private Dropdown ballDropdown;

    private GameOptions _options;
    private ArEnvironment[] _environmentValues;
    private BallProjectile[] _projectileValues;

    private string sceneToLoad;

    [SerializeField]
    private string newtonARScene;
    [SerializeField]
    private string gravityARScene;

    private bool sceneLoadSet = false;

    [Header("Help objects")] 
    [SerializeField]
    private GameObject help1;
    [SerializeField]
    private GameObject help2;

    void Start()
    {
//        _options = new GameOptions(ArEnvironment.Earth, BallProjectile.GolfBall);
//
//        _environmentValues = new ArEnvironment[]
//            {ArEnvironment.Earth, ArEnvironment.Space, ArEnvironment.None};
//        _projectileValues = new BallProjectile[]
//            {BallProjectile.GolfBall, BallProjectile.Meteor, BallProjectile.None};
//
//        // dropdown delegates
//        environmentDropdown.onValueChanged.AddListener(
//            delegate { EnvironmentDropdownValueChanged(environmentDropdown); }
//        );
//
//        ballDropdown.onValueChanged.AddListener(delegate { BallDropdownValueChanged(ballDropdown); });
    }

//    void EnvironmentDropdownValueChanged(Dropdown envDropdown)
//    {
//        _options._arEnvironment = _environmentValues[envDropdown.value];
//        Debug.Log("New Options:" + _options._arEnvironment + " " + _options._ballProjectile);
//    }
//
//    void BallDropdownValueChanged(Dropdown ballDropdown)
//    {
//        _options._ballProjectile = _projectileValues[ballDropdown.value];
//        Debug.Log("New Options:" + _options._arEnvironment + " " + _options._ballProjectile);
//    }

    public void SetSceneToLoad(string sceneName)
    {
        sceneToLoad = sceneName;
        sceneLoadSet = true;
    }

    public void ToInfoView()
    {
        startOptions.SetActive(false);
        howToPlayInfo.SetActive(true);
    }

    public void ToSelectView()
    {
        startOptions.SetActive(true);
        howToPlayInfo.SetActive(false);
        sceneLoadSet = false;
        
        help1.SetActive(true);
        help2.SetActive(false);
        startGameButton.SetActive(false);
    }

    public void EnableStartGameButton()
    {
        if (sceneLoadSet)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }

    public void SwitchHelpInfo()
    {
        help1.SetActive(help2.activeSelf);
        help2.SetActive(!help1.activeSelf);
    }

    public void LoadArScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

}
