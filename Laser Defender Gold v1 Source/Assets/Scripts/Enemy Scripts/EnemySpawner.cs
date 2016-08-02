using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    /* Enemy Spawner is placed into formations that are a game object filled with positions, 
    each position is filled with a copy of the enemy prefab  */

    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 10f;
    public int MinOperandValue = 5;
    public int MaxOperandValue = 10;
    public int formationSize = 7;


    // Spawns enemies 
    void Start()
    {
        GameManager.instance.CurrentFormation = this.gameObject;

        // Create the enemy list
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        //spawn all the enemies 
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }

        // Set the number of each enemey
        GameManager.instance.GenerateEquationAndAnswersList(formationSize);
        SetNumbersToEachEnemy();

        // Set LaserPowerText in GameManager to LaserPower then calculate inital Laser Power
        GameManager.instance.LaserPowerText = GameObject.Find("Laser Power");
        GameManager.instance.CalculateLaserPower();

    }

    // Draw a gizmo on each position to make them easier to see in the editor
    public void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    // Update is called once per frame
    // Checks if the enemy formation has hit its left or right boundary and flips movement direction if nessecary
    void Update()
    {
        // Check if all members are dead 
        if (AllMembersDead())
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Spawn(this.gameObject);
        }

    }


    // Checks if all members of the formation are dead, if so returns true otherwise returns false.
    bool AllMembersDead()
    {
        foreach (Transform childPostitionGameObject in transform)
        {
            if (childPostitionGameObject.childCount > 0)
            {
                return false;
            }
        }

        return true;
    }

    // Sets a random number to each enemy between a specified minimum and a specified maximum 
    void SetNumbersToEachEnemy()
    {
        int positioncount = 0;

        // go into each position in the formation
        foreach (Transform child in transform)
        {
            // go into each enemy in a position 
            foreach (Transform grandChild in child)
            {
                //go into the text mesh object attached to the enemy
                foreach (Transform greatGrandChild in grandChild)
                {
                    // Check to make sure enemy does not already have a number
                    if (greatGrandChild.GetComponent<TextMesh>().text.Length == 0)
                    {
                        // Add number to enemy
                        greatGrandChild.GetComponent<TextMesh>().text = GameManager.instance.EnemyList[positioncount].ToString();
                        positioncount++;
                    }
                }
            }
        }
    }

}



