using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float MaxPlayerHealth;
    public float playerHealth;
    public float Player1stDamageAmount;
    public float Player2ndDamageAmount;
    public float Player3rdDamageAmount;
    public float playerSpeed; 
	public Vector2 playerPos = new Vector2();
	public float xPadding = 0.75f;
	public float yPadding = 0.6f;
	public float projectileDelay = 1.0f;
	public float projectileSpeed = 1.0f;
	public GameObject projectile;
    public GameObject deathParticle;
    public GameObject redLaserParticle;
    public float deathDelay;

    public GameObject Formation;
    private bool FireButtonDown;
    private float nextFire;
	private float xmin;
	private float xmax;
	private float ymin;
	private float ymax;



    // At the start of the players esistance his position is set to playerPos and his boundarys are set the be 
    // xPadding and yPadding away from the edge of the camera 
    void Start () {

        transform.position = playerPos;
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector2 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector2 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		
		Vector2 bottommost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector2 topmost = Camera.main.ViewportToWorldPoint(new Vector3(1,1,distance));
		
		xmin = leftmost.x + xPadding;
		xmax = rightmost.x - xPadding;
		
		ymin = bottommost.y + yPadding;
		ymax = topmost.y - yPadding;

        // Set player health in UI
        playerHealth = MaxPlayerHealth;
        GameManager.instance.HealthText = GameObject.Find("Health");
        GameManager.instance.HealthText.GetComponent<Text>().text = "Health: " + MaxPlayerHealth;

    }
	
	// FixedUpdate is called at a steady rate regardless of frame rate
	void FixedUpdate () {

#if UNITY_ANDROID || UNITY_IOS
        // Get input and clamp the players movement within a set boundary 
        float xPos = transform.position.x + (VirtualJoystickRegion.VJRnormals.x * playerSpeed);
		float yPos = transform.position.y + (VirtualJoystickRegion.VJRnormals.y * playerSpeed);
		playerPos = new Vector2(Mathf.Clamp(xPos, xmin, xmax), Mathf.Clamp(yPos, ymin, ymax));
		
		// Move player
		transform.position = playerPos;
#else
        // Get input and clamp the players movement within a set boundary 
        float xPos = transform.position.x + (Input.GetAxis("Horizontal") * playerSpeed);
        float yPos = transform.position.y + (Input.GetAxis("Vertical") * playerSpeed);
        playerPos = new Vector2(Mathf.Clamp(xPos, xmin, xmax), Mathf.Clamp(yPos, ymin, ymax));

        // Move player
        transform.position = playerPos;
#endif
    }

    void Update () {

        // check if player is attempting to fire
#if !(UNITY_ANDROID || UNITY_IOS)
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
		{
            // Set the delay to prevent player from firing again until projectile delay is over
			nextFire = Time.time + projectileDelay;

            // Create the projectile and give it a velocity
            SoundManager.instance.PlaySingle(SoundManager.instance.playerFire1);
            GameObject projectileInstance = Instantiate(projectile, this.playerPos, Quaternion.identity) as GameObject;
			projectileInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed);
		}
#else
        Debug.Log("Button" + FireButtonDown);
        if (Time.time > nextFire && GameManager.instance.FireButtonDown)
        {
            // Set the delay to prevent player from firing again until projectile delay is over
            nextFire = Time.time + projectileDelay;

            // Create the projectile and give it a velocity
            SoundManager.instance.PlaySingle(SoundManager.instance.playerFire1);
            GameObject projectileInstance = Instantiate(projectile, this.playerPos, Quaternion.identity) as GameObject;
            projectileInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed);
        }
#endif
        // Set Damage sprites 
        if (playerHealth == MaxPlayerHealth)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (playerHealth == Player3rdDamageAmount)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (playerHealth == Player2ndDamageAmount)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (playerHealth == Player1stDamageAmount)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

#if UNITY_ANDROID || UNITY_IOS

    //two functions for the fire button event trigger to call
    public void SetFireButtonDown()
    {
        GameManager.instance.FireButtonDown = true;
    }

    public void SetFireButtonUp()
    {
        GameManager.instance.FireButtonDown = false;
    }

#endif

    // function to handle what happens when the player collides with an enemy ship
    public void HitEnemyShip(){
       
        Time.timeScale = 0.25f;
        gameObject.SetActive(false);
        SoundManager.instance.PlaySingle(SoundManager.instance.playerExplosion);
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Invoke("ExitToReplayScreen", deathDelay);
    }

    // Activates when a trigger collides with the player
    void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyProjectile laser = collider.gameObject.GetComponent<EnemyProjectile>();

        // if the collision is with an enemy laser take damage 
        if (laser)
        {
            playerHealth -= laser.GetDamage();
            SoundManager.instance.PlaySingle(SoundManager.instance.explosion1);
            GameManager.instance.HealthText.GetComponent<Text>().text = "Health: " + playerHealth.ToString();

            if (playerHealth <= 0)
            {
                
                Time.timeScale = 0.25f;

                gameObject.SetActive(false);
                Instantiate(deathParticle, transform.position, Quaternion.identity);
                SoundManager.instance.PlaySingle(SoundManager.instance.playerExplosion);
                Invoke("ExitToReplayScreen", deathDelay);
            }
            // Destroy laser when it hits something
            laser.Hit();
        }
    }

    void ExitToReplayScreen()
    {
        Time.timeScale = 1f;
        GameManager.instance.FinalScore = GameManager.instance.Score;
        GameManager.instance.Score = 0;
        SceneManager.LoadScene("Lose Screen");
    }

}
