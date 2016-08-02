using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreenScore : MonoBehaviour {


	// Grab and Display Final Score
	void Start () {
        this.GetComponent<Text>().text = "Score: " + GameManager.instance.FinalScore.ToString();
    }
}
