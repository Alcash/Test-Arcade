using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour {

    public Button ButtonPause, ButtonContinue, ButtonDisconnect, ButtonExit;
    GameController _gameController;
    public GameObject panel;

    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
    }


    void ButtonPauseClicked()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
    }
    void ButtonContinueClicked()
    {
        panel.SetActive(false);
        Time.timeScale = 1;
    }
    void ButtonDisconnectClicked()
    {
        _gameController.Disconnect();
    }
    void ButtonExitClicked()
    {
        Application.Quit();
    }
}
