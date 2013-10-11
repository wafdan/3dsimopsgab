using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {
	
	public Texture2D background;
	public string Satuan;
	public string Password;
	public bool notif = true;

	// Use this for initialization
	void Start () {
		Satuan = PlayerPrefs.GetString("satuan");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		
		//GUI.color = Color.red;
		//GUI.backgroundColor = Color.blue;
		
		int width = Screen.width;
		int height = Screen.height;
		int wBox = 250;
		int hBox = 280;
		int cornerBox_X = (width - wBox)/2;
		int cornerBox_Y = (height - hBox)/2;
		
		GUIStyle style = new GUIStyle();
		style.normal.background = background;
		
		GUI.Box (new Rect(0,0,width,height),"",style);
		GUI.skin.box.normal.textColor = Color.red;
		GUI.skin.label.normal.textColor = Color.white;
		GUI.backgroundColor = Color.yellow;
		//GUI.Label (new Rect(width-420, 35, 300,22), "Sekolah Staff dan Komando Angkatan Darat");
		//create the bounding box for menu
		GUI.Box( new Rect( cornerBox_X ,cornerBox_Y,wBox,hBox - 130), ":: Login ::");
		
		/* if( GUI.Button( new Rect( cornerBox_X + 5, cornerBox_Y + 30, wBox-10,20), "Mulai")){
			Application.LoadLevel("SkenarioH-9");
		}
		
		if( GUI.Button( new Rect( cornerBox_X + 5, cornerBox_Y + 2*30, wBox-10,20), "Opsi")){
			Application.LoadLevel("Layar Opsi");
		}
		
		if( GUI.Button( new Rect( cornerBox_X + 5, cornerBox_Y + 3*30, wBox-10,20), "Credits")){
			Application.LoadLevel("Layar Credits");
		}
		
		if( GUI.Button( new Rect( cornerBox_X + 5, cornerBox_Y + 4*30, wBox-10,20), "Keluar")){
			Application.Quit();
		} */
		
		GUI.Label (new Rect (cornerBox_X + 40, cornerBox_Y + 40, wBox-120,20), "Satuan : ");
		Satuan = GUI.TextField(new Rect (cornerBox_X + 100, cornerBox_Y + 40, wBox-120,20), Satuan, 25);
		PlayerPrefs.SetString("satuan", Satuan);
		
		GUI.Label (new Rect (cornerBox_X + 25, cornerBox_Y + 70, wBox-120,20), "Password : ");
		Password = GUI.PasswordField(new Rect (cornerBox_X + 100, cornerBox_Y + 70, wBox-120,20), Password, "*"[0], 25);
		
		if (GUI.Button( new Rect( cornerBox_X + 122, cornerBox_Y + 100, wBox-205,20), "Ok")){
			notif = !notif;
			if (Password == "admin" && Satuan != ""){
                HistoryManager.showHistory = false;
				Application.LoadLevel("Game play");
			}
		}
		if (!notif){
			if (Satuan == ""){
				GUI.Label(new Rect(cornerBox_X + 20, cornerBox_Y - 30, 400,20), "Periksa kembali nama satuan anda!");
			}
			else if (Password !="admin"){
				GUI.Label(new Rect(cornerBox_X + 20, cornerBox_Y - 30, 400,20), "Periksa kembali password anda!");
			}
		}
						
		if (GUI.Button( new Rect( cornerBox_X + 170, cornerBox_Y + 100, wBox-195,20), "Hapus")){
			Satuan = "";
			Password = "";
		}
		
		if( GUI.Button( new Rect( width-80, 35, wBox-180,22), "Keluar")){
			Application.Quit();
		}
	}
}
