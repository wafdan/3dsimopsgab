using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEditor;

public class MenuUnit : MonoBehaviour
{
    //File Browser
    protected string lokasiUpload = "Assets\\Upload\\";
    protected string m_textPath;
    protected FileBrowser m_fileBrowser;
    [SerializeField]
    protected Texture2D m_directoryImage,
                        m_fileImage;
    private bool showSaveBrowser = false;
    private bool showSaveUnitDialog = false;
    private bool showConfirmDeleteSave = false;
    private bool showPauseMenu = true;
    public static int curOpPlayIdx;

    //tambah kegiatan
    public Texture2D background;
    public string Lokasi;
    public string NamaKeg;
    public string FilePendukung;
    public string Deskripsi;
    public string JamMulai;
    public string MenitMulai;
    public string HariDurasi;
    public string JamDurasi;
    public string MenitDurasi;
    public bool toggleFile = false;
    public bool toggleUnitConfig = false;
    //public bool toggleScene = false;

    //alusista
    private bool showHUDTop = true;
    private bool showFormKegiatan = false; // hanya true kalau mau nambah atw edit kegiatan saja
    public static bool showPlayMode = false; // play mode, bisa diakses semua unit
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
    private ComboBox comboBoxControl;
    private GUIStyle listStyle
    {
        get
        {
            if (m_styleList == null)
            {
                m_styleList = new GUIStyle();
                m_styleList.normal.textColor = Color.white;
                m_styleList.hover.background = new Texture2D(2, 2);
                m_styleList.padding.bottom = 6;
            }
            return m_styleList;
        }
    }
    protected GUIStyle m_styleList;

    //drag unit ke peta
    private BuildingPlacement buildingPlacement;
    public GameObject[] buildings;

    private bool gamePaused = false;

    //list kegiatan/operasi
    private float addKegX = 80;
    private float addKegY = 5;
    private float addKegW = 240;
    private float addKegH = 25;
    private float kegListW = 250;
    private float kegListH = Screen.height * 0.5f;
    private float kegScrollvH = Screen.height * 0.5f;
    private Vector2 scrollPosKegList = Vector2.zero;
    private Vector2 scrollSceneList = Vector2.zero;

    //private GUIStyle styleKegListItem; //diaturnya di editor, eh, di sini aja dink
    protected GUIStyle styleKegListItem
    {
        get
        {
            if (m_kegListItem == null)
            {
                m_kegListItem = new GUIStyle(GUI.skin.button);
                m_kegListItem.alignment = TextAnchor.MiddleLeft;
                m_kegListItem.fixedHeight = GUI.skin.button.fixedHeight;
            }
            return m_kegListItem;
        }
    }
    protected GUIStyle m_kegListItem;

    protected GUIStyle styleKegListItemUnit
    {
        get
        {
            if (m_kegListItem1 == null)
            {
                m_kegListItem1 = new GUIStyle(GUI.skin.button);
                m_kegListItem1.alignment = TextAnchor.MiddleLeft;
                m_kegListItem1.fixedHeight = GUI.skin.button.fixedHeight;
                m_kegListItem1.normal.background = MakeTex(200, 1, Color.red);
            }
            return m_kegListItem1;
        }
    }
    protected GUIStyle m_kegListItem1;

    protected GUIStyle styleKegListItemNoUnit
    {
        get
        {
            if (m_kegListItem2 == null)
            {
                m_kegListItem2 = new GUIStyle(GUI.skin.button);
                m_kegListItem2.alignment = TextAnchor.MiddleLeft;
                m_kegListItem2.fixedHeight = GUI.skin.button.fixedHeight;
                m_kegListItem2.normal.background = MakeTex(200, 1, Color.green);
            }
            return m_kegListItem2;
        }
    }
    protected GUIStyle m_kegListItem2;

    protected GUIStyle styleSaveList
    {
        get
        {
            if (m_kegListSave == null)
            {
                m_kegListSave = new GUIStyle(GUI.skin.box);
                m_kegListSave.alignment = TextAnchor.UpperCenter;
            }
            return m_kegListSave;
        }
    }
    protected GUIStyle m_kegListSave;

    protected GUIStyle styleSaveItemDel
    {
        get
        {
            if (m_saveItemDel == null)
            {
                m_saveItemDel = new GUIStyle(GUI.skin.button);
                m_saveItemDel.alignment = TextAnchor.MiddleCenter;
                m_saveItemDel.fixedWidth = 25;
            }
            return m_saveItemDel;
        }
    }
    protected GUIStyle m_saveItemDel;

    public Font courierFont; //diambil dari editor
    protected GUIStyle stylePlayLabel
    {
        get
        {
            if (m_playLabel == null)
            {
                m_playLabel = new GUIStyle(GUI.skin.label);
                m_playLabel.alignment = TextAnchor.MiddleLeft;
                m_playLabel.font = courierFont;
                m_playLabel.normal.textColor = Color.red;
            }
            return m_playLabel;
        }
    }
    protected GUIStyle m_playLabel;


    protected GUIStyle stylePlayField
    {
        get
        {
            if (m_playField == null)
            {
                m_playField = new GUIStyle(GUI.skin.label);
                m_playField.alignment = TextAnchor.MiddleRight;
                m_playField.font = courierFont;
                m_playField.fixedWidth = 150f;
            }
            return m_playField;
        }
    }
    protected GUIStyle m_playField;

    protected GUIStyle stylePlayBtBack
    {
        get
        {
            if (m_playBtBack == null)
            {
                m_playBtBack = new GUIStyle(GUI.skin.button);
                m_playBtBack.alignment = TextAnchor.MiddleCenter;
                m_playBtBack.fixedWidth = 120;
            }
            return m_playBtBack;
        }
    }
    protected GUIStyle m_playBtBack;


