using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public GameObject[] FormationArray;
    public GameObject CurrentFormation;
    public GameObject Canvas;
    public GameObject StarField1;
    public GameObject StarField2;
    public GameObject ScoreText;
    public GameObject LaserPowerText;
    public GameObject HealthText;
    public GameObject JoyStick;
    public int MaxOperandValue = 20;
    public int MinOperandValue = 1;
    public bool FireButtonDown;
    public int Score = 0;
    public int FinalScore = 0;
    public float LaserPower= 0;
    public static GameManager instance = null;	//Static instance of GameManager which allows it to be accessed by any other script.
    public List<float> EnemyList;
    public List<string> EquationsList;
    public int EquationType = 0;


    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a Sound Player
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    // Use this for initialization
    public void InitGame() {

        //Setup Canvas for UI
        Instantiate(Canvas);

        //Spawn Player
        Instantiate(player);

#if UNITY_ANDROID || UNITY_IOS
        //Setup Virtual Joystick and Button 
        Instantiate(JoyStick);
#endif

        //Spawn Starfield
        Instantiate(StarField1);
        Instantiate(StarField2);

        //Create enemy formation list and spawn first enemy formation
        Instantiate(FormationArray[Random.Range(0, FormationArray.Length)]);

    }
	

    public void Spawn (GameObject oldFormation)
    {
            Destroy(oldFormation);
            Instantiate(FormationArray[Random.Range(0, FormationArray.Length)], 
                FormationArray[Random.Range(0, FormationArray.Length)].transform.position, Quaternion.identity);
    }


    public void CalculateLaserPower()
    {
        int choice = Random.Range(0, EnemyList.Count);
        LaserPowerText.GetComponent<Text>().text = "Laser Power: " + EquationsList[choice];
        LaserPower = EnemyList[choice];
    }


    public void ScoreCalculator(int Points)
    {
        Score += Points;
        ScoreText.GetComponent<Text>().text = "Score: " + Score.ToString();
    }

    public void GenerateEquationAndAnswersList(int formationSize)
    {
        switch(EquationType)
        {
            case 0: //Addition
                GenerateListOfEnemyAnswersAndEquationsAddition(formationSize);
                break;

            case 1: //Subtraction
                GenerateListOfEnemyAnswersAndEquationsSubtraction(formationSize);
                break;

            case 2: //Division
                GenerateListOfEnemyAnswersAndEquationsDivision(formationSize);
                break;

            case 3: //Multiplication
                GenerateListOfEnemyAnswersAndEquationsMultiplication(formationSize);
                break;

            default:
                Debug.Log("No Equation Type Selected STR compare was to: " + EquationType);
                break;
        }
    }

    public void GenerateListOfEnemyAnswersAndEquationsAddition(int formationSize)
    {
        // Variable Definitions
        int randomValue1 = 0;
        int randomValue2 = 0;
        int answer = 0;
        string equation;
        bool duplicateFound = false;
        EnemyList = new List<float>();
        EquationsList = new List<string>();

        // Equation Generation Loop
        for (int i = 0; i < formationSize; i++)
        {
            randomValue1 = Random.Range(MinOperandValue, MaxOperandValue);
            randomValue2 = Random.Range(MinOperandValue, MaxOperandValue);
            answer = randomValue1 + randomValue2;
            equation = randomValue1 + " + " + randomValue2 + " = ?";
            if (!((MaxOperandValue - MinOperandValue * 2) <= formationSize))
            {
                // Check if number is already in list 
                for (int j = 0; j < EnemyList.Count; j++)
                {
                    if (EnemyList.Contains(answer))
                        duplicateFound = true;
                }
            }

            // Check duplicate flag if true generate a new number else add number to list
            if (duplicateFound)
            {
                i--;
                duplicateFound = false;
            }
            else
            {
                EnemyList.Add(answer);
                EquationsList.Add(equation);
            }

        }
        
    }

    public void GenerateListOfEnemyAnswersAndEquationsSubtraction(int formationSize)
    {
        // Variable Definitions
        int randomValue1 = 0;
        int randomValue2 = 0;
        int answer = 0;
        string equation;
        bool duplicateFound = false;
        EnemyList = new List<float>();
        EquationsList = new List<string>();

        // Equation Generation Loop
        for (int i = 0; i < formationSize; i++)
        {
            randomValue1 = Random.Range(MinOperandValue, MaxOperandValue + 1);
            randomValue2 = Random.Range(MinOperandValue, randomValue1 + 1);
            answer = randomValue1 - randomValue2;
            equation = randomValue1 + " - " + randomValue2 + " = ?";
            if (!(MaxOperandValue - MinOperandValue <= formationSize))
            {
                // Check if number is already in list 
                for (int j = 0; j < EnemyList.Count; j++)
                {
                    if (EnemyList.Contains(answer))
                        duplicateFound = true;
                }
            }

            // Check duplicate flag if true generate a new number else add number to list
            if (duplicateFound)
            {
                i--;
                duplicateFound = false;
            }
            else
            {
                EnemyList.Add(answer);
                EquationsList.Add(equation);
            }

        }

    }

    public void GenerateListOfEnemyAnswersAndEquationsDivision(int formationSize)
    {
        // Variable Definitions
        int randomValue1 = 0;
        int randomValue2 = 0;
        int answer = 0;
        string equation;
        bool duplicateFound = false;
        EnemyList = new List<float>();
        EquationsList = new List<string>();

        // Equation Generation Loop
        for (int i = 0; i < formationSize; i++)
        {
            randomValue1 = Random.Range(MinOperandValue, MaxOperandValue);
            if(randomValue1 == 0)
            { randomValue1 = 1; }

            randomValue2 = Random.Range(MinOperandValue, MaxOperandValue);
            if (randomValue2 == 0)
            { randomValue2 = 1; }

            answer = randomValue2;
            equation = randomValue1 * randomValue2 + " / " + randomValue1 + " = ?";

            if (!(MaxOperandValue - MinOperandValue <= formationSize))
            {
                // Check if number is already in list 
                for (int j = 0; j < EnemyList.Count; j++)
                {
                    if (EnemyList.Contains(answer))
                        duplicateFound = true;
                }
            }

            // Check duplicate flag if true generate a new number else add number to list
            if (duplicateFound)
            {
                i--;
                duplicateFound = false;
            }
            else
            {
                EnemyList.Add(answer);
                EquationsList.Add(equation);
            }

        }

    }

    public void GenerateListOfEnemyAnswersAndEquationsMultiplication(int formationSize)
    {
        // Variable Definitions
        int randomValue1 = 0;
        int randomValue2 = 0;
        int answer = 0;
        string equation;
        bool duplicateFound = false;
        EnemyList = new List<float>();
        EquationsList = new List<string>();

        // Equation Generation Loop
        for (int i = 0; i < formationSize; i++)
        {
            randomValue1 = Random.Range(MinOperandValue, MaxOperandValue);
            randomValue2 = Random.Range(MinOperandValue, MaxOperandValue);
            answer = randomValue1 * randomValue2;
            equation = randomValue1  + " * " + randomValue2 + " = ?";

            if (!(((MaxOperandValue - MinOperandValue) * (MaxOperandValue - MinOperandValue)) <= formationSize))
            {
                // Check if number is already in list 
                for (int j = 0; j < EnemyList.Count; j++)
                {
                    if (EnemyList.Contains(answer))
                        duplicateFound = true;
                }
            }

            // Check duplicate flag if true generate a new number else add number to list
            if (duplicateFound)
            {
                i--;
                duplicateFound = false;
            }
            else
            {
                EnemyList.Add(answer);
                EquationsList.Add(equation);
            }

        }

    }
}
