using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class MenuUnit : MonoBehaviour
{

    //tambah kegiatan
    public Texture2D background;
    public string Lokasi;
    public string NamaKeg;
    public string FilePendukung;
    public string Deskripsi;

    //alusista
    private bool showHUDTop = true;
    private bool showFormKegiatan = false; // hanya true kalau mau nambah atw edit kegiatan saja
    private bool showPlayMode = false; // play mode, bisa diakses semua unit
    private bool showHari = false;
    private bool showAlutsista = false;
    private bool showLaut = false;
    private bool showPersonel = false;
    private bool showUdara = true; // defaultnya true saja lah
    private bool showDarat = false;

    //scrolling hari
    public Vector2 scrollPosition = Vector2.zero;
    private ComboBox kegiatanList;
    private int pilihKegiatan;
    private GUIContent[] kegiatan;
    private string ketSatuan;

    GUIContent[] Hari;
    private ComboBox comboBoxControl = new ComboBox();
    private GUIStyle listStyle = new GUIStyle();

    //drag unit ke peta
    private BuildingPlacement buildingPlacement;
    public GameObject[] buildings;

    private bool gamePaused = false;

    //list kegiatan/operasi
    private float addKegX = 80;
    private float addKegY = 5;
    private float addKegW = 240;
    private float addKegH = 25;
    private float kegListW = 240;
    private float kegListH = Screen.height * 0.5f;
    private Vector2 scrollPosKegList = Vector2.zero;
    private float kegScrollvH = Screen.height * 0.5f;

    //textures
    //udara
    Texture2D Sukhoi;
    Texture2D F16;
    Texture2D F5;
    Texture2D Hawk;
    Texture2D TU16;
    Texture2D B737;
    Texture2D C130;
    Texture2D C212;
    Texture2D CN235;
    Texture2D NAS332;
    Texture2D EC120B;
    Texture2D SA330;
    Texture2D Bell412;

    //laut
    Texture2D KRIayani;
    Texture2D KRIabdulhalim;
    Texture2D KRIfatahilah;
    Texture2D KRIpulaurempang;
    Texture2D KRIpulaurenggat;
    Texture2D KRItelukpenyu;
    Texture2D KRItelukende;
    Texture2D KRItelukbanten;
    Texture2D KRImakasar;
    Texture2D KRIsurabaya;
    Texture2D KRInanggala;
    Texture2D KRIcakra;
    Texture2D KRIteukuumar;
    Texture2D KRIcutnyakdien;

    //darat
    Texture2D leopard;
    Texture2D scorpion;
    Texture2D amx13;
    Texture2D anoa;
    Texture2D amfibi;

    //personel
    Texture2D infanteri;

    //alutsista lain
    Texture2D arhanud;
    Texture2D radar;
    Texture2D howitzer;

    //Unit manager
    private UnitManager unitManager;
    private GameObject unitManagerObject;

    public bool tampil = false;

    public Rect winRect;   //windows basic rect
    public string location;
    private Vector2 scrollPosition1;

    private string[] strs;  //record the special level's selection
    private int index;
    private string path;    //this is the selected file's full name

    public GUIStyle fileStyle;                      //if the item is a file use this style
    public GUIStyle dirStyle;                       //if the item is a directory use this style

    public string filter;                           //filter of file select

    public Texture2D fileTexture;                   //the file texture
    public Texture2D dirTexture;                    //the directory texture


    void Start()
    {
        unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
        unitManager = unitManagerObject.GetComponent<UnitManager>();

        buildingPlacement = GetComponent<BuildingPlacement>();

        ketSatuan = PlayerPrefs.GetString("satuan");
        //satuan
        Hari = new GUIContent[32];
        Hari[0] = new GUIContent("H - 17");
        Hari[1] = new GUIContent("H - 16");
        Hari[2] = new GUIContent("H - 15");
        Hari[3] = new GUIContent("H - 14");
        Hari[4] = new GUIContent("H - 13");
        Hari[5] = new GUIContent("H - 12");
        Hari[6] = new GUIContent("H - 11");
        Hari[7] = new GUIContent("H - 10");
        Hari[8] = new GUIContent("H - 9");
        Hari[9] = new GUIContent("H - 8");
        Hari[10] = new GUIContent("H - 7");
        Hari[11] = new GUIContent("H - 6");
        Hari[12] = new GUIContent("H - 5");
        Hari[13] = new GUIContent("H - 4");
        Hari[14] = new GUIContent("H - 3");
        Hari[15] = new GUIContent("H - 2");
        Hari[16] = new GUIContent("H - 1");
        Hari[17] = new GUIContent("Hari H");
        Hari[18] = new GUIContent("H + 1");
        Hari[19] = new GUIContent("H + 2");
        Hari[20] = new GUIContent("H + 3");
        Hari[21] = new GUIContent("H + 4");
        Hari[22] = new GUIContent("H + 5");
        Hari[23] = new GUIContent("H + 6");
        Hari[24] = new GUIContent("H + 7");
        Hari[25] = new GUIContent("H + 8");
        Hari[26] = new GUIContent("H + 9");
        Hari[27] = new GUIContent("H + 10");
        Hari[28] = new GUIContent("H + 11");
        Hari[29] = new GUIContent("H + 12");
        Hari[30] = new GUIContent("H + 13");
        Hari[31] = new GUIContent("H + 14");

        listStyle.normal.textColor = Color.white;
        listStyle.onHover.background =
        listStyle.hover.background = new Texture2D(2, 2);
        listStyle.padding.left =
        listStyle.padding.right =
        listStyle.padding.top =
        listStyle.padding.bottom = 6;

        initTextures();
    }

    private void initTextures()
    {
        //udara
        Sukhoi = (Texture2D)Resources.Load("Sukhoi");
        F16 = (Texture2D)Resources.Load("F16");
        F5 = (Texture2D)Resources.Load("F-5");
        Hawk = (Texture2D)Resources.Load("Hawk");
        TU16 = (Texture2D)Resources.Load("TU-16");
        B737 = (Texture2D)Resources.Load("Boeing-737");
        C130 = (Texture2D)Resources.Load("C-130");
        C212 = (Texture2D)Resources.Load("C-212");
        CN235 = (Texture2D)Resources.Load("CN235");
        NAS332 = (Texture2D)Resources.Load("NAS-332");
        EC120B = (Texture2D)Resources.Load("EC120B");
        SA330 = (Texture2D)Resources.Load("SA-330");
        Bell412 = (Texture2D)Resources.Load("Bell-412");

        //laut
        KRIayani = (Texture2D)Resources.Load("KRIayani");
        KRIabdulhalim = (Texture2D)Resources.Load("KRIAbdulhalim");
        KRIfatahilah = (Texture2D)Resources.Load("KRIfatahilah");
        KRIpulaurempang = (Texture2D)Resources.Load("KRIpulaurempang");
        KRIpulaurenggat = (Texture2D)Resources.Load("KRIpulaurenggat");
        KRItelukpenyu = (Texture2D)Resources.Load("KRItelukpenyu");
        KRItelukende = (Texture2D)Resources.Load("KRItelukende");
        KRItelukbanten = (Texture2D)Resources.Load("KRItelukbanten");
        KRImakasar = (Texture2D)Resources.Load("KRImakasar");
        KRIsurabaya = (Texture2D)Resources.Load("KRIsurabaya");
        KRInanggala = (Texture2D)Resources.Load("KRInanggala");
        KRIcakra = (Texture2D)Resources.Load("KRIcakra");
        KRIteukuumar = (Texture2D)Resources.Load("KRIteukuumar");
        KRIcutnyakdien = (Texture2D)Resources.Load("KRIcutnyakdien");

        //darat
        leopard = (Texture2D)Resources.Load("Leopard");
        scorpion = (Texture2D)Resources.Load("Scorpion");
        amx13 = (Texture2D)Resources.Load("AMX-13");
        anoa = (Texture2D)Resources.Load("APS-3 Anoa");
        amfibi = (Texture2D)Resources.Load("Amfibi");

        //personel
        infanteri = (Texture2D)Resources.Load("infanteri");

        //alutsista lain
        arhanud = (Texture2D)Resources.Load("Arhanud");
        radar = (Texture2D)Resources.Load("Radar");
        howitzer = (Texture2D)Resources.Load("Howitzer");
    }

    void Awake()
    {
        location = "C:\\.*" + path;
        strs = new string[20];
        index = 0;
        path = location;
    }


    void Update()
    {
        if (!showPlayMode)
        {
            //iTween.MoveTo(Camera.main.gameObject, iTween.Hash("path", iTweenPath.GetPath("playPath"), "time", 20, "easetype", iTween.EaseType.linear));
        }
        if (Input.GetButtonDown("Jump"))
            showHUDTop = showHUDTop ? false : true;
        if (Input.GetKeyDown(KeyCode.Escape))
            gamePaused = gamePaused ? false : true;
        if (gamePaused)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;



        showHUDTop = !showPlayMode; //kalo lagi play kegiatan, GUI lain Hide semua.


    }

    private IEnumerator moveCamera(string p)
    {
        iTween.MoveUpdate(Camera.main.gameObject, iTween.Hash("path", iTweenPath.GetPath(p), "time", 10));
        yield return new WaitForSeconds(1);
    }

    private int lastOpCount = 0;

    public GUIStyle playLabelStyle; //DIATUR di Editor
    private OperationItem curOpItem;
    private string curOpInfo;
    private string submitKegInfo;
    private bool editUnitMode;
    int width = Screen.width;
    int height = Screen.height;

    //Time box
    int timeBoxWidth = 100;
    int timeBoxHeight = 65;

    //menu kegiatan
    int kegiatanW = 200;
    int kegiatanH = 30;
    public static bool testMovementMode = false;
    public static int GA_NGEDIT = -1; //index -1 utk menandakan bahwa form kegiatan ga lagi dipake ngedit
    private int nowEditingOpId = GA_NGEDIT; //index operation item yg sedang diedit di form kegiatan

    void OnGUI()
    {
        if (showPlayMode) { getPlayKegiatanGUI(); } // ini spesial, bisa mendahului showHUDTop
        if (!showHUDTop) return; //kalo showHUDTop false berarti Hide semua GUI (kecuali khusus play mode di atas)

        GUI.backgroundColor = Color.yellow;
        GUI.skin.box.normal.textColor = Color.red;

        if (!editUnitMode)
        {
            getManajemenKegiatanGUI();
            //GUI.Box(new Rect(100, 0, 50, 20), Hari[selectedItemIndex].text);

            //getMilitaryUnitGUI(); Ditampilin pas Edit Unit mode aja

        }// endif hudtop

        if (editUnitMode)
        {
            float groupW = Screen.width;
            float groupH = 50;
            float groupX = 0;//Screen.width/2 - groupW / 2;
            float groupY = 0;

            float btW = 100;
            float btH = 40;
            float btX = Screen.width / 2 - btW / 2;
            float btY = 40;

            GUI.BeginGroup(new Rect(groupX, groupY, groupW, groupH));
            if (GUI.Button(new Rect(0, 0, btW, btH), (!testMovementMode ? "Tes Eksekusi" : "Berhenti")))
            {
                testMovementMode = !testMovementMode;
            }
            if (GUI.Button(new Rect(btW + 1, 0, btW * 2, btH), "Kembali ke Form Kegiatan"))
            {
                //showHUDTop = !showHUDTop; <-- sakral, cuma Update aja yg bisa ngubah ini, yg lain ga boleh
                HistoryManager.showHistory = false;
                editUnitMode = !editUnitMode;
            }
            if (GUI.Button(new Rect(btW * 3 + 1, 0, btW * 2, btH), (Camera.main.orthographic == true) ? "Kamera Perspektif" : "Kamera Ortogonal"))
            {
                Camera.main.orthographic = !Camera.main.orthographic;
            }
            GUI.EndGroup();

            getMilitaryUnitGUI();
        }

        if (gamePaused)
        {
            int wPausedMenu = 180;
            int hPausedMenu = 100;
            int cornXPausedMenu = (width - wPausedMenu) / 2;
            int cornYPausedMenu = (height - hPausedMenu) / 2;
            GUI.Box(new Rect((width - wPausedMenu) / 2, (height - hPausedMenu) / 2, wPausedMenu, hPausedMenu), "Menu Simulasi");

            if (GUI.Button(new Rect(cornXPausedMenu + 45, cornYPausedMenu + 25, 90, 30), "Kembali"))
            {
                gamePaused = false;
            }
            if (GUI.Button(new Rect(cornXPausedMenu + 45, cornYPausedMenu + 60, 90, 30), "Keluar"))
            {
                Application.LoadLevel("TFG Seskoad");
            }
        }
    }

    private void getPlayKegiatanGUI()
    {
        float boxH = 300;
        float boxW = 400;
        float boxX = Screen.width / 2 - boxW / 2;
        float boxY = Screen.height / 2 - boxH / 2;
        //button, relative to box
        float btW = boxW / 3;
        float btH = 20;
        float btX = boxW / 2 - btW / 2;
        float btY = boxH - btH - 2;

        GUI.BeginGroup(new Rect(boxX, boxY, boxW, boxH));
        GUI.Box(new Rect(0, 0, boxW, boxH), "");
        GUI.Label(new Rect(0, 0, boxW, boxH), curOpInfo, playLabelStyle);
        if (GUI.Button(new Rect(btX, btY, btW, btH), "Kembali"))
        {
            showPlayMode = !showPlayMode;
            //showHUDTop = !showHUDTop;
        }
        GUI.EndGroup();
    }

    private void getManajemenKegiatanGUI()
    {
        //combobox hari
        int selectedItemIndex = comboBoxControl.GetSelectedItemIndex();
        selectedItemIndex = comboBoxControl.List(new Rect(5, 0, kegiatanW - 130, 30), Hari[selectedItemIndex], Hari, listStyle);

        // tombol tambah kegiatan
        GUI.Box(new Rect(addKegX, addKegY, addKegW, addKegH), "Tambah Kegiatan");
        if (GUI.Button(new Rect(285, 7, 30, 20), "+"))
        {
            //showFormKegiatan = !showFormKegiatan;
            showFormKegiatan = true; // jadi true saja
            nowEditingOpId = GA_NGEDIT;
        }
        // box List kegiatan
        GUI.Box(new Rect(addKegX, addKegY + addKegH + 2, kegListW, kegListH + 20), "List Kegiatan");

        float hisItemX = 0; //relative to scrollview
        float hisItemY = 0; //relative to scrollview
        float hisItemH = 40;
        float hisItemW = kegListW * 0.9f;

        scrollPosKegList = GUI.BeginScrollView(new Rect(addKegX, addKegY + addKegH + 20, kegListW, kegListH), scrollPosKegList, new Rect(0, 0, kegListW * 0.9f, kegScrollvH), false, true);
        for (int i = 0; i < OperationManager.operationList.Count; i++)
        {
            curOpItem = (OperationItem)OperationManager.operationList[i];
            if (curOpItem.posisiHari != Hari[comboBoxControl.GetSelectedItemIndex()].text){
                continue;
            }
            GUI.BeginGroup(new Rect(hisItemX + 10, hisItemY, hisItemW, hisItemH));
            //if (GUI.Button(new Rect(hisItemX+10, hisItemY, hisItemW, hisItemH), OperationManager.operationList[i].ToString()))
            if (GUI.Button(new Rect(0, 0, hisItemW * 0.8f, hisItemH), curOpItem.ToString()))
            {
                nowEditingOpId = i; //set posisi ngedit
                // tampilkan detail info di dalam form kegiatan
                NamaKeg = curOpItem.name;
                Lokasi = curOpItem.location;
                Deskripsi = curOpItem.location;
                //edit item lainnya menyusul...
                
                showFormKegiatan = true; //tampilkan formnya
            }
            if (GUI.Button(new Rect(hisItemW * 0.8f, 0, hisItemW * 0.2f, hisItemH * 0.5f), "play"))
            {
                //OperationManager.playOperation((OperationItem)OperationManager.operationList[i]);
                //curOpItem = (OperationItem)OperationManager.operationList[i];
                //showHUDTop = false;
                showPlayMode = true;
                HistoryManager.showHistory = false;
                curOpInfo = "KEGIATAN: \n" + curOpItem.name + "\nLOKASI: \n" + curOpItem.location + "\nDESKRIPSI: \n" + curOpItem.description;
            }
            if (GUI.Button(new Rect(hisItemW * 0.8f, hisItemH * 0.5f, hisItemW * 0.2f, hisItemH * 0.5f), "hapus"))
            {
                //anda yakin hapus?
            }
            GUI.EndGroup();
            hisItemY += hisItemH + 3;
            kegScrollvH = ((hisItemH + 3) * (i + 1) <= kegScrollvH ? kegScrollvH : (hisItemH + 3) * (i + 1));

        }
        GUI.EndScrollView();
        // penyesuaian posisi scroll bar
        if (lastOpCount != OperationManager.operationList.Count)
        {
            float newScrollPosY = (OperationManager.operationList.Count * (hisItemH + 1) - kegListH);
            scrollPosKegList.y = newScrollPosY >= 0 ? newScrollPosY : scrollPosKegList.y;
            lastOpCount = OperationManager.operationList.Count;
        }

        if (showFormKegiatan)
        {
            //Form tambah kegiatan
            float wBox = 250;
            float hBox = Screen.height * 0.5f;
            float cornerBox_X = (width - wBox);
            float cornerBox_Y = (height - hBox) / 60;

            GUIStyle style = new GUIStyle();
            style.normal.background = background;

            GUI.Box(new Rect(0, 0, width, height), "", style);
            GUI.skin.box.normal.textColor = Color.red;
            GUI.skin.label.normal.textColor = Color.white;
            //GUI.backgroundColor = Color.yellow;
            //GUI.color = Color.red;
            GUI.backgroundColor = Color.blue;

            GUI.Box(new Rect(cornerBox_X, cornerBox_Y, wBox + 20, hBox + 190), ":: Form Kegiatan ::");

            GUI.Label(new Rect(cornerBox_X + 10, cornerBox_Y + 40, wBox - 120, 25), "Nama Kegiatan : ");
            NamaKeg = GUI.TextField(new Rect(cornerBox_X + 110, cornerBox_Y + 40, wBox - 120, 25), NamaKeg, 25);

            GUI.Label(new Rect(cornerBox_X + 60, cornerBox_Y + 70, wBox - 120, 25), "Lokasi : ");
            Lokasi = GUI.TextField(new Rect(cornerBox_X + 110, cornerBox_Y + 70, wBox - 120, 25), Lokasi, 25);

            GUI.Label(new Rect(cornerBox_X + 10, cornerBox_Y + 100, wBox - 120, 25), "Deskripsi : ");
            Deskripsi = GUI.TextArea(new Rect(cornerBox_X + 10, cornerBox_Y + 125, wBox - 20, 120), Deskripsi, 200);

            GUI.Label(new Rect(cornerBox_X + 10, cornerBox_Y + 250, wBox - 120, 25), "File Pendukung : ");
            if (GUI.Button(new Rect(cornerBox_X + 115, cornerBox_Y + 252, wBox - 180, 20), "Upload"))
            {
                tampil = !tampil;
            }
            if (tampil)
            {
                GUI.backgroundColor = Color.white;
                winRect = new Rect(500, 20, 150, 20);
                winRect = GUILayout.Window(1, winRect, DoMyWindow, "Browser");
            }
            GUI.Label(new Rect(cornerBox_X + 10, cornerBox_Y + 280, wBox - 120, 25), "Konfigurasi Unit : ");
            if (GUI.Button(new Rect(cornerBox_X + 115, cornerBox_Y + 280, wBox * 0.5f, 40), "Atur Pergerakan\nUnit"))
            {

                editUnitMode = true;
                HistoryManager.showHistory = true;
            }
            GUI.Label(new Rect(cornerBox_X + 10, cornerBox_Y + 380, wBox, 40), submitKegInfo);
            if (GUI.Button(new Rect(cornerBox_X + 60, cornerBox_Y + 430, wBox - 180, 40), "Simpan"))
            {
                if (NamaKeg == "" || Lokasi == "" || Deskripsi == "")
                {
                    submitKegInfo = "Nama, Lokasi, dan Deskripsi kegiatan \nharus diisi.";
                    return;
                }
                else
                {
                    if (nowEditingOpId == GA_NGEDIT) 
                    {
                        //nambah
                        OperationManager.addToOperationList(new OperationItem(PlayerPrefs.GetString("satuan", ""), Hari[selectedItemIndex].text, NamaKeg, Lokasi, Deskripsi));
                        NamaKeg = "";
                        Lokasi = "";
                        Deskripsi = "";
                        //item lainnya menyusul
                        submitKegInfo = "berhasil disimpan";
                    }
                    else
                    {
                        //ngedit
                        ((OperationItem)OperationManager.operationList[nowEditingOpId]).name = NamaKeg;
                        ((OperationItem)OperationManager.operationList[nowEditingOpId]).location = Lokasi;
                        ((OperationItem)OperationManager.operationList[nowEditingOpId]).description = Deskripsi;
                        NamaKeg = "";
                        Lokasi = "";
                        Deskripsi = "";
                        submitKegInfo = "berhasil diperbaharui";
                        nowEditingOpId = GA_NGEDIT;
                    }
                }

            }
            if (GUI.Button(new Rect(cornerBox_X + 140, cornerBox_Y + 430, wBox - 180, 40), "Batal"))
            {
                showFormKegiatan = false;
                nowEditingOpId = GA_NGEDIT;
            }
        }
        GUI.Label(new Rect(340, 7, 100, 20), ketSatuan);
    }

    private void getMilitaryUnitGUI()
    {
        GUI.backgroundColor = Color.yellow;

        if (GUI.Button(new Rect(30, 490, 70, kegiatanH), "Udara"))
        {
            showUdara = !showUdara;
            showLaut = false;
            showDarat = false;
            showPersonel = false;
            showAlutsista = false;
            //menuKegiatan = true;
        }
        if (GUI.Button(new Rect(100, 490, 70, kegiatanH), "Laut"))
        {
            showLaut = !showLaut;
            showUdara = false;
            showDarat = false;
            showPersonel = false;
            showAlutsista = false;
        }
        if (GUI.Button(new Rect(170, 490, 70, kegiatanH), "Darat"))
        {
            showDarat = !showDarat;
            showUdara = false;
            showLaut = false;
            showPersonel = false;
            showAlutsista = false;
        }
        if (GUI.Button(new Rect(240, 490, 80, kegiatanH), "Personel"))
        {
            showPersonel = !showPersonel;
            showUdara = false;
            showLaut = false;
            showDarat = false;
            showAlutsista = false;
        }
        if (GUI.Button(new Rect(320, 490, 100, kegiatanH), "Alutsista lain"))
        {
            showAlutsista = !showAlutsista;
            showUdara = false;
            showLaut = false;
            showDarat = false;
            showPersonel = false;
        }

        if (showUdara)
        {
            GUI.Box(new Rect(0, 520, width, 100), "");


            GUI.DrawTexture(new Rect(30, 525, 70, 70), Sukhoi);
            if (GUI.Button(new Rect(30, 575, 70, kegiatanH), "Sukhoi"))
            {
                buildingPlacement.SetItem(buildings[0]);
            }


            GUI.DrawTexture(new Rect(107, 525, 70, 70), F16);
            if (GUI.Button(new Rect(107, 575, 70, kegiatanH), "F16"))
            {
                buildingPlacement.SetItem(buildings[1]);
            }


            GUI.DrawTexture(new Rect(184, 525, 70, 70), F5);
            if (GUI.Button(new Rect(184, 575, 70, kegiatanH), "F5"))
            {
                buildingPlacement.SetItem(buildings[2]);
            }


            GUI.DrawTexture(new Rect(261, 525, 70, 70), Hawk);
            if (GUI.Button(new Rect(261, 575, 70, kegiatanH), "Hawk"))
            {
                buildingPlacement.SetItem(buildings[3]);
            }


            GUI.DrawTexture(new Rect(338, 525, 70, 70), TU16);
            if (GUI.Button(new Rect(338, 575, 70, kegiatanH), "TU-16"))
            {
                buildingPlacement.SetItem(buildings[4]);
            }


            GUI.DrawTexture(new Rect(415, 525, 70, 70), B737);
            if (GUI.Button(new Rect(415, 575, 70, kegiatanH), "B-737"))
            {
                buildingPlacement.SetItem(buildings[5]);
            }


            GUI.DrawTexture(new Rect(492, 525, 70, 70), C130);
            if (GUI.Button(new Rect(492, 575, 70, kegiatanH), "C-130"))
            {
                buildingPlacement.SetItem(buildings[6]);
            }


            GUI.DrawTexture(new Rect(569, 525, 70, 70), C212);
            if (GUI.Button(new Rect(569, 575, 70, kegiatanH), "C-212"))
            {
                buildingPlacement.SetItem(buildings[7]);
            }


            GUI.DrawTexture(new Rect(646, 525, 70, 70), CN235);
            if (GUI.Button(new Rect(646, 575, 70, kegiatanH), "CN235"))
            {
                buildingPlacement.SetItem(buildings[8]);
            }

            GUI.DrawTexture(new Rect(723, 525, 70, 70), NAS332);
            if (GUI.Button(new Rect(723, 575, 70, kegiatanH), "NAS-332"))
            {
                buildingPlacement.SetItem(buildings[9]);
            }


            GUI.DrawTexture(new Rect(800, 525, 70, 70), EC120B);
            if (GUI.Button(new Rect(800, 575, 70, kegiatanH), "EC120B"))
            {
                buildingPlacement.SetItem(buildings[10]);
            }


            GUI.DrawTexture(new Rect(877, 525, 70, 70), SA330);
            if (GUI.Button(new Rect(877, 575, 70, kegiatanH), "SA-330"))
            {
                buildingPlacement.SetItem(buildings[11]);
            }


            GUI.DrawTexture(new Rect(954, 525, 70, 70), Bell412);
            if (GUI.Button(new Rect(954, 575, 70, kegiatanH), "Bell-412"))
            {
                buildingPlacement.SetItem(buildings[12]);
            }
        }
        else if (showLaut)
        {
            GUI.Box(new Rect(0, 520, width, 100), "");


            GUI.DrawTexture(new Rect(30, 525, 70, 70), KRIayani);
            if (GUI.Button(new Rect(30, 575, 70, kegiatanH), "AYN-351"))
            {
                buildingPlacement.SetItem(buildings[13]);
            }


            GUI.DrawTexture(new Rect(107, 525, 70, 70), KRIabdulhalim);
            if (GUI.Button(new Rect(107, 575, 70, kegiatanH), "AHP-355"))
            {
                buildingPlacement.SetItem(buildings[14]);
            }


            GUI.DrawTexture(new Rect(184, 525, 70, 70), KRIfatahilah);
            if (GUI.Button(new Rect(184, 575, 70, kegiatanH), "FAT-361"))
            {
                buildingPlacement.SetItem(buildings[15]);
            }


            GUI.DrawTexture(new Rect(261, 525, 70, 70), KRIpulaurempang);
            if (GUI.Button(new Rect(261, 575, 70, kegiatanH), "PRP-729"))
            {
                buildingPlacement.SetItem(buildings[15]);
            }


            GUI.DrawTexture(new Rect(338, 525, 70, 70), KRIpulaurenggat);
            if (GUI.Button(new Rect(338, 575, 70, kegiatanH), "PRE-711"))
            {
                buildingPlacement.SetItem(buildings[16]);
            }


            GUI.DrawTexture(new Rect(415, 525, 70, 70), KRItelukpenyu);
            if (GUI.Button(new Rect(415, 575, 70, kegiatanH), "TPN-315"))
            {
                buildingPlacement.SetItem(buildings[17]);
            }


            GUI.DrawTexture(new Rect(492, 525, 70, 70), KRItelukende);
            if (GUI.Button(new Rect(492, 575, 70, kegiatanH), "TLE-517"))
            {
                buildingPlacement.SetItem(buildings[18]);
            }


            GUI.DrawTexture(new Rect(569, 525, 70, 70), KRItelukbanten);
            if (GUI.Button(new Rect(569, 575, 70, kegiatanH), "TBT-516"))
            {
                buildingPlacement.SetItem(buildings[19]);
            }


            GUI.DrawTexture(new Rect(646, 525, 70, 70), KRImakasar);
            if (GUI.Button(new Rect(646, 575, 70, kegiatanH), "MKS-590"))
            {
                buildingPlacement.SetItem(buildings[20]);
            }


            GUI.DrawTexture(new Rect(723, 525, 70, 70), KRIsurabaya);
            if (GUI.Button(new Rect(723, 575, 70, kegiatanH), "SBY-591"))
            {
                buildingPlacement.SetItem(buildings[21]);
            }


            GUI.DrawTexture(new Rect(800, 525, 70, 70), KRInanggala);
            if (GUI.Button(new Rect(800, 575, 70, kegiatanH), "NGL-402"))
            {
                buildingPlacement.SetItem(buildings[22]);
            }


            GUI.DrawTexture(new Rect(877, 525, 70, 70), KRIcakra);
            if (GUI.Button(new Rect(877, 575, 70, kegiatanH), "CKR-401"))
            {
                buildingPlacement.SetItem(buildings[23]);
            }


            GUI.DrawTexture(new Rect(954, 525, 70, 70), KRIteukuumar);
            if (GUI.Button(new Rect(954, 575, 70, kegiatanH), "TMR-385"))
            {
                buildingPlacement.SetItem(buildings[24]);
            }


            GUI.DrawTexture(new Rect(1031, 525, 70, 70), KRIcutnyakdien);
            if (GUI.Button(new Rect(1031, 575, 70, kegiatanH), "CND-375"))
            {
                buildingPlacement.SetItem(buildings[25]);
            }
        }
        else if (showDarat)
        {
            GUI.Box(new Rect(0, 520, width, 100), "");


            GUI.DrawTexture(new Rect(30, 525, 70, 70), leopard);
            if (GUI.Button(new Rect(30, 575, 70, kegiatanH), "Leopard"))
            {
                buildingPlacement.SetItem(buildings[26]);
            }


            GUI.DrawTexture(new Rect(107, 525, 70, 70), scorpion);
            if (GUI.Button(new Rect(107, 575, 70, kegiatanH), "Scorpion"))
            {
                buildingPlacement.SetItem(buildings[27]);
            }


            GUI.DrawTexture(new Rect(184, 525, 70, 70), amx13);
            if (GUI.Button(new Rect(184, 575, 70, kegiatanH), "AMX-13"))
            {
                buildingPlacement.SetItem(buildings[28]);
            }


            GUI.DrawTexture(new Rect(261, 525, 70, 70), anoa);
            if (GUI.Button(new Rect(261, 575, 70, kegiatanH), "Anoa"))
            {
                buildingPlacement.SetItem(buildings[29]);
            }


            GUI.DrawTexture(new Rect(338, 525, 70, 70), amfibi);
            if (GUI.Button(new Rect(338, 575, 70, kegiatanH), "Amfibi"))
            {
                buildingPlacement.SetItem(buildings[30]);
            }
        }
        else if (showPersonel)
        {
            GUI.Box(new Rect(0, 520, width, 100), "");


            GUI.DrawTexture(new Rect(30, 525, 70, 70), infanteri);
            if (GUI.Button(new Rect(30, 575, 70, kegiatanH), "Infanteri"))
            {
                buildingPlacement.SetItem(buildings[34]);
            }
        }

        else if (showAlutsista)
        {
            GUI.Box(new Rect(0, 520, width, 100), "");


            GUI.DrawTexture(new Rect(30, 525, 70, 70), arhanud);
            if (GUI.Button(new Rect(30, 575, 70, kegiatanH), "Arhanud"))
            {
                buildingPlacement.SetItem(buildings[31]);
            }


            GUI.DrawTexture(new Rect(107, 525, 70, 70), radar);
            if (GUI.Button(new Rect(107, 575, 70, kegiatanH), "Radar"))
            {
                buildingPlacement.SetItem(buildings[32]);
            }


            GUI.DrawTexture(new Rect(184, 525, 70, 70), howitzer);
            if (GUI.Button(new Rect(184, 575, 70, kegiatanH), "Howitzer"))
            {
                buildingPlacement.SetItem(buildings[33]);
            }
        }
        else
        {
            GUI.Box(new Rect(0, 520, width, 100), "");
        }
    }


    void DoMyWindow(int windowID)
    {

        OpenFileWindow(location);
        GUI.DragWindow();
    }

    void OpenFileWindow(string location)
    {
        scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1, GUILayout.Width(400), GUILayout.Height(400));
        GUILayout.BeginVertical();
        FileBrowser(location, 0, 0);
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.Label("Selected:" + path);
    }

    void FileBrowser(string location, int spaceNum, int index)
    {
        FileInfo fileSelection;
        DirectoryInfo directoryInfo;
        DirectoryInfo directorySelection;

        //
        fileSelection = new FileInfo(location);
        if (fileSelection.Attributes == FileAttributes.Directory)
            directoryInfo = new DirectoryInfo(location);
        else
            directoryInfo = fileSelection.Directory;

        //
        GUILayout.BeginVertical();
        foreach (DirectoryInfo dirInfo in directoryInfo.GetDirectories())
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceNum);
            GUILayout.Label(dirTexture, dirStyle, GUILayout.Width(12));
            if (GUILayout.Button(dirInfo.Name, dirStyle))
            {
                strs[index] = dirInfo.FullName;
                path = dirInfo.FullName;
            }
            GUILayout.EndHorizontal();
            if (dirInfo.FullName == strs[index] && strs[index] != location)
                FileBrowser(strs[index], spaceNum + 20, index + 1);
        }

        //list the special file with speical style and texture under current directory
        //if( filter=="") filter = "*.*";
        fileSelection = SelectList(directoryInfo.GetFiles(), null, fileStyle, fileTexture, spaceNum) as FileInfo;
        if (fileSelection != null)
            path = fileSelection.FullName;

        GUILayout.EndVertical();
    }

    private object SelectList(ICollection list, object selected, GUIStyle style, Texture image, int spaceNum)
    {
        foreach (object item in list)
        {
            //just show the name of directory and file
            FileSystemInfo info = item as FileSystemInfo;
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceNum);
            GUILayout.Label(image, style, GUILayout.Width(12));
            if (GUILayout.Button(info.Name, style))
            {
                selected = item;
            }
            GUILayout.EndHorizontal();
        }
        return selected;
    }
}