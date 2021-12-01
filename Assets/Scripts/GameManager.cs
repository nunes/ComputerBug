using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private GameObject _obstacleContainer;


    public int currentLevel = 0;


    public List<string> levelNames;


    private GameObject _menucanvas;
    private GameObject _menuItemButtonStart;
    private GameObject _menuItemButtonContinue;
    private GameObject _player;


    private bool _exitWasEnabled = true;

    // Start is called before the first frame update
    void Awake()
    {

        GameObjectUtil.KeepAliveOneCopy("GameManager", this.gameObject);


        _player = GameObject.Find("Player");

        _menucanvas = GameObject.Find("MenuCanvas");

        _menuItemButtonContinue = GameObject.Find("ButtonContinue");
        _menuItemButtonStart = GameObject.Find("ButtonStart");

        UpdateMenuItems();

    }

    void Update()
    {
        CheckShowMenu();
    }

    private void CheckShowMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused())
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void CheckExit()
    {
        bool exitEnabled = true;


        CableScript[] cables = _obstacleContainer.GetComponentsInChildren<CableScript>();

        foreach (CableScript cable in cables)
        {
            if (cable.targetComponent != null 
                    && cable.targetComponent.TryGetComponent<HeatsinkScript>(out HeatsinkScript connectedHeatSink))
            {
                exitEnabled &= connectedHeatSink.isCut;
            }
        }

        if (exitEnabled)
        {
            ExitScript[] exits = _obstacleContainer.GetComponentsInChildren<ExitScript>();

            foreach (ExitScript exit in exits)
            {
                exit.EnableExit();
            }
        }

        if (!_exitWasEnabled && exitEnabled)
        {
            _player.GetComponent<PlayerScript>().ExitEnabled_Event();
        }

        _exitWasEnabled = exitEnabled;

    }


    public void StartGame()
    {
        NextLevel();
        UnPauseGame();
    }

    public void ContinueGame()
    {
        UnPauseGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void UnPauseGame()
    {
        if (_menucanvas.activeInHierarchy)
        {
            _menucanvas.SetActive(false);
            Time.timeScale = 1f;

        }
    }

    private void PauseGame()
    {
        _menucanvas.SetActive(true);
        UpdateMenuItems();
        Time.timeScale = 0f;
    }

    private bool GameIsPaused()
    {
        return (_menucanvas.activeInHierarchy);
    }

    private void UpdateMenuItems()
    {
        if (SceneManager.GetActiveScene().name =="Menu")
        {
            _menuItemButtonContinue.SetActive(false);
            _menuItemButtonStart.SetActive(true);
        }
        else
        {
            _menuItemButtonContinue.SetActive(true);
            _menuItemButtonStart.SetActive(false);
        }
    }

    public void Restart()
    {
        StartCoroutine(LoadLevel(levelNames[currentLevel]));
    }

    private IEnumerator LoadLevel(string levelname)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelname);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        RefreshSceneObjects();

    }

    public void NextLevel()
    {
        if (currentLevel < levelNames.Count - 1)
        {
            currentLevel++;
        }

        _player.GetComponent<PlayerScript>().Success_Event();

        StartCoroutine(LoadLevel(levelNames[currentLevel]));

    }

    public void RefreshSceneObjects()
    {

        _player.GetComponent<PlayerScript>().Reset();

        _obstacleContainer = GameObject.Find("Obstacles");

        CheckExit();

    }
}
