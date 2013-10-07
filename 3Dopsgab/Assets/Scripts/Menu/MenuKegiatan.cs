using UnityEngine;
using System.Collections;
using System;

public class MenuKegiatan : MonoBehaviour {
	
	private bool showHUDTop = true;
	private bool menuKegiatan = true;
	private ComboBox kegiatanList;
	private int pilihKegiatan;
	private GUIContent [] kegiatan;
	
	GUIContent[] kogabKeg;
	GUIContent[] kohanudnasKeg;
	GUIContent[] satuanList;
	private ComboBox comboBoxControl;
	private GUIStyle listStyle;
	
	private BuildingPlacement buildingPlacement;
	public GameObject[] buildings;
	
	private bool gamePaused = false;

	void Start(){
		comboBoxControl = new ComboBox();
		listStyle = new GUIStyle();
		buildingPlacement = GetComponent<BuildingPlacement>();
		
		//satuan
		satuanList = new GUIContent[11];
		satuanList[0] = new GUIContent("KOGAB");
		satuanList[1] = new GUIContent("KOHANUDNAS");
		satuanList[2] = new GUIContent("KOGASGABUD");
		satuanList[3] = new GUIContent("KOGASGABLA");
		satuanList[4] = new GUIContent("KOGASGABFIB");
		satuanList[5] = new GUIContent("KOGASGAB LINUD");
		satuanList[6] = new GUIContent("KOGASGABRAT");
		satuanList[7] = new GUIContent("KOGASGAB RATMIN");
		satuanList[8] = new GUIContent("KOSATGAS TER");
		satuanList[9] = new GUIContent("KOSATGAB INTEL TIS");
		satuanList[10] = new GUIContent("KOSATGAS PASSUS");
		
	    listStyle.normal.textColor = Color.white; 
	    listStyle.onHover.background =
	    listStyle.hover.background = new Texture2D(2, 2);
	    listStyle.padding.left =
	    listStyle.padding.right =
	    listStyle.padding.top =
	    listStyle.padding.bottom = 11;
		
	}
	
	void Update () {
		if(Input.GetButtonDown("Jump"))
			showHUDTop = showHUDTop ? false : true;
		if(Input.GetKeyDown(KeyCode.Escape))
			gamePaused = gamePaused ? false : true;
		if( gamePaused)
			Time.timeScale = 0.0f;
		else
			Time.timeScale = 1.0f;
	}
	
