using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[SerializeAll]
public class UnitManager : MonoBehaviour
{

    public List<GameObject> selectedUnits;
    public bool debug = true;

    LineRenderer lineRenderer;
    public int lengthOfLineRenderer = 20;
    private string testStatus = "";
    private bool menuVisible = false;
    private Vector3 screenPos;
    private float menuH;
    private float menuW;
    private float menuX;
    private float menuY;
    private float menuItemX;
    private float menuItemY;
    private float menuItemW;
    private float menuItemH;
    private string[] unitOrderList = new string[] { "Delete"};
    private Vector3 selectedUnitPos;
    private List<GameObject> unitsToBeProcessed;
    public static string UNIT_TAG = "Building";
    private GUIStyle historyStyle;
    private bool targettingMode = false; //jika true maka klik kanan jadi menentukan sasaran, bukan waypoint

    private bool mouseOverGUI = false; //pendanda apakah mouse pointer ada di atas GUI

    void Start()
    {
        if (LevelSerializer.IsDeserializing) return;
        LevelSerializer.AddPrefabPath("Prefabs/");

        selectedUnits.Clear();

        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        //lineRenderer.SetColors(c1, c2);
        //lineRenderer.SetWidth(0.2F, 0.2F);
        //lineRenderer.SetVertexCount(lengthOfLineRenderer);
        //testAddUnit();
    }

    public IEnumerator executeMovement()
    {
        float startTime;
        Vector3 startPoint;
        foreach (GameObject go in selectedUnits)
        {
            BasicUnitMovement bum = (BasicUnitMovement)go.GetComponent(typeof(BasicUnitMovement));
            var time = 5.0;
            if (bum != null)
            {
                startPoint = bum.gameObject.transform.position;
                for (int i = 0; i < bum.waypoints.Count; i++)
                {
                    //yield MoveObject.use.Translation(bum.gameObject.transform, bum.waypoints[i], bum.waypoints[i+1], time, MoveType.Time);

                    //startTime = Time.time;
                    //Vector3.Lerp(startPoint, endPoint, i);

                    //go.transform.LookAt(bum.waypoints[i]);
                    //go.transform.Translate(Vector3.forward * bum.moveSpeed * Time.deltaTime);
                    //go.transform.position += (wp - transform.position).normalized * bum.moveSpeed * Time.deltaTime;
                    //StartCoroutine(moveToPoint(go,bum.waypoints[i],bum));
                    go.transform.position = Vector3.Lerp(startPoint, bum.waypoints[i], bum.moveSpeed * Time.deltaTime);
                    startPoint = bum.waypoints[i];

                }
            }
        }
        yield return null;
    }

    private IEnumerator moveToPoint(GameObject go, Vector3 wp, BasicUnitMovement bum)
    {
        go.transform.LookAt(wp);
        go.transform.Translate(Vector3.forward * bum.moveSpeed * Time.deltaTime);
        go.transform.position += (wp - transform.position).normalized * bum.moveSpeed * Time.deltaTime;
        yield return new WaitForSeconds(0.1f);
    }

