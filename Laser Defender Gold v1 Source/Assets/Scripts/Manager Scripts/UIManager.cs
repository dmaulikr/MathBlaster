using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIManager: MonoBehaviour {

    public Dropdown Dropdown;
    public InputField MaxInput;
    public InputField MinInput;

    public void LoadLevel(string name)
    {
        Debug.Log("New Level load: " + name);
        SceneManager.LoadScene(name);
    }

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}

    public void DropDownEquationType()
    {
        GameManager.instance.EquationType = Dropdown.value;

    }

    public void MaxOperandValueInput()
    {
        GameManager.instance.MaxOperandValue = int.Parse(MaxInput.text);
    }

    public void MinOperandValueInput()
    {
        GameManager.instance.MinOperandValue = int.Parse(MinInput.text);
    }



}
