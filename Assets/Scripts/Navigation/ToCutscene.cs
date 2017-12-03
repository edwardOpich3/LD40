using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToCutscene : MonoBehaviour
{
	public Button myButton;

	// Use this for initialization
	void Start ()
	{
		myButton = GetComponent<Button>();
		myButton.onClick.AddListener(Navigation);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void Navigation()
	{
		SceneManager.LoadScene("Cutscene");
	}
}