	void OnGUI(){
		GUI.backgroundColor = Color.yellow;
		GUI.skin.box.normal.textColor = Color.red;
		
		int width = Screen.width;
		int height = Screen.height;

		//Time box
		int timeBoxWidth = 100;
		int timeBoxHeight = 65;
		
		//menu kegiatan
		int kegiatanW = 200;
		int kegiatanH = 30;
		
		if(showHUDTop){
			int selectedItemIndex = comboBoxControl.GetSelectedItemIndex();
			selectedItemIndex = comboBoxControl.List(new Rect(width-430,0,kegiatanW-30,30), "Satuan =", satuanList, listStyle);
			GUI.Label(new Rect(width-250, 5, 310, 21), satuanList[selectedItemIndex].text);
			if (satuanList[selectedItemIndex] == satuanList[0]){
				GUI.Box(new Rect(0,0,310,100), "Kegiatan");
				if(GUI.Button(new Rect(2,25,302,30), "# Monitor pelaksanaan kegiatan KOGASGAB")){
					Application.LoadLevelAdditive("Kogab1");
				}
				if(GUI.Button(new Rect(2,55,302,30), "# Pengecekan kesiapan KOGASGAB")){
					Application.LoadLevelAdditive("Kogab2");
				}
			}
			else if (satuanList[selectedItemIndex] == satuanList[1]){
				GUI.Box(new Rect(0,0,162,310), "ALUTSISTA");
				
				Texture2D Sukhoi = (Texture2D)Resources.Load("Sukhoi");
				GUI.DrawTexture(new Rect(5,40,70,70), Sukhoi);
				
				if (GUI.Button(new Rect(5,90,70,kegiatanH), "Sukhoi")){
					buildingPlacement.SetItem(buildings[0]);
				}
				
				Texture2D F16 = (Texture2D)Resources.Load("F16");
				GUI.DrawTexture(new Rect(82,40,70,70), F16);		
				
				if (GUI.Button(new Rect(82,90,70,kegiatanH), "F16")){
					buildingPlacement.SetItem(buildings[1]);
				}
			
				Texture2D Arhanud = (Texture2D)Resources.Load("Arhanud");
				GUI.DrawTexture(new Rect(5,130,70,70), Arhanud);
				
				GUI.Button(new Rect(5,180,70,kegiatanH), "Arhanud");
				
				Texture2D Paskhas = (Texture2D)Resources.Load("Paskhas");
				GUI.DrawTexture(new Rect(82,130,70,70), Paskhas);
				
				GUI.Button(new Rect(82,180,70,kegiatanH), "Paskhas");

				Texture2D Radar = (Texture2D)Resources.Load("Radar");
				GUI.DrawTexture(new Rect(5,220,70,70), Radar);
				
				GUI.Button(new Rect(5,270,70,kegiatanH), "Radar");
				
				Texture2D KRI = (Texture2D)Resources.Load("KRIfatahilah");
				GUI.DrawTexture(new Rect(82,220,70,70), KRI);
				
				GUI.Button(new Rect(82,270,70,kegiatanH), "KRI Fat");
				
				/*Texture2D KRI = (Texture2D)Resources.Load("KRIfatahilah");
				GUI.DrawTexture(new Rect(5,310,70,70), KRI);
				
				GUI.Button(new Rect(5,360,70,kegiatanH), "KRI Fat");
				*/
			}
			else if (satuanList[selectedItemIndex] == satuanList[2]){
				/*GUI.Box(new Rect(0,0,310,70), "Kegiatan");
				if(GUI.Button(new Rect(2,25,302,30), "# Seluruh unsur melaksanakan siaga tempur")){
					Application.LoadLevelAdditive("Kogasgabud1");
				}*/
				GUI.Box(new Rect(0,0,162,310), "ALUTSISTA");
				
				Texture2D ec120b = (Texture2D)Resources.Load("EC120B");
				GUI.DrawTexture(new Rect(5,40,70,70), ec120b);
				
				if (GUI.Button(new Rect(5,90,70,kegiatanH), "EC-120B")){
					buildingPlacement.SetItem(buildings[20]);
				}
				
				Texture2D nas332 = (Texture2D)Resources.Load("NAS-332");
				GUI.DrawTexture(new Rect(82,40,70,70), nas332);		
				
				if (GUI.Button(new Rect(82,90,70,kegiatanH), "NAS332")){
					buildingPlacement.SetItem(buildings[21]);
				}
			
				Texture2D c130 = (Texture2D)Resources.Load("C-130");
				GUI.DrawTexture(new Rect(5,130,70,70), c130);
				
				GUI.Button(new Rect(5,180,70,kegiatanH), "C-130");
				
				Texture2D c212 = (Texture2D)Resources.Load("C-212");
				GUI.DrawTexture(new Rect(82,130,70,70), c212);
				
				GUI.Button(new Rect(82,180,70,kegiatanH), "C-212");
			}
			else if (satuanList[selectedItemIndex] == satuanList[3]){
				GUI.Box(new Rect(0,0,310,130), "Kegiatan");
				if(GUI.Button(new Rect(2,25,302,30), "# Uji jaring komunikasi tahap 1")){
					Application.LoadLevelAdditive("Kogasgabla1");
				}
				if(GUI.Button(new Rect(2,55,302,30), "# Penyapuan ranjau DO	 BOUY5 S.D")){
					Application.LoadLevel("Penyapuan Ranjau");
				}
				if(GUI.Button(new Rect(2,85,302,30), "# Embarkasi dan peran LSBA")){
					Application.LoadLevelAdditive("Kogasgabla3");
				}
			}
			else if (satuanList[selectedItemIndex] == satuanList[4]){
				GUI.Box(new Rect(0,0,310,70), "Kegiatan");
				if(GUI.Button(new Rect(2,25,302,30), "# Pergeseran seluruh material unsur pasrat")){
					Application.LoadLevel("Amfibi Bergerak");
				}
			}
			else if (satuanList[selectedItemIndex] == satuanList[5]){
				GUI.Box(new Rect(0,0,310,70), "Kegiatan");
				if(GUI.Button(new Rect(2,25,302,30), "# Pernyataan siaga 1")){
					Application.LoadLevel("KogasgabLinud1");
				}
			}
			else if (satuanList[selectedItemIndex] == satuanList[6]){
				GUI.Box(new Rect(0,0,310,100), "Kegiatan");
				if(GUI.Button(new Rect(2,25,302,30), "# KI Taipur melaksanakan infiltrasi menuju BPP")){
					Application.LoadLevelAdditive("KogasGabrat1");
				}
				if(GUI.Button(new Rect(2,55,302,30), "# KI mekanis tiba di dermaga ujung SBY")){
					Application.LoadLevelAdditive("KogasGabrat2");
				}
			}
			else if (satuanList[selectedItemIndex] == satuanList[7]){
				GUI.Box(new Rect(0,0,310,70), "Kegiatan");
				if(GUI.Button(new Rect(2,25,302,30), "# Siaga tempur dan pengecekan kesiapan")){
					Application.LoadLevelAdditive("KogasgabRatmin1");
				}
			}
			else if (satuanList[selectedItemIndex] == satuanList[8]){
				GUI.Box(new Rect(0,0,310,160), "Kegiatan");
				if(GUI.Button(new Rect(2,25,302,30), "#	Pembinaan teritorial di wilayah Tarakan")){
					Application.LoadLevelAdditive("KosatgasTer1");
				}
				if(GUI.Button(new Rect(2,55,302,30), "# Pengumpulan keterangan disposisi, komposisi")){
					Application.LoadLevelAdditive("KosatgasTer2");
				}
				if(GUI.Button(new Rect(2,85,302,30), "# Pembinaan teritorial di wilayah Kutim")){
					Application.LoadLevelAdditive("KosatgasTer3");
				}
				if(GUI.Button(new Rect(2,115,302,30), "# Pengecekan personel dan latihan")){
					Application.LoadLevelAdditive("KosatgasTer4");
				}
			}
			else if (satuanList[selectedItemIndex] == satuanList[9]){
				GUI.Box(new Rect(0,0,310,70), "Kegiatan");
				if(GUI.Button(new Rect(2,25,302,30), "# Siaga tempur dan pengecekan kesiapan")){
					Application.LoadLevelAdditive("KosatgasIntelTis1");
				}
			}
			else if (satuanList[selectedItemIndex] == satuanList[10]){
				GUI.Box(new Rect(0,0,310,100), "Kegiatan");
				if(GUI.Button(new Rect(2,25,302,30), "# Pengecekan Kesehatan")){
					Application.LoadLevelAdditive("KosatgasPassus1");
				}
				if(GUI.Button(new Rect(2,55,302,30), "# Latihan menembak")){
					Application.LoadLevelAdditive("KosatgasPassus2");
				}
			}
			else {
				Debug.Log ("test");
			}
			//GUI.Box(new Rect(width-320,0,kegiatanW,65), "Satuan");
			//GUI.Label(new Rect(width-265,30,kegiatanW,kegiatanH), "KOHANUDNAS");
			GUI.Box(new Rect(width-timeBoxWidth,0,timeBoxWidth,timeBoxHeight), "H - 9"); 

			if(GUI.Button(new Rect(width-80,25,60,kegiatanH), "Selesai")){
				//Application.LoadLevel("SkenarioH-8");
				Application.LoadLevelAdditive("Menu Opsi H-9");
			}
		}
		
		if(gamePaused){
			int wPausedMenu = 180;
			int hPausedMenu = 100;
			int cornXPausedMenu = (width - wPausedMenu) /2;
			int cornYPausedMenu = (height - hPausedMenu) / 2;
			GUI.Box(new Rect((width - wPausedMenu)/2, (height-hPausedMenu)/2,wPausedMenu,hPausedMenu),"Menu Simulasi");
			
			if(GUI.Button(new Rect(cornXPausedMenu + 45, cornYPausedMenu + 25,90,30),"Kembali")){
				gamePaused = false;
			}
			if(GUI.Button(new Rect(cornXPausedMenu + 45, cornYPausedMenu + 60,90,30),"Keluar")){
				Application.LoadLevel("TFG Seskoad");
			}
		}
	}
}