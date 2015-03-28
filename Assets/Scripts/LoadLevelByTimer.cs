using UnityEngine;
using System.Collections;

public class LoadLevelByTimer : MonoBehaviour {

	public float delay = 3;
	
	public string levelName;
	
	public IEnumerator Start()
	{
		yield return new WaitForSeconds(delay);
		
		Application.LoadLevel(levelName);
	}
}
