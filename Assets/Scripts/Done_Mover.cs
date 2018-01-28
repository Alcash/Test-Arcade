using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Done_Mover : NetworkBehaviour
{
	public float speed;
    [SerializeField]
    PlayerController m_PlayerController;

    [ServerCallback]
	void Start ()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

    public PlayerController PlayerController
    {
        get
        {
            return m_PlayerController;
        }

        set
        {
            m_PlayerController = value;
        }
    }

   
}