    protected GUIStyle stylePlayDesc
    {
        get
        {
            if (m_stylePlayDesc == null)
            {
                m_stylePlayDesc = new GUIStyle(GUI.skin.box);
                //m_stylePlayDesc.normal.background = new Texture2D(2, 2);
            }
            return m_stylePlayDesc;
        }
    }
    protected GUIStyle m_stylePlayDesc;


    protected GUIStyle styleClockPlay
    {
        get
        {
            if (m_clockPlay == null)
            {
                m_clockPlay = new GUIStyle(GUI.skin.box);
                m_clockPlay.normal.textColor = Color.yellow;
                m_clockPlay.font = courierFont;
                //m_stylePlayDesc.normal.background = new Texture2D(2, 2);
            }
            return m_clockPlay;
        }
    }
    protected GUIStyle m_clockPlay;

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
    public bool list = false;

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

    ///
    private int lastOpCount = 0;

    private OperationItem curOpItem;
    public static OperationItem curOpPlaying;
    private string curOpInfo;
    private string submitKegInfo;
    private string submitUpload;
    [SerializeThis]
    private bool editUnitMode;
    private bool editScene;
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

    private bool hasSaveUnit = false; //udah save pergerakan unit apa belum

    // save game window
    string NamaFileSave = "";
    string filenameToDelete = "";

    // position of description scroll
    Vector2 playGUIkegScrollPos = Vector2.zero;
    private ArrayList sceneNames = new ArrayList();
    private UnitInfo[] unitUdaraList; // buat list menu unit udara
    private UnitInfo[] unitLautList; // buat list menu unit laut
    private UnitInfo[] unitDaratList; // buat list menu unit darat
    private UnitInfo[] unitPersonelList; // buat list menu personel
    private UnitInfo[] unitAlutList; // buat list menu alutsista

    void Start()
    {
        comboBoxControl = new ComboBox();
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

        initTextures();
        initSceneNames();
    }

