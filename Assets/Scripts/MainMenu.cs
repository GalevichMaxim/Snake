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
		CameraBtnText.text = "Camera: top view";
		gameManager.curTCamera = typeCamera.TOP_VIEW;
	}

	public void onClickPlayBtn(string scene)
	{
		Application.LoadLevel (scene);
	}

	public void onClickCameraBtn()
	{
		switch (gameManager.curTCamera) 
		{
			case typeCamera.TOP_VIEW:
				gameManager.curTCamera = typeCamera.THIRD_PERSON;
				CameraBtnText.text = "Camera: from 3rd person";
				break;

			case typeCamera.THIRD_PERSON:
				gameManager.curTCamera = typeCamera.TOP_VIEW;
				CameraBtnText.text = "Camera: top view";
				break;
		}
	}

	public void onClickExitBtn()
	{
		Application.Quit ();
	}
}
	