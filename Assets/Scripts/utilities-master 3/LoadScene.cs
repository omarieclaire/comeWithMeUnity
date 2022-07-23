using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	public string SceneName{
		get{return _sceneName;}
		set{_sceneName = value;}
	}
	
	[SerializeField]
	private string _sceneName;
	public bool Activate{
		get;set;
	}
	public StringEvent OutputLoadProgressText;
	public FloatEvent OutputLoadProgressFloat;
	public StringEvent OnLoadComplete;
	public void Load(){
		SceneManager.LoadScene(SceneName);
	}
	
	public void LoadAsync(){
		StartCoroutine(AsyncCoroutine(SceneName));
	}
	
	IEnumerator AsyncCoroutine(string scene){

		//Begin to load the Scene you specify
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
		//Don't let the Scene activate until you allow it to
		asyncOperation.allowSceneActivation = false;
		Debug.Log("Pro :" + asyncOperation.progress);
		string text;
		//When the load is still in progress, output the Text and progress bar
		while (!asyncOperation.isDone)
		{
			//Output the current progress
			text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
			OutputLoadProgressText.Invoke(text);
			OutputLoadProgressFloat.Invoke(asyncOperation.progress);
		
			// Check if the load has finished
			if (asyncOperation.progress >= 0.9f)
			{
				//Change the Text to show the Scene is ready
				OnLoadComplete.Invoke(scene +" loaded");
				//Wait to you press the space key to activate the Scene
				if (Activate)
					//Activate the Scene
					asyncOperation.allowSceneActivation = true;
			}

			yield return new WaitForEndOfFrame();
		}
	}
	
	public void Exit(){
		Application.Quit();
	}
}
