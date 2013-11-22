﻿using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.InteropServices;


public class OperationManager : MonoBehaviour
{

    // the Singleton
    private static OperationManager instance = null;
    public static OperationManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("instantiate");
                GameObject go = new GameObject();
                instance = go.AddComponent<OperationManager>();
                go.name = "singleton";
            }
            return instance;
        }
    }

    //attributes
    public static ArrayList operationList = new ArrayList();
    public static int InstanceIdx = 0;
    public static string FILE_EXT = ".tfgsg";
    public static string FILE_EXT_UNITCONF = ".tfguc";

    public static DateTime HARI_HA = new DateTime(2007, 10, 1, 00, 00, 00);

    //PLAY MODE RELATED
    OperationItem opFirstPlay; //kegiatan yg diklik-play oleh user
    private DateTime opFirstPlayStartDate; //startTime opFirstPlay

    DateTime gameClockTime; // clock virtual
    public static string gameClockValue; // nilai clock, dipake di GUI
    public static int gameClockSpeed = 1; // kecepatan default clock
    private string posHariInGameClock = ""; //posisi hari berdasarkan clock

    public static ArrayList nowPlayingList = new ArrayList(); // list kegiatan yg sedang aktif saat clock berjalan
    private OperationItem curRunOp;
    private bool movReady;
    public static MovieTexture movTexture;

    //HD MOVIE RELATED
    //public AVProWindowsMediaMovie _movie;
    //public AVProWindowsMediaGUIDisplay _movieDisplay;
    //public OperationPlayVideo Oplayvid;
    private GCHandle _bytesHandle;
    private System.IntPtr _moviePtr;
    private uint _movieLength;

    // SIMPAN ANTAR SCENE
    public static OperationItem curOpItem;
    private GameObject taggerPrefab;
    private bool isMovieNowPlaying = false;

    void Start()
    {
        //_movieDisplay = Camera.main.GetComponent<AVProWindowsMediaGUIDisplay>();
        //_movie = Camera.main.GetComponent<AVProWindowsMediaMovie>();
        //Oplayvid = Camera.main.GetComponent<OperationPlayVideo>();
        StartCoroutine(startTheGameClock());
        StartCoroutine(playTheKegs());
    }

    private Transform getTagContainer()
    {
        GameObject go = GameObject.Find("TagContainer");
        if (go == null)
        {
            go = new GameObject("TagContainer");
        }
        return go.transform;
    }

    private void removeAllTagLocation()
    {
        Transform tagContainer = getTagContainer();
        //tagContainer.DestroyChildren();
        for (int i = 0; i < tagContainer.childCount; i++)
        {
            Destroy(tagContainer.GetChild(i).gameObject);
        }
    }

    private void loadOperationLocationTags(OperationItem opItem)
    {
        removeAllTagLocation();
        if (opItem.locationPoints.Length > 0)
        {
            //ListLokasiTemp.Clear();
            //listLokasiTagObj.Clear();
            for (int i = 0; i < opItem.locationPoints.Length; i++)
            {
                GameObject pin = GameObject.Instantiate(getTaggerPrefab(), opItem.locationPoints[i].locationPoint, Quaternion.identity) as GameObject;
                pin.name = "Pin_" + opItem.locationPoints[i].objInstanceID;
                pin.transform.parent = getTagContainer();

                //ListLokasiTemp.Add(opItem.locationPoints[i]);
                //listLokasiTagObj.Add(pin);
            }
        }
    }

    private GameObject getTaggerPrefab()
    {
        if (taggerPrefab == null)
        {
            taggerPrefab = Resources.Load("Models/Miscellaneous/Pin_ready") as GameObject;
        }
        return taggerPrefab;

    }

    private IEnumerator playTheKegs()
    {
        Debug.Log("playTheKegs");
        while (true)
        {
            yield return null;
            if (MenuUnit.showPlayMode)
            {
                for (int i = MenuUnit.curOpPlayIdx; i < OperationManager.operationList.Count; i++)//DI SINI MASALAHNYA, SOLUSINYA: SORTING BY DATE
                {
                    yield return null;
                    
                    OperationItem opItIn = (OperationItem)OperationManager.operationList[i];
                    Debug.Log("play kegiatan: " + opItIn.name);
                    if (opItIn.satuan == PlayerPrefs.GetString("satuan"))
                    {
                        string pos = preProcPosHar(opItIn.posisiHari);

                        DateTime opStartTime = opItIn.startTime;
                        DateTime opDoneTime = opItIn.endTime;

                        if (posHariInGameClock == pos)
                        {

                            if (gameClockTime.TimeOfDay >= opStartTime.TimeOfDay)// && gameTime < opDoneTime)
                            {

                                if (!nowPlayingList.Contains(opItIn))
                                {
                                    nowPlayingList.Add(opItIn);
                                    curRunOp = opItIn;



                                    if (opItIn.locationPoints.Length > 0)
                                    {
                                        if (!opItIn.isRunning)
                                        {
                                            loadOperationLocationTags(opItIn);
                                            playOperationVideoIfExists(opItIn);
                                            while (AVProOperationVideo.isActiveMoviePlaying)
                                            {
                                                yield return null;
                                                MenuUnit.testMovementMode = false;
                                            }
                                            moveOperationUnitIfExists(opItIn);
                                            opItIn.isRunning = true;
                                        }
                                    }

                                }
                            }
                        }

                        if (gameClockTime >= opDoneTime)
                        {
                            if (nowPlayingList.Contains(opItIn))
                            {
                                nowPlayingList.Remove(opItIn);

                            }
                        }

                    }
                    if (!MenuUnit.showPlayMode) { nowPlayingList.Clear(); break; }
                }
            }

        }
    }

    //MOVIE METHODS
    private void playOperationVideoIfExists(OperationItem operationToPlay)
    {
        if (operationToPlay.hasVideo)
        {
            Debug.Log("File loading dari " + operationToPlay.files);
            if (operationToPlay.hasVideo)
            {
                summonVideoPlayerThenPlay(operationToPlay.files);
                //isMovieNowPlaying = true;
            }
        }
    }

    private void summonVideoPlayerThenPlay(string path)
    {
        GameObject om = GameObject.Find("OtherManager");
        if (om != null)
        {
            AVProOperationVideo avp = om.GetComponent<AVProOperationVideo>();
            if (avp != null)
            {
                Debug.Log("ngeplay nih!");
                AVProOperationVideo.showPlayer = true;
                avp.PlayOperationVideo(path);
            }
        }
    }

    private void pauseTestVideo()
    {
        GameObject om = GameObject.Find("OtherManager");
        if (om != null)
        {
            AVProOperationVideo avp = om.GetComponent<AVProOperationVideo>();
            if (avp != null)
            {
                //AVProOperationVideo.showPlayer = false;
                avp.PauseTestVideo();
            }
        }
    }

    private void stopTestVideo()
    {
        GameObject om = GameObject.Find("OtherManager");
        if (om != null)
        {
            AVProOperationVideo avp = om.GetComponent<AVProOperationVideo>();
            if (avp != null)
            {
                AVProOperationVideo.showPlayer = false;
                avp.StopTestVideo();
            }
        }
    }

    //END. MOVIE METHODS

    private IEnumerator startTheGameClock()
    {
        while (true)
        {
            yield return null;
            if (MenuUnit.showPlayMode)
            {

                // opIt starting point
                opFirstPlay = (OperationItem)OperationManager.operationList[MenuUnit.curOpPlayIdx];
                // mulai semua kegiatan dari starting point
                gameClockTime = opFirstPlay.startTime; //timeToIncrement
                //DateTime lastST = DateTime.Parse(opFirstPlay.startTime); //caching hari "kemarin"
                opFirstPlayStartDate = opFirstPlay.startTime;//startTime
                //DateTime eTim = opFirstPlay.startTime.Add(opFirstPlay.duration); //endTime

                string curPosHar = opFirstPlay.posisiHari;

                gameClockValue = opFirstPlay.posisiHari + " " + string.Format("{0}:{1}:{2}", gameClockTime.Hour, gameClockTime.Minute, gameClockTime.Second);
                while (true)
                {
                    yield return null;
                    if (!AVProOperationVideo.isActiveMoviePlaying) // STOP CLOCK SELAMA IA MAIN VIDEONYA
                    {
                        gameClockTime = gameClockTime.AddSeconds(1 * gameClockSpeed);
                        yield return new WaitForSeconds(1 / gameClockSpeed);
                        //proses posHar
                        posHariInGameClock = getEndPosHar(curPosHar);

                        gameClockValue = posHariInGameClock + " " + gameClockTime.ToString(" HH:mm:ss");//("dd/MM/yyyy HH:mm:ss");
                        //+ "\nmulai: " + sTim.ToString("dd/MM/yyyy HH:mm") + "\ntamat: " + eTim.ToString("dd/MM/yyyy HH:mm");

                        if (!MenuUnit.showPlayMode) { break; }
                    }

                }//endwhile
            }//endif
        }//endwhile
    }

    private string getEndPosHar(string curPosHar)
    {
        string hPosHar = curPosHar.Substring(2).Replace(" ", ""); //cari string posisi hari bagian angkanya
        //Debug.Log("hposHar: " + hPosHar);
        int posHarInt = 0; //siap2 konversi string jadi int
        Int32.TryParse(hPosHar, out posHarInt); //kalo gagal berarti hari H

        int hEnd = posHarInt - (opFirstPlayStartDate.Date - gameClockTime.Date).Days;

        //Debug.Log("selisih hari: "+(lastST - gameTime).Days+" lastST: "+lastST + " hEnd: " + hEnd);
        string hEndStr = "Hari H";
        if (hEnd < 0) { hEndStr = "H" + hEnd; }
        else if (hEnd > 0) { hEndStr = "H+" + hEnd; }

        return hEndStr;
    }

    private string preProcPosHar(string posHar)
    {
        string hPosHar = posHar.Substring(2).Replace(" ", "");
        int posHarInt = 0;
        Int32.TryParse(hPosHar, out posHarInt);

        string hEndStr = "Hari H";
        if (posHarInt < 0) { hEndStr = "H" + posHarInt; }
        else if (posHarInt > 0) { hEndStr = "H+" + posHarInt; }

        return hEndStr;
    }

    //private IEnumerator playIndividualOperation(OperationItem opItIndv)
    //{

    //}

    private IEnumerator loadKegMovie(string p)
    {
        movReady = false;
        //GameObject go = GameObject.Find("movieTestObject");
        //GUITexture gtex = go.GetComponent<GUITexture>();
        //AudioSource auds = gameObject.AddComponent<AudioSource>();
        p = @"file://" + "C:\\Users\\Asus\\Documents\\UnityProject\\seskoadrev18\\3Dopsgab\\Assets\\Movie\\Test.ogg";
        //p = "http://www.unity3d.com/webplayers/Movie/sample.ogg";
        //if (File.Exists(p))
        {

            WWW www = new WWW(p);
            Debug.Log("loading movie from " + p + " " + www.size);
            movTexture = www.movie;

            while (!movTexture.isReadyToPlay)
            {
                Debug.Log("loading process.." + www.progress);
                yield return null;
            }
            movReady = true;
            Debug.Log("loading done " + movReady);
            movTexture.Play();
            //transform.localScale = new Vector3(0, 0, 0);
            //transform.position = new Vector3(0.5f, 0.5f, 0);
            //gtex.pixelInset = new Rect(0, 0, movTexture.width, movTexture.height);
            //.xMin = -movTexture.width / 2;
            //gtex.pixelInset.xMax = movTexture.width / 2;
            //gtex.pixelInset.yMin = -movTexture.height / 2;
            //gtex.pixelInset.yMax = movTexture.height / 2;

            //audio.clip = movTexture.audioClip;
            //movTexture.Play();
        }
    }

    private void moveOperationUnitIfExists(OperationItem operationToPlay)
    {
        //mainkan pergerakan
        if (operationToPlay.hasUnitMovement)
        {
            //if (!AVProOperationVideo.isActiveMoviePlaying) // STOP PERGERAKAN SELAMA IA MAIN VIDEONYA
            {
                //operationToPlay.isRunning = true;
                if (Application.loadedLevelName == operationToPlay.sceneName)
                    LevelSerializer.LoadObjectTree(operationToPlay.unitConfig);
                MenuUnit.testMovementMode = true;
            }
            //else
            //{
            //    MenuUnit.testMovementMode = false;
            //}
        }


    }

    //END PLAY MODE RELATED

    public static void setHariH(DateTime newHariH)
    {
        HARI_HA = newHariH;
    }

    public static bool addToOperationList(OperationItem a)
    {
        if (a == null) return false;
        operationList.Add(a);
        //LevelSerializer.SaveGame(a.ToString());
        Debug.Log("tambah kegiatan: " + a.ToString() + " max saves:" + LevelSerializer.MaxGames);
        return true;
    }
    /*
    public static bool undoHistory(){
        if (historyList.Count == 0) return false;
        historyList.RemoveAt(historyList.Count - 1);
        return true;
    }
     */

    public static bool undoOperation(OperationItem a)
    {
        Debug.Log("loading : " + a.ToString());
        foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
        {
            Debug.Log("\t\tsave entries: " + sg.Name);
            if (sg.Name == a.ToString())
            {
                sg.Load();
                //LevelSerializer.LoadNow(sg.Data);
                Debug.Log("berhasil Load");
                return true;
            }
        }
        Debug.Log("gagal Load");
        return false;
    }


    public void OnApplicationQuit()
    {
        //foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
        //{
        //    sg.Delete();
        //}
        //Debug.Log("all saved operation removed");
    }

    public static IEnumerator playOperation()
    {
        while (true)
        {
            //cek if playmode, do this..
            //cek start mulai playnya dari mana
            //mulai dari posisi itu
            //IF otomatis, looping for posisi itu hingga akhir di operationList 
            //  IF kondisi satuan, hari, sesuai,
            //  play dengan waktu tertentu, misal fix 10 detik per slide.
            //  oh ya ada tombol "next" dan "prev" untuk play mode.
            //  IF hasUnit
            //    posisikan GUI di pojok kanan atas, kecil.
            //    testEksekusi = true (Coroutine?)
            //  IF hasVids
            //    testEksekusi = false;
            //    playVids... (Coroutine?)
            //  getNextOperation..
            //  endloop

            // pada akhirnya, playmode = false
            yield return null;
        }
    }

    public static void saveGameToFile(string filename)
    {
        LevelSerializer.SerializeLevelToFile(filename);
        LevelSerializer.SavedGames.Clear();
        HistoryManager.historyList.Clear();
        Debug.Log("Saved as " + filename);
    }

    public static byte[] saveUnitConfigTree()
    {
        byte[] bytarr = null;
        GameObject ucon = GameObject.Find("UnitContainer") as GameObject;
        if (ucon != null)
        {
            //BasicUnitMovement[] bumsInUcon = ucon.transform.GetComponentsInChildren<BasicUnitMovement>();
            //for(int i=0; i<bumsInUcon.Length;i++)
            //{
            //    bumsInUcon[i].isSelected=true; //biar line renderenya aktif dulu;
            //}
            bytarr = LevelSerializer.SaveObjectTree(ucon);
        }
        return bytarr;
    }

    public static bool deleteSavedGame(string filename)
    {
        Debug.Log("mau delete: " + filename);
        try
        {
            // A.
            // Try to delete the file.
            File.Delete(filename);
            Debug.Log("berhasil delete");
            return true;
        }
        catch (IOException)
        {
            // B.
            // We could not delete the file.
            Debug.Log("gagal delete");
            return false;
        }
    }

    internal static void stopRunningAll()
    {
        for (int i = 0; i < operationList.Count; i++)
        {
            ((OperationItem)operationList[i]).isRunning = false;
        }
    }
}

