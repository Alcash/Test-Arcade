using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GameController : NetworkBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    
    NetworkManager m_NetWorkManager;
    public Text restartText;
    [SyncVar]
    private bool gameOver;    
    [SyncVar]
    private bool restart;
    public Text gameOverText;
    public Text TextPause;

    

    List<PlayerHealth> m_Players;

    [ServerCallback]
    void Start()
    {
        m_NetWorkManager = GetComponent<NetworkManager>();
        //m_NetWorkManager
        gameOver = false;
        gameOverText.text = "";
        restartText.text = "";
        m_Players = new List<PlayerHealth>();

        StartCoroutine(SpawnWaves());
    }
    

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
                // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void Disconnect()
    {
        NetworkServer.DisconnectAll();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            TextPause.text = "Пауза";
        else
            TextPause.text = "";

        
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                var enemy = Instantiate(hazard, spawnPosition, spawnRotation);
                NetworkServer.Spawn(enemy);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            IsGameOver();
            if (gameOver)
            {
                gameOverText.text = "Game Over!";
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }
    }

        

    public void IsGameOver()
    {
        m_Players.Clear();

          var clients = NetworkServer.connections ;
          foreach (NetworkConnection client in clients)
          {
              var players = client.playerControllers;
              foreach (UnityEngine.Networking.PlayerController _player in players)
              {
                  m_Players.Add(_player.gameObject.GetComponent<PlayerHealth>());
              }
          }
        
        

        m_Players.AddRange( GetComponents<PlayerHealth>());
        gameOver = true;

        Debug.Log("IsGameOver m_Players.count " + m_Players.Count);
        foreach (PlayerHealth player in m_Players)
        {
            gameOver &= player.isDead();
        }
        


    }

    void Restart()
    {
        gameOver = false;
        gameOverText.text = "";
        restartText.text = "";
        restart = false;
       // RpcRestart();


        m_Players.ForEach(x => x.RpcAlive());
        StartCoroutine(SpawnWaves());
    }

    [ClientRpc]
    void RpcRestart()
    {
        Debug.Log("GC RpcRestart");
        Debug.Log("GC m_Players.count " + m_Players.Count);
        foreach (PlayerHealth player in m_Players)
        {
            Debug.Log("GC player isDead " + player.isDead());
            Debug.Log("GC player enabled " + player.enabled);
        }
        m_Players.ForEach(x => x.enabled = true);
        m_Players.ForEach(x => x.RpcAlive());
    }

 }