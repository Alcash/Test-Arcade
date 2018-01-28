using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Done_DestroyByContact : NetworkBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	private PlayerController playerController;

	void Start ()
	{
        /*	GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
            if (gameControllerObject != null)
            {
                playerController = gameControllerObject.GetComponent <PlayerController>();
            }
            if (playerController == null)
            {
                Debug.Log ("Cannot find 'GameController' script");
            }*/

       
    }

    [ServerCallback]
    void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Boundary" || other.tag == "Enemy")
		{
			return;
		}

		if (explosion != null)
		{
            CmdExplosion(transform.position, transform.rotation);
            
		}

		if (other.tag == "Player")
		{
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            CmdDestroy(other.transform.position, other.transform.rotation);
            playerHealth.RpcGameOver();
		}
        
        if(other.tag == "PlayerBullet")
        {
            playerController = other.GetComponent<Done_Mover>().PlayerController;
            playerController.AddScore(scoreValue);
        }
        
		
		Destroy (gameObject);
	}
    [Command]
    void CmdDestroy(Vector3 position, Quaternion rotation)
    {
        var expl = Instantiate(playerExplosion, position, rotation);
        NetworkServer.Spawn(expl);
    }

    [Command]
    void CmdExplosion(Vector3 position, Quaternion rotation)
    {
        var expl = Instantiate(explosion, position, rotation);
        NetworkServer.Spawn(expl);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary")
        {
            Destroy(gameObject);
        }
    }
}