using System.IO;
using UnityEngine;


public static class CountFiles
{
	public static int Count(string directory){
		string saveLocation = directory;
		DirectoryInfo dir = new DirectoryInfo (saveLocation);
		int count = 0; 
		count = dir.GetFiles().Length;

		
		return count;
	}
}
