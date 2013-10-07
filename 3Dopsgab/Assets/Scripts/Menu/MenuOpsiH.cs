using UnityEngine;
using System.Collections;

public class MenuOpsiH : MonoBehaviour {
	
	public Texture2D background;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		
		//GUI.color = Color.red;
		GUI.backgroundColor = Color.blue;
		
		int width = Screen.width;
		int height = Screen.height;
		int wBox = 200;
		int hBox = 180;
		int cornerBox_X = (width - wBox)/2;
		int cornerBox_Y = (height - hBox)/2;
		
		GUIStyle style = new GUIStyle();
		style.normal.background = background;
		
		GUI.Box (new Rect(0,0,width,height),"",style);
		GUI.skin.box.normal.textColor = Color.red;
		//create the bounding box for menu
		GUI.Label( new Rect( cornerBox_X + 10 ,cornerBox_Y,wBox + 100,hBox - 155), "Simpan dan lanjutkan kegiatan hari berikutnya?");
		
		if( GUI.Button( new Rect( cornerBox_X + 195, cornerBox_Y + 30, wBox-150,20), "Ya")){
			Application.LoadLevel("TFG Seskoad");
		}
		
		if( GUI.Button( new Rect( cornerBox_X + 250, cornerBox_Y + 30, wBox-150,20), "Tidak")){
			
		}
	}
}