public class OperationItem : IComparable
{
    public string satuan;
    public string posisiHari;
    public string name;
    public string location;
    public OperationLocation[] locationPoints;
    public string description;
    public string files;
    public byte[] unitConfig;
    //public string unitConfig;
    //public string startTime;
    public DateTime startTime;
    public TimeSpan duration;
    //public string endTime;
    public DateTime endTime;
    public bool hasUnitMovement;
    public bool hasVideo;
    public bool isRunning = false;
    public string sceneName;

    public OperationItem()
    {
        satuan = "";
        posisiHari = "";
        name = "";
        location = "";
        locationPoints = new OperationLocation[] { };
        description = "";
        files = "";
        unitConfig = new byte[] { };
        startTime = OperationManager.HARI_HA;
        endTime = OperationManager.HARI_HA;
        sceneName = Const.PETA_INDONESIA;
    }

    public OperationItem(string satuan, string posHar, string nama, string lokasi, string deskripsi)
    {
        this.satuan = satuan;
        this.posisiHari = posHar;
        this.name = nama;
        this.location = lokasi;
        this.description = deskripsi;
        this.locationPoints = new OperationLocation[] { };
        this.files = "";
        this.unitConfig = null;
        startTime = OperationManager.HARI_HA;
        endTime = OperationManager.HARI_HA;
    }

