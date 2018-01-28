using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour {

    public Button ButtonPause, ButtonContinue, ButtonDisconnect, ButtonExit;
    public GameController _gameController;
    public GameObject panel;

    private void Start()
    {
        ButtonPause.onClick.AddListener(ButtonPauseClicked);
        ButtonContinue.onClick.AddListener(ButtonContinueClicked);
        ButtonDisconnect.onClick.AddListener(ButtonDisconnectClicked);
        ButtonExit.onClick.AddListener(ButtonExitClicked);
        
    }


    void ButtonPauseClicked()
    {
        Time.timeScale = 0;
        _gameController.RpcPause(true);
        panel.SetActive(true);

        
    }
    void ButtonContinueClicked()
    {
        panel.SetActive(false);
        _gameController.RpcPause(false);
        Time.timeScale = 1;
    }
    void ButtonDisconnectClicked()
    {
        Time.timeScale = 1;
        _gameController.Disconnect();
    }
    void ButtonExitClicked()
    {
        Application.Quit();
    }
}