    void Update()
    {
        if (mouseOverGUI) return;
        if (Camera.main.enabled)
        {
            Ray ray;
            RaycastHit hit;

            if (Input.GetMouseButtonDown(0))
            {

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (debug)
                    {
                        //Debug.Log ("You clicked " + hit.collider.gameObject.name, hit.collider.gameObject);
                    }
                    /*
                    INI DICURIGAI SEBAGAI SEBAB BUG DIMANA SI UNIT NGOMBANG-AMBING GA JELAS
					hit.transform.gameObject.SendMessage ("Clicked", hit.point, SendMessageOptions.DontRequireReceiver);
                    */
                    if (hit.collider.gameObject.tag != UNIT_TAG) // cek apakah objek yg diselect adalah Unit, kalau tidak, deselect
                    {
                        Debug.Log("DESELECT");
                        DeselectAllUnits();
                    }
                }

            }
            if (Input.GetMouseButtonDown(1))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Debug.DrawRay(ray.GetPoint(200), Vector3.down * 30, Color.yellow);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (debug)
                    {
                        //Debug.Log ("You right clicked " + hit.collider.gameObject.name, hit.collider.gameObject);
                    }

                    if (selectedUnits.Contains(hit.collider.gameObject))
                    {
                        unitsToBeProcessed = new List<GameObject>(selectedUnits);

                        selectedUnitPos = hit.point;
                        menuVisible = true;
                    }
                    else
                    {

                        //hit.transform.gameObject.SendMessage("RightClicked", hit.point, SendMessageOptions.DontRequireReceiver);
                        if (selectedUnits.Count > 0)
                        {
                            string acter = "";
                            //ActionHistory.idx = 0;
                            foreach (GameObject go in selectedUnits)
                            {
                                BasicUnitMovement bum = (BasicUnitMovement)go.GetComponent(typeof(BasicUnitMovement));
                                if (bum != null)
                                {
                                    //bum.goal = hit.point;
                                    //bum.action = true;
                                    if (acter == "")
                                        acter += bum.gameObject.name + "-" + bum.gameObject.GetInstanceID().ToString();

                                    //lineRenderer = bum.gameObject.GetComponent<LineRenderer>();
                                    //lineRenderer.SetVertexCount(bum.idx + 2);
                                    //if (bum.lastPoint == Vector3.zero) 
                                    //    { bum.lastPoint = bum.gameObject.transform.position; }
                                    //lineRenderer.SetPosition(bum.idx, bum.lastPoint);
                                    //lineRenderer.SetPosition(bum.idx+1, bum.goal);
                                    //bum.lastPoint = bum.goal;
                                    //bum.idx++;
                                    if (targettingMode)
                                    {
                                        Debug.Log("targetting mode!");
                                        bum.addTargetpoint(hit.point);
                                    }
                                    else
                                    {

                                        if (bum.isUnitLaut && hit.collider.gameObject.tag == "daratan")
                                            return;
                                        bum.addWaypoint(hit.point);
                                    }
                                }
                            }



                        }
                    }
                }
            }

            /*
            m = Input.mousePosition;
            //m = new Vector3(m.x,m.y,transform.position.y);
            p = Camera.main.ScreenToWorldPoint(m);
			
			
            if (Input.GetMouseButtonDown(0))
            {
                //if (actionMenuVisible) { actionMenuVisible = false; }
                //else { actionMenuVisible = true; }
                Clicked();
            }
            if (Input.GetMouseButtonDown(1))
            {
                // if menu not visible, do nothing
                //if (actionMenuVisible)
                //{
                RightClicked(new Vector3(p.x, 10, p.z));
                //if (isCAPClicked)
                //{
                //mulai gerak;
                //   RightClicked(Input.mousePosition);
                //   isCAPClicked = false;
                actionMenuVisible = false;
                //}
                //  if isCAPClicked
                //   setGoal; allowMove;
                //  else 
                //   do nothing
                //}
            }*/
        }
    }

    public bool IsSelected(GameObject unit)
    {
        if (selectedUnits.Contains(unit))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SelectSingleUnit(GameObject unit)
    {
        selectedUnits.Clear();
        if (unit.tag == UNIT_TAG)
            selectedUnits.Add(unit);
        //Debug.Log(selectedUnits);
    }

    public void SelectAdditionalUnit(GameObject unit)
    {
        if (unit.tag == UNIT_TAG)
        {
            selectedUnits.Add(unit);
        }
    }

    public void DeselectAllUnits()
    {
        selectedUnits.Clear();
    }

    public List<GameObject> GetSelectedUnits()
    {
        return selectedUnits;
    }

    public bool destroySelectedUnits()
    {
        Debug.Log("BEGIN DESTROY: " + unitsToBeProcessed.Count + " units");
        foreach (GameObject gob in unitsToBeProcessed)
        {
            //GameObject g = (GameObject)Instantiate(gob);
            selectedUnits.Remove(gob);
            Destroy(gob);

        }
        unitsToBeProcessed.Clear();
        Debug.Log("DESTROY SUCCESS!!!");
        return true;
    }

    //history operation
    public bool executeHistoryItem(HistoryItem h)
    {
        bool ret = false;
        switch (h.command)
        {
            case "add":
                GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/" + h.prefabName, typeof(GameObject)));

                if (g != null)
                {
                    g.name = h.objectName;
                    g.transform.position = h.initialPos;
                    ret = true;
                }
                break;
            case "edit":
                if (h.objectName != null)
                {

                }
                break;
            default:
                break;
        }
        return ret;
    }

    //testing
    public void testAddUnit()
    {
        foreach (HistoryItem i in HistoryManager.testOperationArray)
        {
            if (executeHistoryItem(i))
            {
                testStatus += "\noperasi \"" + i.command + "\" objek \"" + i.objectName + "\" berhasil.";
            }
        }
    }

    // GUI operations
    void OnGUI()
    {
        if (menuVisible && Camera.main.enabled)
            showSelectedUnitMenu();
        else
            mouseOverGUI = false;

        if (HistoryManager.showHistory)
            showHistoryGUI();

        //GUI.Box(new Rect(400, 500, 300, 100), testStatus);

    }

    private float selectedUnitUdaraHeight = BasicUnitMovement.UNIT_UDARA_Y; // nilai ketinggian unit udara berdasarkan slider
    private float sliderUdaraH, btKembaliH; //tambahan ketinggian menu, opsional
    private float btMenuLain; //tambahan menu lain opsional
    void showSelectedUnitMenu()
    {
        //screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos = Camera.main.WorldToScreenPoint(selectedUnitPos);

        menuH = 30f + 25f * ((float)unitOrderList.Length) + sliderUdaraH + btMenuLain + btKembaliH;
        menuW = 300;
        menuX = screenPos.x;
        menuY = Screen.height - screenPos.y - menuH;

        //Debug.Log("target is " + screenPos.x +","+screenPos.y+","+screenPos.z+ " pixels from the left");
        menuItemX = screenPos.x + 5;
        menuItemY = menuY + 20;
        menuItemW = menuW - 10;
        menuItemH = 25;

        Rect boxRect = new Rect(menuX, menuY, menuW, menuH);
        GUI.Box(boxRect, "menu unit");

        //cek apalah mouse ada di dalam rect, utk mencegah deselect yg tidak diinginkan akibat click yg nembus GUI ke objek di belakangnya
        if (boxRect.Contains(Event.current.mousePosition))
        {
            mouseOverGUI = true;
        }
        else
        {
            mouseOverGUI = false;
        }

        foreach (string order in unitOrderList)
        {
            if (GUI.Button(new Rect(menuItemX, menuItemY, menuItemW, menuItemH), order))
            {
                switch (order)
                {
                    case "Delete":
                        if (destroySelectedUnits()) { menuVisible = false; }
                        break;
                    default:
                        break;
                }
            }
            menuItemY += menuItemH + 2;
        }
        // cek if semua yg diselect adalah unit udara
        bool showAturKetinggian = true;
        foreach (GameObject gob in unitsToBeProcessed)
        {
            if (!gob.GetComponent<BasicUnitMovement>().isUnitUdara)
                showAturKetinggian = false;
            //else
                //selectedUnitUdaraHeight = gob.transform.position.y;

            gob.transform.position = new Vector3(gob.transform.position.x, selectedUnitUdaraHeight, gob.transform.position.z);    
        }
        if (showAturKetinggian)
        {
            sliderUdaraH = 50;
            //if (GUI.Button(new Rect(menuItemX, menuItemY, menuItemW, menuItemH), "menu ketinggian yohoo"))
            //{
            //}
            GUI.Label(new Rect(menuItemX, menuItemY, menuItemW, menuItemH), "Ketinggian Unit: " + selectedUnitUdaraHeight + " km");
            menuItemY += menuItemH + 2;
            selectedUnitUdaraHeight = GUI.HorizontalSlider(new Rect(menuItemX, menuItemY, menuItemW, menuItemH), selectedUnitUdaraHeight, 0.0f, 100.0f);
            menuItemY += menuItemH + 2;
        }
        btMenuLain = 25;
        if (GUI.Button(new Rect(menuItemX, menuItemY, menuItemW, menuItemH), "Set Sasaran Serangan"))
        {
            targettingMode = true;
            Debug.Log("true targetting!");
        }
        menuItemY += menuItemH + 2;

        btKembaliH = 25;
        if (GUI.Button(new Rect(menuItemX, menuItemY, menuItemW, menuItemH), "Kembali"))
        {
            targettingMode = false;
            menuVisible = false;
        }
    }

    private Vector2 scrollPosition = Vector2.zero;
    private float hScrollvH = 200;
    private int lastHistoryCount = 0;
    
    void showHistoryGUI()
    {
        historyStyle = new GUIStyle(GUI.skin.button);
        historyStyle.fontSize = 11;

        float hisPosW = 300;
        float hisPosH = 250;
        float hisPosX = Screen.width - hisPosW;
        float hisPosY = 200;
        float hisItemH = 40;
        float hisItemW = hisPosW * 0.9f;
        GUI.Box(new Rect(hisPosX, hisPosY, hisPosW, hisPosH), "Histori Aksi");

        float hisItemX = 0; //relative to scroolview
        float hisItemY = 0; //relative to scrollview
        int len;
        scrollPosition = GUI.BeginScrollView(
                            new Rect(hisPosX, hisPosY + 20, hisPosW, hisPosH),
                            scrollPosition,
                            new Rect(-10, 0, hisPosW - 10, hScrollvH), false, false);
        for (int i = 0; i < HistoryManager.historyList.Count; i++)
        {
            len = HistoryManager.historyList.Count;
            if (GUI.Button(new Rect(hisItemX, hisItemY, hisItemW, hisItemH), "[" + i + "] " + HistoryManager.historyList[i].ToString(), historyStyle))
            {
                //foreach history i sampe len-i
                //unco action[i]
                //for(int j=i, lenj=len-i; j<lenj; j++){
                //undoAction(hitem);
                //}
                HistoryManager.undoHistory((HistoryItem)HistoryManager.historyList[i]);
            }
            hisItemY += hisItemH + 1;
            hScrollvH = ((hisItemH + 1) * (i + 1) <= hScrollvH ? hScrollvH : (hisItemH + 1) * (i + 1));

        }
        GUI.EndScrollView();

        /* cek jika terjadi penambahan item, set scroll position ke bawah.
         * kalo ga dicek gini scrollpositionY nya stuck di bawah terus walaupun perhitungannya betul.
         * */
        if (lastHistoryCount != HistoryManager.historyList.Count)
        {
            float newScrollPosY = (HistoryManager.historyList.Count * (hisItemH + 1) - hisPosH);
            scrollPosition.y = newScrollPosY >= 0 ? newScrollPosY : scrollPosition.y;
            lastHistoryCount = HistoryManager.historyList.Count;
        }

    }
}
