using UnityEngine;
using System.Collections;

public class Intermission : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		
		GUI.backgroundColor = Color.blue;
		
		int width = Screen.width;
		int height = Screen.height;
		int wBox = width*2/3;
		int hBox = 20;
		int cornerBox_X = (width - wBox)/2;
		int cornerBox_Y = (height - hBox)/2;
		
		GUI.Box(new Rect((width - wBox)/2, 0, wBox,hBox),"Intermission Phase");
		
		if(GUI.Button (new Rect( width - 100, height - 30, 100,30),"Proceed")){
			//Load to the main game scene
			Application.LoadLevel ("GameScene");
		}
	}
}
