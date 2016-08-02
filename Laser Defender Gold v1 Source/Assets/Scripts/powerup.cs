using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class powerup: MonoBehaviour
{
    public bool repairPowerup;
    public bool answerPowerup;

    private string findName = "";

    //Destroy Projectile when it leaves the screen
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerController player = collider.gameObject.GetComponent<PlayerController>();

        Debug.Log("test Collison Confirmed");

        if (player)
        {
            Debug.Log("player collision Confirmed");
            Debug.Log("Repair Powerup is " + repairPowerup);
            if (repairPowerup)
            {
                SoundManager.instance.PlaySingle(SoundManager.instance.powerUpRepair);
                Debug.Log("Repair Powerup if entered");
                if (player.GetComponent<PlayerController>().playerHealth < player.GetComponent<PlayerController>().MaxPlayerHealth)
                {
                    player.GetComponent<PlayerController>().playerHealth += 100;
                    GameManager.instance.HealthText.GetComponent<Text>().text = "Health: " + player.GetComponent<PlayerController>().playerHealth;
                }
                Destroy(gameObject);
            }
            else if (answerPowerup)
            {
                SoundManager.instance.PlaySingle(SoundManager.instance.powerUpAnswer);
                string EquationText = GameManager.instance.LaserPowerText.GetComponent<Text>().text;
                findName = EquationText;
                findName = findName.Substring(findName.IndexOf(":") + 2).TrimStart();
                Debug.Log("Trimed String " + findName);
                Debug.Log("Test PowerUp " + GameManager.instance.EquationsList.FindIndex(isName));
                EquationText = EquationText.TrimEnd('?') + GameManager.instance.EnemyList[GameManager.instance.EquationsList.FindIndex(isName)].ToString();
                Debug.Log("Equation Text " + EquationText);
                GameManager.instance.LaserPowerText.GetComponent<Text>().text = EquationText;
                Destroy(gameObject);
            }
        }

     }

    private bool isName(string name)
    {
        return (name == findName);
    }
    



 }