    private void initSceneNames()
    {
        //ArrayList temp = new ArrayList();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                //temp.Add(name);
                sceneNames.Add(name);
            }
        }
        //return temp.ToArray();
    }

    /*    void Awake()
        {
            location = "C:\\.*" + path;
            strs = new string[20];
            index = 0;
            path = location;
        }
    */
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

    void OnGUI()
    {
        if (m_fileBrowser != null)
        {
            m_fileBrowser.OnGUI();
        }

        if (!gamePaused)
        {
            if (showPlayMode)
            { // ini spesial, bisa mendahului showHUDTop
                getPlayKegiatanGUI(curOpPlaying); 
                return; 
            } 
            //if (!showHUDTop) return; //kalo showHUDTop false berarti Hide semua GUI (kecuali khusus play mode di atas)

            GUI.backgroundColor = Color.yellow;
            GUI.skin.box.normal.textColor = Color.red;

            if (!editUnitMode)
            {
                getManajemenKegiatanGUI();
                //GUI.Box(new Rect(100, 0, 50, 20), Hari[selectedItemIndex].text);

                //getMilitaryUnitGUI(); Ditampilin pas Edit Unit mode aja

            }// endif hudtop
            else
            //if (editUnitMode)
            {
                getUnitControlButtonUI();

                getMilitaryUnitGUI();
            }
        }
        if (gamePaused)
        {
            if (showPauseMenu)
            {
                //save dan load di kanan atas
                //buttons
                float btW = 90;
                float btH = 25;
                float btHplusMargin = btH + 5;

                //box
                float wPausedMenu = 180;
                float hPausedMenu = btHplusMargin * 5;
                float cornXPausedMenu = (Screen.width - wPausedMenu) / 2;
                float cornYPausedMenu = (Screen.height - hPausedMenu) / 2 - 40;
                GUI.Box(new Rect(cornXPausedMenu, cornYPausedMenu, wPausedMenu, hPausedMenu), "Menu Simulasi");

                //buttons again
                float btX = cornXPausedMenu + 45;
                float btY = cornYPausedMenu + 25;

                if (editUnitMode) { GUI.enabled = false; } else { GUI.enabled = true; }
                if (GUI.Button(new Rect(btX, btY, btW, btH), "Save/Load"))
                {
                    showSaveBrowser = true;
                }
                GUI.enabled = true;
                if (GUI.Button(new Rect(btX, btY + btHplusMargin * 2, btW, btH), "Kembali"))
                {
                    gamePaused = false;
                    showSaveBrowser = false;
                }
                if (editUnitMode) { GUI.enabled = false; } else { GUI.enabled = true; }
                if (GUI.Button(new Rect(btX, btY + btHplusMargin * 3, btW, btH), "Keluar"))
                {
                    Application.LoadLevel("TFG Seskoad");
                }
            }//endif gamePaused
        }
        if (gamePaused && showSaveBrowser)
        {
            showPauseMenu = false;
            showSaveWindow();
        }
        else
        {
            showPauseMenu = true;
        }
    }

    private void showSaveWindow()
    {
        if (!showConfirmDeleteSave)
        {
            string[] array2 = Directory.GetFiles(Application.persistentDataPath + "/", "*" + OperationManager.FILE_EXT);
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 300, 400, 400), GUI.skin.box);

            GUILayout.BeginVertical(styleSaveList);

            GUILayout.Label("Nama file save:");

            NamaFileSave = GUILayout.TextField(NamaFileSave);
            if (GUILayout.Button("Save Game"))
            {
                string formattedName = "";
                if (NamaFileSave == "")
                {
                    DateTime When = DateTime.Now;
                    string Name = PlayerPrefs.GetString("satuan");//"Kegiatan";
                    formattedName = string.Format("{0} - {1:yyyy.MM.dd.HH.MM.ss}", Name, When) + OperationManager.FILE_EXT;
                }
                else
                {
                    formattedName = NamaFileSave + OperationManager.FILE_EXT;
                }
                OperationManager.saveGameToFile(formattedName);

            }
            GUILayout.Space(60);

            if (array2.Length > 0)
            {
                GUILayout.Label("File-file hasil save. Untuk Load, klik salah satu.");
                foreach (string sg in array2)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(Path.GetFileNameWithoutExtension(sg)))
                    {
                        LevelSerializer.LoadSavedLevelFromFile(Path.GetFileName(sg));
                        //Time.timeScale = 1;
                    }
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("X", styleSaveItemDel))
                    {
                        showConfirmDeleteSave = true;
                        filenameToDelete = sg;
                    }
                    GUI.backgroundColor = Color.gray;
                    GUILayout.EndHorizontal();

                }
            }
            //for(var sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) { 
            //   if(GUILayout.Button(sg.Caption)) { 
            //     LevelSerializer.LoadNow(sg.Data);
            //     Time.timeScale = 1;
            //     } 
            //} 
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<< Kembali"))
            {
                showSaveBrowser = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndArea();

        }
        if (showConfirmDeleteSave)
        {
            //delete
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), GUI.skin.box);
            GUILayout.BeginVertical();
            GUILayout.Label("Yakin hapus?", styleSaveList);
            GUILayout.Space(30);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Ya"))
            {
                OperationManager.deleteSavedGame(filenameToDelete);
                showConfirmDeleteSave = false;
            }
            if (GUILayout.Button("Batal"))
            {
                showConfirmDeleteSave = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }

    private void getUnitControlButtonUI()
    {
        float groupW = 200; //Screen.width;
        float groupH = 150; //50;
        float groupX = 5;//Screen.width/2 - groupW / 2;
        float groupY = 50;

        float btW = 100;
        float btH = 40;
        float btX = Screen.width / 2 - btW / 2;
        float btY = 40;

        Color collam = GUI.backgroundColor;
        GUILayout.BeginArea(new Rect(groupX, 0, groupW, 40), GUI.skin.box);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Kamera:",GUILayout.Width(50));
        if (GUILayout.Button((Camera.main.orthographic == true) ? "Ortogonal" : "Perspektif"))
        {
            Camera.main.orthographic = !Camera.main.orthographic;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(groupX, groupY, groupW, groupH),GUI.skin.box);
        GUILayout.BeginVertical();
        GUI.backgroundColor = Color.red;
        
        if (GUILayout.Button((!testMovementMode ? "Tes Eksekusi" : "Berhenti")))
        {
            testMovementMode = !testMovementMode;
        }
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Simpan Pergerakan"))
        {
            //nah di sini simpan pergerakan unit

        }
        GUILayout.FlexibleSpace();
        GUI.backgroundColor = Color.white;
        if (GUILayout.RepeatButton("Kembali ke Form Kegiatan"))
        {
            HistoryManager.showHistory = false;
            editUnitMode = !editUnitMode;
        }
        GUI.backgroundColor = Color.grey;
        GUILayout.EndVertical();
        GUILayout.EndArea();
        return;

    }
    //end onGUI

    private void getPlayKegiatanGUI(OperationItem op)
    {

        float clockW = 300;
        float clockH = 80;
        float clockX = (Screen.width-clockW) ;
        float clockY = 0;
        float boxW = 300;
        float boxH = 300;
        float boxX = (Screen.width - boxW);
        float boxY = clockH+2;
        //if (!op.hasUnitMovement)
        //{
        //    boxH = 300;
        //    boxW = 400;
        //    boxX = Screen.width - boxW;
        //    boxY = 0;
        //}
        //button, relative to box
        float btW = boxW / 3;
        float btH = 20;
        float btX = boxW / 2 - btW / 2;
        float btY = boxH - btH - 2;

        GUILayout.BeginArea(new Rect(clockX, clockY, clockW, clockH),GUI.skin.box);
        GUILayout.Label(HistoryManager.gameClockValue,styleClockPlay);
        HistoryManager.timeSpeed = GUILayout.HorizontalSlider(HistoryManager.timeSpeed, 1, 300);
        GUILayout.BeginHorizontal();
        GUILayout.Label("1X",GUILayout.Width(25));
        GUILayout.FlexibleSpace();
        GUILayout.Label("skala waktu 1 : "+ HistoryManager.timeSpeed*10+"");
        GUILayout.FlexibleSpace();
        GUILayout.Label("3000X", GUILayout.Width(40));
        GUILayout.EndHorizontal();
        
        GUILayout.EndArea();
        
        GUILayout.BeginArea(new Rect(boxX, boxY, boxW, boxH), stylePlayDesc);
        GUI.backgroundColor = Color.white;
        GUILayout.BeginVertical(GUI.skin.button);
        //GUILayout.Label(curOpInfo, playLabelStyle);

        //string durasi="";
        //if (curOpPlaying.duration.Days > 0)
        //    durasi += curOpPlaying.duration.Days.ToString() + " hari,";
        //if (curOpPlaying.duration.Hours > 0)
        //    durasi += curOpPlaying.duration.Hours.ToString() + " jam,";
        //if (curOpPlaying.duration.Minutes > 0)
        //    durasi += curOpPlaying.duration.Minutes.ToString() + " menit,";
        //if(durasi!="")
        //    GUILayout.Label(durasi, stylePlayLabel);
        GUILayout.BeginVertical();
        for (int i = 0; i < HistoryManager.nowPlayingList.Count; i++)
        {
            OperationItem opintem = (OperationItem)HistoryManager.nowPlayingList[i];
            GUILayout.BeginHorizontal();
            GUILayout.Label("Nama Kegiatan :", stylePlayField);
            GUILayout.Label(opintem.name, stylePlayLabel);
            
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Waktu :", stylePlayField);
        //GUILayout.Label(curOpPlaying.posisiHari + " " + curOpPlaying.startTime + " sampai " + curOpPlaying.endTime, stylePlayLabel);
        //GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Lokasi :", stylePlayField);
        //GUILayout.Label(curOpPlaying.location, stylePlayLabel);
        //GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Deskripsi :", stylePlayField);
        //playGUIkegScrollPos = GUILayout.BeginScrollView(playGUIkegScrollPos);//, GUILayout.Width(boxW-10), GUILayout.Height(50));
        //GUILayout.Label(curOpPlaying.description, stylePlayLabel);
        //GUILayout.EndScrollView();
        //GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Kembali", stylePlayBtBack))
        {
            showPlayMode = !showPlayMode;
            curOpPlayIdx = 0;
            //showHUDTop = !showHUDTop;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(styleSaveList);
        if (GUILayout.Button("<< Sebelumnya", stylePlayBtBack))
        {
            for (int i = curOpPlayIdx - 1; i >= 0; i--)
            {
                OperationItem opCek = ((OperationItem)OperationManager.operationList[i]);
                if (opCek.satuan == PlayerPrefs.GetString("satuan"))
                {
                    curOpPlaying = opCek;
                    curOpPlayIdx = i;
                    break;
                }
            }
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Selanjutnya >>", stylePlayBtBack))
        {
            for (int i = curOpPlayIdx + 1; i < OperationManager.operationList.Count; i++)
            {
                OperationItem opCek = ((OperationItem)OperationManager.operationList[i]);
                if (opCek.satuan == PlayerPrefs.GetString("satuan"))
                {
                    curOpPlaying = opCek;
                    curOpPlayIdx = i;
                    break;
                }
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
        GUI.backgroundColor = Color.gray;
        //GUI.BeginGroup(new Rect(boxX, boxY, boxW, boxH));
        //GUI.Box(new Rect(0, 0, boxW, boxH), "");
        //GUI.Label(new Rect(0, 0, boxW, boxH), curOpInfo, playLabelStyle);
        //if (GUI.Button(new Rect(btX, btY, btW, btH), "Kembali"))
        //{
        //    showPlayMode = !showPlayMode;
        //    //showHUDTop = !showHUDTop;
        //}
        //GUI.EndGroup();
    }

    private void getManajemenKegiatanGUI()
    {

        float leftGroupY = 0;
        //combobox hari
        int selectedItemIndex = comboBoxControl.GetSelectedItemIndex();
        selectedItemIndex = comboBoxControl.List(new Rect(5, leftGroupY, kegiatanW - 142, 25), Hari[selectedItemIndex], Hari, listStyle);

        GUI.Box(new Rect(addKegX - 10, leftGroupY, addKegW + 30, addKegH), "Kegiatan");
        // tombol tambah kegiatan
        if (GUI.Button(new Rect(275, leftGroupY + 2, 30, 20), "+"))
        {
            //showFormKegiatan = !showFormKegiatan;
            showFormKegiatan = true; // jadi true saja
            emptyTheField();
            nowEditingOpId = GA_NGEDIT;
        }
        //tombol toggle show list kegiatan
        if (GUI.Button(new Rect(307, leftGroupY + 2, 30, 20), "v"))
        {
            list = !list; // jadi true saja
        }
        // box List kegiatan
        if (list)
        {
            //GUI.Box(new Rect(addKegX - 10, leftGroupY + addKegH + 1, kegListW + 5, kegListH - 30), "");

            float hisItemX = 0; //relative to scrollview
            float hisItemY = 0; //relative to scrollview
            float hisItemH = 70;
            float hisItemW = kegListW * 0.9f;
            GUILayout.BeginArea(new Rect(addKegX - 10, leftGroupY + addKegH + 1, kegListW + 35, kegListH - 30), GUI.skin.box);
            //scrollPosKegList = GUI.BeginScrollView(new Rect(addKegX, leftGroupY + addKegH, kegListW + 10, kegListH - 30), scrollPosKegList, new Rect(0, 0, kegListW * 0.9f, kegScrollvH + 60), false, true);
            scrollPosKegList = GUILayout.BeginScrollView(scrollPosKegList);
            GUILayout.BeginVertical();
            for (int i = 0; i < OperationManager.operationList.Count; i++)
            {
                curOpItem = (OperationItem)OperationManager.operationList[i];
                //kalo satuannya bukan satuan yg lagi dimainkan, lewat.
                if (curOpItem.satuan != PlayerPrefs.GetString("satuan"))
                {
                    continue;
                }
                //kalo harinya bukan hari yg lagi dipilih, lewat.
                if (curOpItem.posisiHari != Hari[comboBoxControl.GetSelectedItemIndex()].text)
                {
                    continue;
                }
                if (curOpItem.hasUnitMovement)
                { GUI.backgroundColor = Color.red; }
                else if (curOpItem.hasVideo)
                { GUI.backgroundColor = Color.green; }
                else
                { GUI.backgroundColor = Color.yellow; }
                GUILayout.BeginHorizontal(GUI.skin.box);

                string curin = curOpItem.name +
                    "\nlokasi: " + curOpItem.location +
                    "\nwaktu mulai: " + curOpItem.getStartTimeString() +
                    "\nwaktu selesai: " + curOpItem.getEndTimeString();
                if (GUILayout.Button(curin,styleKegListItem))
                {
                    nowEditingOpId = i; //set posisi ngedit
                    // tampilkan detail info di dalam form kegiatan
                    NamaKeg = curOpItem.name;
                    Lokasi = curOpItem.location;
                    Deskripsi = curOpItem.location;
                    //waktu mulai
                    JamMulai = curOpItem.startTime.Hour.ToString();
                    MenitMulai = curOpItem.startTime.Minute.ToString();
                    //durasi
                    HariDurasi = curOpItem.duration.Days.ToString();
                    JamDurasi = curOpItem.duration.Hours.ToString();
                    MenitDurasi = curOpItem.duration.Minutes.ToString();

                    toggleFile = curOpItem.hasVideo;
                    toggleUnitConfig = curOpItem.hasUnitMovement;

                    showFormKegiatan = true; //tampilkan formnya
                }
                

                GUILayout.BeginVertical(GUILayout.Width(40));
                if (GUILayout.Button("play"))
                {
                    showPlayMode = true;
                    HistoryManager.showHistory = false;
                    curOpPlaying = curOpItem;
                    curOpPlayIdx = i;
                    
                }
                
                if (GUILayout.Button("hapus"))
                {
                    //anda yakin hapus?
                }
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
                hisItemY += hisItemH + 3;
                kegScrollvH = ((hisItemH + 3) * (i + 1) <= kegScrollvH ? kegScrollvH : (hisItemH + 3) * (i + 1));

                GUI.backgroundColor = Color.blue;
            }//endfor
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            // penyesuaian posisi scroll bar
            if (lastOpCount != OperationManager.operationList.Count)
            {
                float newScrollPosY = (OperationManager.operationList.Count * (hisItemH + 1) - kegListH);
                scrollPosKegList.y = newScrollPosY >= 0 ? newScrollPosY : scrollPosKegList.y;
                lastOpCount = OperationManager.operationList.Count;
            }
            GUILayout.EndArea();

        }

        if (showFormKegiatan)
        {
            //Form tambah kegiatan
            float wBox = 250;
            float hBox = Screen.height;
            float cornerBox_X = (Screen.width - wBox) - 10;
            float cornerBox_Y = (Screen.height - hBox) / 60;

            float txtFieldHplusMargin = 30;
            float txtAreaH = 125;
            float posFieldY = cornerBox_Y;//posisi awal Y utk field, diinkremen selalu;
            float timeW = 25;
            float timeH = 25;


            GUIStyle style = new GUIStyle();
            style.normal.background = background;

            GUI.Box(new Rect(0, 0, width, height), "", style);
            GUI.skin.box.normal.textColor = Color.red;
            GUI.skin.label.normal.textColor = Color.white;
            //GUI.backgroundColor = Color.yellow;
            //GUI.color = Color.red;
            GUI.backgroundColor = Color.blue;

            GUI.Box(new Rect(cornerBox_X, cornerBox_Y, wBox + txtFieldHplusMargin, hBox + 100), ":: Form Kegiatan ::");
            posFieldY += txtFieldHplusMargin;

            GUI.Label(new Rect(cornerBox_X + 10, posFieldY, wBox - 120, 25), "Nama Kegiatan : ");
            NamaKeg = GUI.TextField(new Rect(cornerBox_X + 110, posFieldY, wBox - 120, 25), NamaKeg, 25);
            posFieldY += txtFieldHplusMargin;

            GUI.Label(new Rect(cornerBox_X + 60, posFieldY, wBox - 120, 25), "Lokasi : ");
            Lokasi = GUI.TextField(new Rect(cornerBox_X + 110, posFieldY, wBox - 120, 25), Lokasi, 25);
            posFieldY += txtFieldHplusMargin;

            GUI.Label(new Rect(cornerBox_X + 10, posFieldY, wBox - 120, 25), "Deskripsi : ");
            posFieldY += txtFieldHplusMargin;
            Deskripsi = GUI.TextArea(new Rect(cornerBox_X + 10, posFieldY, wBox - 20, 120), Deskripsi, 200);
            posFieldY += txtAreaH;

            GUI.Label(new Rect(cornerBox_X + 10, posFieldY, wBox - 120, 25), "Waktu Mulai : ");
            JamMulai = GUI.TextField(new Rect(cornerBox_X + 110, posFieldY, timeW, timeH), JamMulai, 2);
            GUI.Label(new Rect(cornerBox_X + 110 + timeW + 2, posFieldY, 5, 25), ":");
            MenitMulai = GUI.TextField(new Rect(cornerBox_X + 110 + timeW + 5, posFieldY, timeW, timeH), MenitMulai, 2);
            posFieldY += txtFieldHplusMargin;

            //durasi
            GUI.Label(new Rect(cornerBox_X + 10, posFieldY, wBox - 120, 25), "Durasi : ");
            HariDurasi = GUI.TextField(new Rect(cornerBox_X + 60, posFieldY, timeW + 5, timeH), HariDurasi, 2);
            GUI.Label(new Rect(cornerBox_X + 60 + timeW + 5, posFieldY, 40, 25), " Hari");
            //posFieldY += txtFieldHplusMargin;

            JamDurasi = GUI.TextField(new Rect(cornerBox_X + 120, posFieldY, timeW + 5, timeH), JamDurasi, 2);
            GUI.Label(new Rect(cornerBox_X + 120 + timeW + 5, posFieldY, 40, 25), " Jam");
            //posFieldY += txtFieldHplusMargin;

            MenitDurasi = GUI.TextField(new Rect(cornerBox_X + 185, posFieldY, timeW + 5, timeH), MenitDurasi, 2);
            GUI.Label(new Rect(cornerBox_X + 185 + timeW + 5, posFieldY, 40, 25), " Menit");
            posFieldY += txtFieldHplusMargin;

            // file pendukung
            GUI.Label(new Rect(cornerBox_X + 125, posFieldY, wBox - 120, 25), submitUpload);
            toggleFile = GUI.Toggle(new Rect(cornerBox_X + 10, posFieldY, wBox - 120, 25), toggleFile, "File Pendukung : ");
            if (toggleFile)
            {
                posFieldY += txtFieldHplusMargin;

                GUI.BeginGroup(new Rect(cornerBox_X + 30, posFieldY, wBox * 0.5f, 25));
                GUILayout.Label(m_textPath ?? "------ pilih file ------");

                GUI.EndGroup();
                if (GUI.Button(new Rect(cornerBox_X + 115 + 45, posFieldY, wBox - 220, 20), "..."))
                {
                    m_fileBrowser = new FileBrowser(new Rect(500, 0, 600, 500), "Pilih File", FileSelectedCallback);
                    m_fileBrowser.SelectionPattern = "*.*";
                    m_fileBrowser.DirectoryImage = m_directoryImage;
                    m_fileBrowser.FileImage = m_fileImage;
                }
                if (GUI.Button(new Rect(cornerBox_X + 115 + 78, posFieldY, wBox - 195, 20), "Upload"))
                {
                    string namaFile = Path.GetFileName(m_textPath);
                    if (m_textPath == null)
                    {
                        submitUpload = "File belum dipilih";
                        return;
                    }
                    else
                    {
                        FileUtil.CopyFileOrDirectory(m_textPath, lokasiUpload + namaFile);
                        submitUpload = "File berhasil diupload";
                    }
                }
            }
            posFieldY += txtFieldHplusMargin;

            GUI.backgroundColor = Color.blue;
            //konfigurasi unit
            //GUI.Label(new Rect(cornerBox_X + 10, posFieldY, wBox - 120, 25), "Konfigurasi Unit : ");
            toggleUnitConfig = GUI.Toggle(new Rect(cornerBox_X + 10, posFieldY, wBox - 120, 25), toggleUnitConfig, "Konfigurasi Unit : ");
            if (toggleUnitConfig)
            {
                if (GUI.Button(new Rect(cornerBox_X + 115 + 10, posFieldY, wBox - 130, 25), "Atur Pergerakan"))
                {
                    editUnitMode = true;
                    HistoryManager.showHistory = true;
                }
            }
            posFieldY += txtFieldHplusMargin;

            GUI.Label(new Rect(cornerBox_X + 10, posFieldY, wBox - 120, 25), "Daftar Scene : ");

            GUI.Box(new Rect(cornerBox_X + 10, posFieldY + 30, wBox - 20, 100), "");

            GUILayout.BeginArea(new Rect(cornerBox_X + 10, posFieldY + 30, wBox - 20, 100));
            scrollSceneList = GUI.BeginScrollView(new Rect(0, 0, wBox - 20, 100), scrollSceneList, new Rect(0, 0, wBox + 100, wBox + 100), false, true);
            GUILayout.BeginVertical();
            foreach (string sn in sceneNames)
            {
                if (GUILayout.Button(sn))
                {
                    //tinggal diimplementasi
                }
            }
            GUILayout.EndVertical();
            GUI.EndScrollView();
            GUILayout.EndArea();

            posFieldY += txtAreaH;

            GUI.Label(new Rect(cornerBox_X + 10, posFieldY + 10, wBox, 40), submitKegInfo);
            posFieldY += txtFieldHplusMargin;

            if (GUI.Button(new Rect(cornerBox_X + 60, posFieldY + 20, wBox - 180, 30), "Simpan"))
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
                        //mulai
                        JamMulai = JamMulai == "" ? "00" : JamMulai;
                        MenitMulai = MenitMulai == "" ? "00" : MenitMulai;
                        string waktuMulai = JamMulai + ":" + MenitMulai;
                        //durasi
                        HariDurasi = HariDurasi == "" ? "0" : HariDurasi;
                        JamDurasi = JamDurasi == "" ? "0" : JamDurasi;
                        MenitDurasi = MenitDurasi == "" ? "0" : MenitDurasi;
                        TimeSpan durasi = TimeSpan.FromDays(Double.Parse(HariDurasi)).Add(TimeSpan.FromHours(Double.Parse(JamDurasi)).Add(TimeSpan.FromMinutes(Double.Parse(MenitDurasi))));

                        OperationManager.addToOperationList(
                            new OperationItem(
                                PlayerPrefs.GetString("satuan", ""),
                                Hari[selectedItemIndex].text,
                                NamaKeg,
                                Lokasi,
                                Deskripsi,
                                null,
                                null,
                                waktuMulai,
                                durasi, toggleFile, toggleUnitConfig));
                        emptyTheField();
                        //item lainnya menyusul
                        submitKegInfo = "berhasil disimpan";
                        list = true;
                    }
                    else
                    {
                        //ngedit
                        //waktu mulai
                        JamMulai = JamMulai == "" ? "00" : JamMulai;
                        MenitMulai = MenitMulai == "" ? "00" : MenitMulai;
                        string waktuMulai = JamMulai + ":" + MenitMulai;
                        //durasi
                        HariDurasi = HariDurasi == "" ? "0" : HariDurasi;
                        JamDurasi = JamDurasi == "" ? "0" : JamDurasi;
                        MenitDurasi = MenitDurasi == "" ? "0" : MenitDurasi;
                        TimeSpan durasi = TimeSpan.FromDays(Double.Parse(HariDurasi)).Add(TimeSpan.FromHours(Double.Parse(JamDurasi)).Add(TimeSpan.FromMinutes(Double.Parse(MenitDurasi))));

                        ((OperationItem)OperationManager.operationList[nowEditingOpId]).name = NamaKeg;
                        ((OperationItem)OperationManager.operationList[nowEditingOpId]).location = Lokasi;
                        ((OperationItem)OperationManager.operationList[nowEditingOpId]).description = Deskripsi;
                        //((OperationItem)OperationManager.operationList[nowEditingOpId]).startTime = waktuMulai;
                        ((OperationItem)OperationManager.operationList[nowEditingOpId]).duration = durasi;
                        
                        ((OperationItem)OperationManager.operationList[nowEditingOpId]).prosesStartAndEndTime(waktuMulai,Hari[selectedItemIndex].text,durasi);
                        ((OperationItem)OperationManager.operationList[nowEditingOpId]).hasVideo = toggleFile;
                        ((OperationItem)OperationManager.operationList[nowEditingOpId]).hasUnitMovement = toggleUnitConfig;
                        emptyTheField();
                        submitKegInfo = "berhasil diperbaharui";
                        nowEditingOpId = GA_NGEDIT;
                        list = true;
                    }
                    foreach (OperationItem oi in OperationManager.operationList)
                    {
                        Debug.Log("opList. opName = " + oi.name);
                    }
                    Debug.Log("SETELAH SORTING:");
                    OperationItem_SortByHariStartTimeAscending comp = new OperationItem_SortByHariStartTimeAscending();
                    OperationManager.operationList.Sort(comp);
                    foreach (OperationItem oi in OperationManager.operationList)
                    {
                        Debug.Log("opList. opName = "+oi.name);
                    }
                }

            }
            if (GUI.Button(new Rect(cornerBox_X + 140, posFieldY + 20, wBox - 180, 30), "Batal"))
            {
                showFormKegiatan = false;
                nowEditingOpId = GA_NGEDIT;
            }
        }
        GUI.Label(new Rect(350, 2, 100, 20), ketSatuan);
    }

    private void emptyTheField()
    {
        NamaKeg = "";
        Lokasi = "";
        Deskripsi = "";
        JamMulai = "";
        MenitMulai = "";
        HariDurasi = "";
        JamDurasi = "";
        MenitDurasi = "";
        toggleFile = false;
        toggleUnitConfig = false;
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
            
            GUILayout.BeginArea(new Rect(0, 520, width, 100), GUI.skin.box);
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            for (int i=0, len=unitUdaraList.Length; i<len; i++)
            {
                UnitInfo uin = unitUdaraList[i];
                if (uin.building == null) { GUI.enabled = false; } else { GUI.enabled = true; }
                GUILayout.BeginVertical(uin.texture, GUI.skin.box,GUILayout.Width(70));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(uin.name, GUILayout.Height(kegiatanH)))
                {
                    buildingPlacement.SetItem(uin.building);
                }
                GUILayout.EndVertical();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            return;

        }
        else if (showLaut)
        {
            GUILayout.BeginArea(new Rect(0, 520, width, 100), GUI.skin.box);
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            for (int i = 0, len = unitLautList.Length; i < len; i++)
            {
                UnitInfo uin = unitLautList[i];
                if (uin.building == null) { GUI.enabled = false; } else { GUI.enabled = true; }
                GUILayout.BeginVertical(uin.texture, GUI.skin.box, GUILayout.Width(70));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(uin.name, GUILayout.Height(kegiatanH)))
                {
                    buildingPlacement.SetItem(uin.building);
                }
                GUILayout.EndVertical();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            return;
            
        }
        else if (showDarat)
        {
            GUILayout.BeginArea(new Rect(0, 520, width, 100), GUI.skin.box);
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            for (int i = 0, len = unitDaratList.Length; i < len; i++)
            {
                UnitInfo uin = unitDaratList[i];
                if (uin.building == null) { GUI.enabled = false; } else { GUI.enabled = true; }
                GUILayout.BeginVertical(uin.texture, GUI.skin.box, GUILayout.Width(70));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(uin.name, GUILayout.Height(kegiatanH)))
                {
                    buildingPlacement.SetItem(uin.building);
                }
                GUILayout.EndVertical();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            return;
            
        }
        else if (showPersonel)
        {
            GUILayout.BeginArea(new Rect(0, 520, width, 100), GUI.skin.box);
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            for (int i = 0, len = unitPersonelList.Length; i < len; i++)
            {
                UnitInfo uin = unitPersonelList[i];
                if (uin.building == null) { GUI.enabled = false; } else { GUI.enabled = true; }
                GUILayout.BeginVertical(uin.texture, GUI.skin.box, GUILayout.Width(70));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(uin.name, GUILayout.Height(kegiatanH)))
                {
                    buildingPlacement.SetItem(uin.building);
                }
                GUILayout.EndVertical();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            return;

        }

        else if (showAlutsista)
        {
            GUILayout.BeginArea(new Rect(0, 520, width, 100), GUI.skin.box);
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            for (int i = 0, len = unitAlutList.Length; i < len; i++)
            {
                UnitInfo uin = unitAlutList[i];
                if (uin.building == null) { GUI.enabled = false; } else { GUI.enabled = true; }
                GUILayout.BeginVertical(uin.texture, GUI.skin.box, GUILayout.Width(70));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(uin.name, GUILayout.Height(kegiatanH)))
                {
                    buildingPlacement.SetItem(uin.building);
                }
                GUILayout.EndVertical();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            return;

        }
        else
        {
            GUI.Box(new Rect(0, 520, width, 100), "");
        }
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
        // init unit infonya
        unitUdaraList = new UnitInfo[] { 
                new UnitInfo("Sukhoi", Sukhoi,buildings[0]), 
                new UnitInfo("F16", F16,buildings[1]), 
                new UnitInfo("F-5", F5,buildings[2]), 
                new UnitInfo("Hawk", Hawk,buildings[3]), 
                new UnitInfo("TU-16", TU16,buildings[4]), 
                new UnitInfo("Boeing-737", B737,buildings[5]), 
                new UnitInfo("C-130", C130,buildings[6]), 
                new UnitInfo("C-212", C212,buildings[7]), 
                new UnitInfo("CN235", CN235,buildings[8]), 
                new UnitInfo("NAS-332", NAS332,buildings[9]), 
                new UnitInfo("EC120B", EC120B,buildings[10]), 
                new UnitInfo("SA-330", SA330,buildings[11]), 
                new UnitInfo("Bell-412", Bell412,buildings[12])
            };

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
        // init unit info unit laut
        unitLautList = new UnitInfo[] { 
                new UnitInfo("AYN-351", KRIayani,buildings[13]), 
                new UnitInfo("AHP-355", KRIabdulhalim,buildings[14]), 
                new UnitInfo("FAT-361", KRIfatahilah,buildings[15]), 
                new UnitInfo("PRP-729", KRIpulaurempang,buildings[16]), 
                new UnitInfo("PRE-711", KRIpulaurenggat,buildings[17]), 
                new UnitInfo("TPN-315", KRItelukpenyu,buildings[18]), 
                new UnitInfo("TLE-517", KRItelukende,buildings[19]), 
                new UnitInfo("TBT-516", KRItelukbanten,buildings[20]), 
                new UnitInfo("MKS-590", KRImakasar,buildings[21]), 
                new UnitInfo("SBY-591", KRIsurabaya,buildings[22]), 
                new UnitInfo("NGL-402", KRInanggala,buildings[23]), 
                new UnitInfo("CKR-401", KRIcakra,buildings[24]), 
                new UnitInfo("TMR-385", KRIteukuumar,buildings[25]),
                new UnitInfo("CND-375", KRIcutnyakdien,buildings[26])
            };
        
        //darat
        leopard = (Texture2D)Resources.Load("Leopard");
        scorpion = (Texture2D)Resources.Load("Scorpion");
        amx13 = (Texture2D)Resources.Load("AMX-13");
        anoa = (Texture2D)Resources.Load("APS-3 Anoa");
        amfibi = (Texture2D)Resources.Load("Amfibi");
        // init unit info unit darat
        unitDaratList = new UnitInfo[] { 
                new UnitInfo("Leopard", leopard,buildings[27]), 
                new UnitInfo("Scorpion", scorpion,buildings[28]), 
                new UnitInfo("AMX-13", amx13,buildings[29]), 
                new UnitInfo("APS-3 Anoa", anoa,buildings[30]), 
                new UnitInfo("Amfibi", amfibi,buildings[31])
            };

        //personel
        infanteri = (Texture2D)Resources.Load("infanteri");
        // init unit info personel
        unitPersonelList = new UnitInfo[] { 
                new UnitInfo("infanteri", infanteri,buildings[32])
            };


        //alutsista lain
        arhanud = (Texture2D)Resources.Load("Arhanud");
        radar = (Texture2D)Resources.Load("Radar");
        howitzer = (Texture2D)Resources.Load("Howitzer");
        // init unit info alutsista
        unitAlutList = new UnitInfo[] { 
                new UnitInfo("Arhanud", arhanud,buildings[33]),
                new UnitInfo("Radar", radar,buildings[34]),
                new UnitInfo("Howitzer", howitzer,buildings[35])
            };
    }

    //begin file browser
    /*    protected string m_textPath;
        protected FileBrowser m_fileBrowser;
        [SerializeField]
        protected Texture2D m_directoryImage,
                            m_fileImage;
        private bool showSaveBrowser = false;
        private bool showSaveUnitDialog = false;
        private bool showConfirmDeleteSave = false;
        private bool showPauseMenu = true;
        private int curOpPlayIdx;
    */
    protected void OnGUIMain()
    {
        float bPosW = 300;
        float bPosH = 200;
        float bPosX = (Screen.width - bPosW) / 2;
        float bPosY = (Screen.height - bPosH) / 2 - 40;

        GUILayout.BeginArea(new Rect(bPosX, bPosY, bPosW, bPosH));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Text File", GUILayout.Width(100));
        GUILayout.FlexibleSpace();
        GUILayout.Label(m_textPath ?? "none selected");
        if (GUILayout.Button("...", GUILayout.ExpandWidth(false)))
        {
            m_fileBrowser = new FileBrowser(
                new Rect(100, 100, 600, 500),
                "Choose Text File",
                FileSelectedCallback
            );
            m_fileBrowser.SelectionPattern = "*.txt";
            m_fileBrowser.DirectoryImage = m_directoryImage;
            m_fileBrowser.FileImage = m_fileImage;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    protected void FileSelectedCallback(string path)
    {
        m_fileBrowser = null;
        m_textPath = path;
    }

    //end file browser

    /*    void DoMyWindow(int windowID)
        {

            OpenFileWindow(location);
            GUI.DragWindow();
        }
    */

    /*    void OpenFileWindow(string location)
        {
            scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1, GUILayout.Width(400), GUILayout.Height(400));
            GUILayout.BeginVertical();
            FileBrowser(location, 0, 0);
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.Label("Selected:" + path);
        }
    */

    /*    void FileBrowser(string location, int spaceNum, int index)
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
    
        */

    /*    private object SelectList(ICollection list, object selected, GUIStyle style, Texture image, int spaceNum)
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
    */

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}