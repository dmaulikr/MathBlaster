using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
#if  !(UNITY_ANDROID || UNITY_IOS)
        GameObject.Find("Fire Button").SetActive(false);
#endif
        GameManager.instance.ScoreText = GameObject.Find("Score");
        GameManager.instance.ScoreText.GetComponent<Text>().text = "Score: " + "0";
    }


}
