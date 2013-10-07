using UnityEngine;
using System.Collections;

public class CreditScreen : MonoBehaviour {
	public Texture2D background;
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
		int wBox = 200;
		int hBox = 150;
		int cornerBox_X = (width - wBox)/2;
		int cornerBox_Y = (height - hBox)/2;
		
		GUI.Box( new Rect( cornerBox_X ,cornerBox_Y,wBox - 10,hBox + 30), "Tim");
		GUI.Label( new Rect( cornerBox_X + 9, cornerBox_Y + 25, wBox-10, 40), "AI Programmer : Wafdan");
		GUI.Label( new Rect( cornerBox_X + 9, cornerBox_Y + 2*25, wBox-10, 40), "Network Programmer : Galih");
		GUI.Label( new Rect( cornerBox_X + 9, cornerBox_Y + 3*25, wBox-10, 40), "GUI Programmer : Fery");
		GUI.Label( new Rect( cornerBox_X + 9, cornerBox_Y + 4*25, wBox-10, 40), "3D Artist : Amir");
		GUI.Label( new Rect( cornerBox_X + 9, cornerBox_Y + 5*25, wBox-10, 40), "Quality Control : Dadan");
		if ( GUI.Button ( new Rect( cornerBox_X + 5, cornerBox_Y + 6*25, wBox-20,20), "Kembali ke Menu")){
			Application.LoadLevel ("TFG Seskoad");	
		}
	}
}
