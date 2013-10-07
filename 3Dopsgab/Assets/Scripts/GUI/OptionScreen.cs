using UnityEngine;
using System.Collections;

public class OptionScreen : MonoBehaviour {
	public Texture2D background;
	
	private GUIContent[] resolutionCont;
	private ComboBox resolutionList;
	private int selectedResolutionIndex;
	
	private GUIStyle listStyle;
	// Use this for initialization
	void Start () {
		resolutionList = new ComboBox();
		resolutionCont = new GUIContent[3];
		resolutionCont[0] = new GUIContent("800 x 600");
		resolutionCont[1] = new GUIContent("1024 x 768");
		resolutionCont[2] = new GUIContent("1280 x 960");
		
		listStyle = new GUIStyle();
		listStyle.padding.top = 10;
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
		
		GUI.Box (new Rect( (width - wBox)/2, (height-hBox)/2,wBox,hBox), "Pengaturan");
		/*GUI.Label(new Rect(cornerBox_X+5,cornerBox_Y+20,140,20),"Screen Resolution");
		selectedResolutionIndex = resolutionList.GetSelectedItemIndex();
		selectedResolutionIndex = resolutionList.List(new Rect(cornerBox_X + 50, cornerBox_Y+50,100,25), 
							resolutionCont[selectedResolutionIndex].text, resolutionCont,listStyle);
		*/
		if( GUI.Button( new Rect(cornerBox_X + 5, cornerBox_Y + hBox - 25, 80, 20),"Kembali")){
			Application.LoadLevel("TFG Seskoad");	
		}
		
		if( GUI.Button( new Rect(cornerBox_X + wBox - 85, cornerBox_Y + hBox - 25, 80, 20),"Apply")){
			//Save Configuration settings
		}
	}
}