    public OperationItem(string satuan, string posHar, string nama, string lokasi, string deskripsi, string newUnitConfig)
    {
        this.satuan = satuan;
        this.posisiHari = posHar;
        this.name = nama;
        this.location = lokasi;
        this.description = deskripsi;
        this.locationPoints = new OperationLocation[] { };
        this.files = "";
        this.unitConfig = new byte[] { };
        startTime = OperationManager.HARI_HA;
        endTime = OperationManager.HARI_HA;
    }

    public OperationItem(string satuan, string posHar, string nama, string lokasi, string deskripsi, string file, string newUnitConfig)
    {
        this.satuan = satuan;
        this.posisiHari = posHar;
        this.name = nama;
        this.location = lokasi;
        this.description = deskripsi;
        this.locationPoints = new OperationLocation[] { };
        this.files = file;
        this.unitConfig = new byte[] { };
        startTime = OperationManager.HARI_HA;
        endTime = OperationManager.HARI_HA;
    }

    //yg dipake
    public OperationItem(string satuan, string posHar, string nama, string lokasi, OperationLocation[] newLocPoints, string deskripsi, string file, byte[] newUnitConfig, string startTime, TimeSpan duration, bool hasFile, bool hasUnit, string sceneName)
    {
        this.satuan = satuan;
        this.posisiHari = posHar;
        this.name = nama;
        this.location = lokasi;
        this.description = deskripsi;
        this.locationPoints = newLocPoints;
        this.files = file;
        this.unitConfig = newUnitConfig;
        this.duration = duration;
        //proses starttime dan endtime
        prosesStartAndEndTime(startTime, posHar, duration);

        this.hasVideo = hasFile;
        this.hasUnitMovement = hasUnit;
        this.sceneName = sceneName;
    }

