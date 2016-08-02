using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehavior : MonoBehaviour {



    
    public GameObject projectile;
    public GameObject Player;
    public GameObject repairPowerup;
    public GameObject correctAnswerPowerup;
    public float repairPowerupUpDropChance;
    public float correctAnswerPowerupDropChance;
    public float projectileSpeed = 1f;
    public float powerupSpeed = 1f;
    public int PointWorth;
    public bool inPlay;
    public GameObject deathParticle;
    public float fireRate = 0.25f; 

    // When this object comes into contact with another trigger collider 
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Try to grab a player or laser collider 
        PlayerProjectile laser = collider.gameObject.GetComponent<PlayerProjectile>();
        PlayerController player = collider.gameObject.GetComponent<PlayerController>();

        // Check if ship is in play yet
        if (inPlay)
        { 
            // If the object that is collided with is the player destroy the player 
            if (laser || player)
            {
                if (player)
                {
                    Destroy(gameObject);
                    this.transform.parent.GetComponent<SpriteRenderer>().enabled = false; //set the renderer for shield to false
                    Instantiate(deathParticle, transform.position, Quaternion.identity);
                    SoundManager.instance.PlaySingle(SoundManager.instance.explosion1);
                    player.HitEnemyShip();
                }
                // Otherwise check if the laser power is equal to the ship number, if so destroy the ship
                else if (laser.LaserPower == System.Int32.Parse(this.GetComponentInChildren<TextMesh>().text))
                {
                    int index = GameManager.instance.EnemyList.IndexOf(laser.LaserPower);
                    GameManager.instance.EnemyList.RemoveAt(index);
                    GameManager.instance.EquationsList.RemoveAt(index);

                    if (GameManager.instance.EnemyList.Count > 0)
                        GameManager.instance.CalculateLaserPower();

                    Instantiate(deathParticle, transform.position, Quaternion.identity);
                    GameManager.instance.ScoreCalculator(PointWorth);
                    this.transform.parent.GetComponent<SpriteRenderer>().enabled = false; //set the renderer for shield to false
                    SoundManager.instance.PlaySingle(SoundManager.instance.explosion1);

                    // Powerup Spawner
                    if (Random.value <= repairPowerupUpDropChance)
                    {
                        GameObject powerup = Instantiate(repairPowerup, transform.position, Quaternion.identity) as GameObject;
                        powerup.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -powerupSpeed);
                    }
                    else if (Random.value <= correctAnswerPowerupDropChance)
                    {
                        GameObject powerup = Instantiate(correctAnswerPowerup, transform.position, Quaternion.identity) as GameObject;
                        powerup.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -powerupSpeed);
                    }

                    Destroy(gameObject);
                    Destroy(collider.gameObject);

                }
                else
                {
                    collider.GetComponent<PlayerProjectile>().HitShield();
                }
            }
        }
	}

    
    // Runs every frame and tells the ship to fire shots
    void Update() {

        if (inPlay)
        {
            // Multiply shotsPerSeconds by Time.deltaTime to make it frame rate independent
            float probability = Time.deltaTime * fireRate;

            // Roll randomly to ensure a more natural fire rate among enemies 
            if (Random.value < probability)
            {
                Fire();
            }
        }


    }

    // set ship to inPlay as soon as it becomes visable 
    void OnBecameVisible()
    {
        inPlay = true;
    }

    // Enemy Fire 
    void Fire()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.enemyFire1);
        GameObject missile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }



}
