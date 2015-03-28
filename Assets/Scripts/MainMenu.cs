using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private Text CameraBtnText;
	private GameManager gameManager;
		
	void Awake()
	{
		gameManager = GameManager.Instance;
		CameraBtnText = GameObject.FindWithTag ("CameraBtn").GetComponentInChildren<Text> ();
		CameraBtnText.text = "Camera: from 3rd person";
		gameManager.curTCamera = typeCamera.THIRD_PERSON;
	}

	public void onClickPlayBtn(string scene)
	{
		Application.LoadLevel (scene);
	}

	public void onClickCameraBtn()
	{
		switch (gameManager.curTCamera) 
		{
			case typeCamera.FIRST_PERSON:
				gameManager.curTCamera = typeCamera.THIRD_PERSON;
				CameraBtnText.text = "Camera: from 3rd person";
				break;

			case typeCamera.THIRD_PERSON:
				gameManager.curTCamera = typeCamera.FIRST_PERSON;
				CameraBtnText.text = "Camera: from 1st person";
				break;
		}
	}

	public void onClickExitBtn()
	{
		Debug.Log ("Quit");
		Application.Quit ();
	}
}
	