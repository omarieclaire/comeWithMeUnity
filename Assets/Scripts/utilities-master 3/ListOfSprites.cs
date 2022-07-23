using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfSprites : MonoBehaviour
{
	public List<Sprite> Sprites{
		get{return sprites;}set{sprites = value;}
	}
	[SerializeField]
	private List<Sprite> sprites;
	public SpriteEvent OutputSprite;
	public IntEvent OutputCountEvent;
	public void IterateList(int input){
		input = Mathf.Clamp(input,0,Sprites.Count-1);
		Sprite output = Sprites[input];
		OutputSprite.Invoke(output);
	}
	public void OutputCount(){
		int output = Sprites.Count;
		OutputCountEvent.Invoke(output);
	}
	public void GetSpriteFromName(string input){
		Sprite output = Sprites.Find(s=>s.name == input);
		if(output!=null){
			OutputSprite.Invoke(output);
		}
	}
	public SpriteListEvent OutputListEvent;
	public void OutputList(){
		OutputListEvent.Invoke(Sprites);
	}
}
