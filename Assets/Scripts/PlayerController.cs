using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;


public class PlayerController : NetworkBehaviour
{
    
    public float speed;
    public float Shot_Speed;

    public float tilt;
	public Boundary boundary;

	public GameObject shot;
    public GameObject m_Body;

    public Transform shotSpawn;
    
    float moveHorizontal;
    float moveVertical;

    [SyncVar]
    public float fireRate;	 
    
    [SyncVar]
	private float nextFire;

    [SerializeField]
    [SyncVar]
    private bool gameOver;
    
    [SerializeField]
    [SyncVar(hook = "UpdateScore")]
    private int score;

    public Text scoreText;

    public Text gameOverText;

    public string m_DeadMessage = "ты умер! поздравляю!";

    public string m_ScoreLabel = "Унесенные жизни врагов";

    new Rigidbody rigidbody;

    AudioSource audioSource;

    void Update ()
	{
        if (!isLocalPlayer)
        {
            scoreText.enabled = false;
            gameOverText.enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire) 
		{
            nextFire = Time.time + fireRate;
           
            //Debug.Log("nextFire" + nextFire);
            CmdFire(shotSpawn.position, shotSpawn.rotation);

        }

         moveHorizontal = Input.GetAxis("Horizontal");
         moveVertical = Input.GetAxis("Vertical");
    }

   

    

    void FixedUpdate ()
	{
        if (!isLocalPlayer)
            return;        

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        rigidbody.velocity = movement * speed;

        rigidbody.position = new Vector3
		(
			Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
		);

        rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}
    [Command]
    void CmdFire(Vector3 position, Quaternion rotation)
    {
        
        var shot_gameobject = Instantiate(shot, position, rotation);
        var rigidbody = shot_gameobject.GetComponent<Rigidbody>();
        rigidbody.velocity = shot_gameobject.transform.forward * speed;
        audioSource.Play();
        shot_gameobject.GetComponent<Done_Mover>().PlayerController = this;
        NetworkServer.Spawn(shot_gameobject);
    }

    public void AddScore(int newScoreValue)
    {
       score += newScoreValue;
        
    }

    void UpdateScore(int value)
    {
        score = value;
        scoreText.text = m_ScoreLabel+": " + score;
    }

    public void GameOver()
    {
        gameOverText.text = m_DeadMessage;
        
        m_Body.SetActive(false);
        GetComponent<MeshCollider>().enabled = false;
        rigidbody.velocity = Vector3.zero;
        CmdGameOver();
        this.enabled = false;
    }

    public void CmdGameOver()
    {
        
        m_Body.SetActive(false);
        GetComponent<MeshCollider>().enabled = false;
        rigidbody.velocity = Vector3.zero;
    }

    private void Start()
    {
        score = 0;

        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        gameOverText.text = "";
        UpdateScore(0);
    }

    public void Restart()
    {
        score = 0;
        gameOverText.text = "";
        this.enabled = true;
        m_Body.SetActive(true);
        GetComponent<MeshCollider>().enabled = true;
        Debug.Log("Restart");
        UpdateScore(0);
    }

    
        

    
}

