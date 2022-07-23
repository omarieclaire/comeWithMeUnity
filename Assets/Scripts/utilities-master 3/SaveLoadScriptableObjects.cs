using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadScriptableObjects : MonoBehaviour
{
	
	[Header("Meta")]
	public string persisterName;
	public bool usePersistency = true;
	[Header("Scriptable Objects")]
	public List<ScriptableObject> objectsToPersist;
	public StringEvent OutputJSON;
	
	public void Load()
	{
		for (int i = 0; i < objectsToPersist.Count; i++) 
		{
			if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i )))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i ), FileMode.Open);
				JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file),objectsToPersist[i]);
				//Debug.Log(objectsToPersist[i].name);
				file.Close();
				
			} else 
			{
				//Do Nothing
			}
		}
	}
	
	public void Save( List<ScriptableObject> input){
		//Debug.Log("saving ");
		for (int i = 0; i < input.Count; i++) 
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i ));
			var json = JsonUtility.ToJson(input[i]);
			//Debug.Log(json);
			bf.Serialize(file, json);
			file.Close();
		}
	}
	protected void OnDisable()
	{
		
		Save(objectsToPersist);
	}
	
	//void OnApplicationPause(){
	//	Save(objectsToPersist);
	//}
	
	void OnAppliationQuit(){
		Save(objectsToPersist);
	}
	
	public void DeleteProgress(){
		for (int i = 0; i < objectsToPersist.Count; i++) 
		{
			File.Delete(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, i));
		}
	}
}
