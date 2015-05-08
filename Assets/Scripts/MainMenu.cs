using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private Text CameraBtnText;
	private GameManager gameManager;

	private string[] cameraBtnText = {"Camera: top view", "Camera: from 3rd person", "Camera: from 1st person"};
		
	void Awake()
	{
		gameManager = GameManager.Instance;
		CameraBtnText = GameObject.FindWithTag ("CameraBtn").GetComponentInChildren<Text> ();
		CameraBtnText.text = cameraBtnText[(int)gameManager.curTCamera];
	}

	public void onClickPlayBtn(string scene)
	{
		if(gameManager.Pause)
		{
			gameManager.Pause = false;
			Destroy (transform.parent.gameObject);
		}
		else{
			Application.LoadLevel (scene);
		}
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
				gameManager.curTCamera = typeCamera.FIRST_PERSON;
				CameraBtnText.text = "Camera: from 1st person";
				break;

			case typeCamera.FIRST_PERSON:
				gameManager.curTCamera = typeCamera.TOP_VIEW;
				CameraBtnText.text = "Camera: top view";
				break;
		}
		GameManager.Instance.onEventChangeCameraType ();
	}

	public void onClickExitBtn()
	{
		Application.Quit ();
	}
}
	