    public void prosesStartAndEndTime(string startTime, string posisiHari, TimeSpan durasi)
    {
        int jamStart = 0;
        Int32.TryParse(startTime.Split(':')[0], out jamStart);

        int menitStart = 0;
        Int32.TryParse(startTime.Split(':')[1], out menitStart);

        this.startTime = OperationManager.HARI_HA.AddDays(getPosHarInt(posisiHari)).AddHours(jamStart).AddMinutes(menitStart);
        this.endTime = this.startTime.Add(durasi);

    }

    public int getPosHarInt()
    {
        string hPosHar = this.posisiHari.Substring(2).Replace(" ", "");
        int posHarInt = 0;
        Int32.TryParse(hPosHar, out posHarInt);

        return posHarInt;
    }

    public int getPosHarInt(string posisiHari)
    {
        string hPosHar = posisiHari.Substring(2).Replace(" ", "");
        int posHarInt = 0;
        Int32.TryParse(hPosHar, out posHarInt);

        return posHarInt;
    }

    public string getStartTimeString()
    {
        string r = "";
        int posHarInt = this.getPosHarInt();
        r += ((posHarInt < 0) ? ("H" + posHarInt) : (posHarInt > 0) ? ("H+" + posHarInt) : "hari H");
        r += " pukul ";
        r += this.startTime.ToString("HH:mm");
        return r;
    }

