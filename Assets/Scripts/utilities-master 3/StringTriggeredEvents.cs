using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class StringTriggeredEvents : MonoBehaviour
{
	[System.Serializable]
	public class StringCommand{
		public string CommandName;
		public StringEvent Command;
	}
	
	public List<StringCommand> StringCommands;
	public UnityEvent CommandNotFound;
	
	public void InputCommand(string commandName){
		Debug.Log(commandName);
		if(StringCommands.Find(i => i.CommandName == commandName)!=null){
			StringCommand command = StringCommands.FirstOrDefault(i => i.CommandName == commandName);
			command.Command.Invoke(commandName);
		} else {
			CommandNotFound.Invoke();
		}
	}
}
