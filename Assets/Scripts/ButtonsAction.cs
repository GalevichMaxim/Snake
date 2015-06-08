using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// скрипт действий при нажатии на кнопках панели сохранения результатов игры
public class ButtonsAction : MonoBehaviour {

	public GameObject tableScore;      

	private GameObject btnOk;
	private GameObject btnCancel;
	private GameObject inputField;
	private Text text;

	void Awake()
	{
		if(GameManager.Instance.curPlayer != null)
		{
			GameManager.Instance.UpdateSaveScore();
			Destroy (transform.parent.gameObject);
		}
		btnOk = gameObject.transform.FindChild ("BtnOk").gameObject;
		btnCancel = gameObject.transform.FindChild ("BtnCancel").gameObject;
		inputField = gameObject.transform.FindChild ("InputField").gameObject;
		text = gameObject.transform.FindChild ("TextQuestion").GetComponent<Text> ();
	}

	// при отказе от сохранения ( кнопка "No") осуществляется переход в главное меню
	public void OnCancel()
	{
		if(GameManager.Instance.gameOver)
		{
			GameManager.Instance.gameOver = false;
			Application.LoadLevel ("MainMenu");
		}
		else
		{
			Destroy (transform.parent.gameObject);
			GameManager.Instance.BackToMainMenu();
		}
	}

	// при сохранении результатов ( кнопка "Yes") появляется поле для ввода имени
	public void OnYes()
	{
		Destroy (btnOk);
		Destroy (btnCancel);
		inputField.SetActive(true);
		text.text = "Please, type your name:";

	}

	// действие после окончания ввода имени - сохраняется результат и появляется
	// таблица результатов
	public void OnEndEdit()
	{
		// после окончания ввода имени игрока, зпоминаем текущий счёт и показываем таблицу рейтинга
		string inText = inputField.GetComponent<InputField> ().text;
		if(inText != string.Empty && inText[0] != ' ')
		{
			GameManager.Instance.AddRecordToTableScore(inText);
			ShowTableScore();
		}
	}

	void ShowTableScore()
	{
		Destroy (transform.parent.gameObject); // удаление панели ввода имени игрока
		Instantiate (tableScore);
	}
}