    public string getEndTimeString()
    {
        string r = "";
        int posHarInt = this.getPosHarEndInt();
        r += ((posHarInt < 0) ? ("H" + posHarInt) : (posHarInt > 0) ? ("H+" + posHarInt) : "hari H");
        r += " pukul ";
        r += this.endTime.ToString("HH:mm");
        return r;
    }

    private int getPosHarEndInt()
    {
        return (int)(this.endTime.Date - OperationManager.HARI_HA.Date).TotalDays;// -1;
    }

    public override string ToString()
    {
        return name + "\nLokasi: " + location + "\nSatuan: " + satuan;
    }

    int IComparable.CompareTo(object obj) //ascending
    {
        OperationItem oi = (OperationItem)obj;
        if (this.getPosHarInt() < oi.getPosHarInt())
        {
            return -1;
        }
        else if (this.getPosHarInt() > oi.getPosHarInt())
        {
            return 1;
        }
        else
        {// harinya sama
            DateTime thisStartTime = this.startTime;
            DateTime oiStartTime = oi.startTime;
            if (thisStartTime.CompareTo(oiStartTime) < 0)
            {
                return -1;
            }
            else if (thisStartTime.CompareTo(oiStartTime) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}

class OperationItem_SortByHariStartTimeAscending : IComparer
{
    int System.Collections.IComparer.Compare(object xx, object yy)
    {
        OperationItem x = (OperationItem)xx;
        OperationItem y = (OperationItem)yy;
        if (x.getPosHarInt() > y.getPosHarInt()) return 1;
        else if (x.getPosHarInt() < y.getPosHarInt()) return -1;
        else
        {
            DateTime xStartTime = x.startTime;
            DateTime yStartTime = y.startTime;
            if (xStartTime.CompareTo(yStartTime) < 0)
                return -1;
            else if (xStartTime.CompareTo(yStartTime) > 0)
                return 1;
            else
            {
                //kalo waktu mulai sama, maka yg punya video duluan main.
                if (x.hasVideo && !y.hasVideo) return -1;
                else if(!x.hasVideo && y.hasVideo) return 1;
                else
                {
                    //kalo dua2nya punya video, maka yg punya pergerakan unit duluan.
                    if (x.hasUnitMovement && !y.hasUnitMovement) return -1;
                    else if (!x.hasUnitMovement && y.hasUnitMovement) return 1;
                    else return 0;
                }
                //return 0;
            }
        }
    }
}

public class OperationLocation
{
    public Vector3 locationPoint;
    public string locationName;
    public int objInstanceID;

    public OperationLocation()
    {
        locationPoint = Vector3.zero;
        locationName = "";
        objInstanceID = 0;
    }

    public OperationLocation(Vector3 v3, string nm, int instID)
    {
        locationPoint = v3;
        locationName = nm;
        objInstanceID = instID;
    }
    public OperationLocation(Vector3 v3, int instID)
    {
        locationPoint = v3;
        locationName = "";
        objInstanceID = instID;
    }
}
