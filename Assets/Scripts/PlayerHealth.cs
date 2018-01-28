using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

    PlayerController m_PlayerController;
    bool gameOver;
    // Use this for initialization
    void Start () {
        m_PlayerController = GetComponent<PlayerController>();

    }

    [ClientRpc]
    public void RpcGameOver()
    {
        m_PlayerController.GameOver();
        
        gameOver = true;
    }

    public bool isDead()
    {
        return gameOver;
    }

    [ClientRpc]
    public void RpcAlive()
    {
        gameOver = false;
        m_PlayerController.enabled = true;
        m_PlayerController.Restart();
    }


}
