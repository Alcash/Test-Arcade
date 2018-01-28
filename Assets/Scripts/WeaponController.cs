using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WeaponController : NetworkBehaviour
{
	public GameObject bullet;
	public Transform shotSpawn;
	public float fireRate;
	public float delay;
    public float speed;

    void Start ()
	{
		InvokeRepeating ("Fire", delay, fireRate);
	}

    void Fire()
    {
        CmdFire(shotSpawn.position, shotSpawn.rotation);        
    }

    [Command]
    void CmdFire (Vector3 position, Quaternion rotation)
	{

		var  _shot =  Instantiate(bullet, position, rotation);
        _shot.GetComponent<Rigidbody>().velocity = _shot.transform.forward * speed;

        GetComponent<AudioSource>().Play();
        NetworkServer.Spawn(_shot);
	}
}
