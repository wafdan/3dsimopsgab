using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[DoNotSerialize]
public class BasicUnitMovement : MonoBehaviour
{

    public float moveSpeed = 2.0f;
    //public float goalRadius = 0.1f;
    Transform myTransform;

    public bool action = false;
    public bool isSelected = false;
    public bool isUnitLaut; //diset di Editor prefabnya
    public Vector3 goal;

    public GameObject moveEffectObject;
    private UnitManager unitManager;
    Vector3 posisi;
    private bool debug = true;
    string seluns = "";
    private GameObject unitManagerObject;

    //Action
    private Vector3 screenPos = Vector3.zero;
    string selectedFrom;
    string selectedGoal;
    string selectedAction;

    //GUI menu
    private GUIStyle mStatStyle;
    private UnitOrder unit;
    private string[] unitOrderList;
    private string missionStatus;
    private bool actionMenuVisible = false;
    private bool missionStatusVisible = false;
    private int statW;
    private int statH;
    private int statX;
    private int statY;
    private int menuH;
    private int menuW;
    private float menuX;
    private float menuY;
    private float menuItemX;
    private float menuItemY;
    private int menuItemW;
    private int menuItemH;
    private bool isCAPClicked;
    private Vector3 m;
    private Vector3 p;
    [SerializeThis]
    public Vector3 lastPoint; // posisi terakhir unit, untuk kepentingan History
    public int idx; // indeks history pergerakan unit
    [SerializeThis]
    public List<Vector3> waypoints;
    private int curWaypointIdx = 0;
    private Vector3 velocity;

    void Start()
    {
        //waypoints = new ArrayList(new Vector3[] { new Vector3(-114, 9, 383), new Vector3(-176, 9, 288) });

        unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
        unitManager = unitManagerObject.GetComponent<UnitManager>();
        unit = (UnitOrder)MainScript.unitOrders["Sukhoi"];
        unitOrderList = unit.orderList;
        initGUIStyle();
        myTransform = transform;

        //if (LevelSerializer.IsDeserializing) return; // skip initialization when loading saved game

        goal = transform.position;
        initWaypoint();
        idx = 0;

    }

    private void initWaypoint()
    {
        if (waypoints == null)
            waypoints = new List<Vector3>();
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer != null && waypoints.Count > 0)
        {
            waypoints = waypoints.Distinct<Vector3>().ToList<Vector3>();
            lineRenderer.SetVertexCount(waypoints.Count + 1);
            //if (lastPoint == Vector3.zero)
            //    { lastPoint = gameObject.transform.position; }

            lineRenderer.SetPosition(0, gameObject.transform.position);
            for (int i = 0, len = waypoints.Count; i < len; i++)
            {
                lineRenderer.SetPosition(i + 1, (Vector3)waypoints[i]);
            }
        }
    }

    public void MoveOrder(Vector3 newGoal)
    {
        goal = newGoal;
    }

    private float waterUnitLandDetectRange = 30f;
    public static float UNIT_LAUTY = 4;
	
	void Update()
    {
        if (MenuUnit.testMovementMode)
        {
            Debug.Log("execute movement of: " + gameObject.name);
            if (waypoints.Count > 0)
            {
                if (curWaypointIdx < waypoints.Count)
                {
                    
                    //water unit handling
                    if (isUnitLaut)
                    {
                        
                        Debug.Log("unit laut bergerak");            
                        RaycastHit hit;
                        Debug.DrawRay(myTransform.position, ((Quaternion.Euler(0, 0,7)) * myTransform.forward).normalized * waterUnitLandDetectRange, Color.red);

                        if (Physics.Raycast(myTransform.position, ((Quaternion.Euler(0,0,7)) * myTransform.forward).normalized, out hit, waterUnitLandDetectRange))
                        {
                            if (hit.collider.gameObject.tag == "daratan")
                            {
                                Debug.Log("Daratan!!");  
                                return;
                            }
                        }
                    }

                    Vector3 target = waypoints[curWaypointIdx];
                    Vector3 moveDir = target - myTransform.position;
                    velocity = rigidbody.velocity;

                    myTransform.LookAt(target);
                    myTransform.position = Vector3.MoveTowards(myTransform.position, target, Time.deltaTime * moveSpeed);

                    if (Vector3.Distance(myTransform.position, target) <= 0.1f)
                    {
                        curWaypointIdx++;
                        if (curWaypointIdx < waypoints.Count)
                            myTransform.LookAt(waypoints[curWaypointIdx]);
                    }

                }
            }
        }
        else
        {
            //Debug.Log("stop movement of: " + gameObject.name);
        }
        //Debug.DrawRay(transform.position,Vector3.back*100,Color.green);

        /* INI DITUNDA DULU, JANGAN DIHAPUS!
        if (action == true && isSelected == true)
        {
            if (!checkIfGoalSame(transform.position, goal))
            {
                myTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

                myTransform.LookAt(goal);
                transform.position += (goal - transform.position).normalized * moveSpeed * Time.deltaTime;
            }
        }
        */

        //foreach(Collider obj in Physics.OverlapSphere(goal, goalRadius)) {
        //	if(obj.gameObject == gameObject) {
        //		transform.position = goal;
        //	}
        //}
        updateSeluns();
    }

    public void addWaypoint(Vector3 wpItem)
    {
        if (waypoints == null) return;
        if (isUnitLaut) { wpItem.y = UNIT_LAUTY; }

        goal = wpItem;

        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(waypoints.Count + 2);
        if (lastPoint == Vector3.zero)
            lastPoint = gameObject.transform.position;
        if (waypoints.Count > 0)
            lastPoint = waypoints[waypoints.Count - 1];
        lineRenderer.SetPosition(waypoints.Count, lastPoint);
        lineRenderer.SetPosition(waypoints.Count + 1, goal);
        lastPoint = goal;
        idx++;

        waypoints.Add(wpItem);

        //add to history
        //prepare to add to history
        string name = transform.collider.gameObject.name;
        int idxclone = transform.collider.gameObject.name.IndexOf("(Clone)");
        string prefabName = (idxclone < 0) ? name : name.Remove(idxclone, "(Clone)".Length);
        int id = transform.collider.gameObject.GetInstanceID();
        //string newName = prefabName + "" + id;
        string newName = name;
        HistoryManager.addToHistory(new HistoryItem(HistoryManager.HISTORY_ADD_WAYPOINT, newName, prefabName, wpItem));
        //change name of the new added unit
        //collider.gameObject.name = newName;
    }

    public void removeLastWayPoint()
    {
        if (waypoints.Count > 0)
        {
            waypoints.RemoveAt(waypoints.Count - 1);
            lastPoint = (Vector3)waypoints[waypoints.Count - 1];
            idx--;
        }
    }

    void Clicked()
    {
        //Debug.Log("CLICKED..");
        isSelected = true;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            unitManager.SelectAdditionalUnit(gameObject);
        }
        else
        {

            unitManager.SelectSingleUnit(gameObject);
        }
    }

    void RightClicked(Vector3 clickPosition)
    {
        /* MOVE ORDER DINONAKTIFKAN DAHULU
        foreach (GameObject unit in unitManager.GetSelectedUnits())
        {
            if (!checkIfGoalSame(transform.position, clickPosition))
            {
                action = true;
                actionMenuVisible = false;
                unit.SendMessage("MoveOrder", clickPosition);
                Instantiate(moveEffectObject, clickPosition, moveEffectObject.transform.rotation);

            }
        }
         * */
    }

    void OnGUI()
    {
        //showSelectedUnits();
        //showActionMenu();
        //showMissionStatus();
    }

    private void showMissionStatus()
    {
        if (missionStatusVisible)
        {
            statW = 400;
            statH = 300;
            statX = 0;
            statY = Screen.height - statH;

            missionStatus = "Status Misi unit: " + unit.name
                            + "\nMisi\t\t: " + selectedAction
                            + "\nDari\t\t: " + selectedFrom
                            + "\nTarget\t: " + selectedGoal;
            //status = "myTransform: "+(int)myTransform.position.x+", "+myTransform.position.y+", "+myTransform.position.z+"\ngoal: "+(int)goal.x+", "+goal.y+", "+goal.z+"";

            GUI.Box(new Rect(statX, statY, statW, statH), missionStatus, mStatStyle);

        }
    }

    private void showActionMenu()
    {
        if (actionMenuVisible && Camera.main.enabled)
        {

            screenPos = Camera.main.WorldToScreenPoint(transform.position);


            menuH = 30 + 25 * (unitOrderList.Length);
            menuW = 300;
            menuX = screenPos.x;
            menuY = Screen.height - screenPos.y - menuH;

            //Debug.Log("target is " + screenPos.x +","+screenPos.y+","+screenPos.z+ " pixels from the left");
            menuItemX = screenPos.x + 5;
            menuItemY = menuY + 20;
            menuItemW = menuW - 10;
            menuItemH = 25;

            GUI.Box(new Rect(menuX, menuY, menuW, menuH), "Perintah " + unit.name);

            foreach (string order in unitOrderList)
            {
                if (GUI.Button(new Rect(menuItemX, menuItemY, menuItemW, menuItemH), order))
                {

                    selectedAction = order;
                    if (order.Contains("Patrol"))
                    {
                        isCAPClicked = true;
                    }
                    else
                    {
                        //theSpotlight.SetActive (false);
                    }
                }
                menuItemY += menuItemH + 2;
            }
        }//endif menuVisible && mainCamera.enabled
    }

    void updateSeluns()
    {
        List<GameObject> units = unitManager.GetSelectedUnits();
        seluns = "";
        foreach (GameObject unit in units)
        {
            seluns += unit.name + "\n";
        }
        seluns += "\ngoal        : (" + goal.x + ", " + goal.y + ", " + goal.z + ")";
        seluns += "\ntransformpos: (" + myTransform.position.x + ", " + myTransform.position.y + ", " + myTransform.position.z + ")";
        seluns += "\nWAYPOINTS (" + waypoints.Count + "):\n";
        foreach (Vector3 v in waypoints)
        {
            seluns += (v).ToString() + "\n";
        }
        seluns += "LASTPOINT: " + lastPoint + "\n";
    }

    private void showSelectedUnits()
    {
        if (debug)
        {
            GUI.Box(new Rect(700, 100, 300, 800), "Selected Unit:\n" + seluns);
        }
    }

    private void initGUIStyle()
    {
        mStatStyle = new GUIStyle();
        mStatStyle.alignment = TextAnchor.UpperLeft;
        mStatStyle.margin = new RectOffset(40, 10, 10, 10);
        mStatStyle.fontSize = 20;
        mStatStyle.fontStyle = FontStyle.Bold;
        mStatStyle.normal.textColor = Color.white;
    }

    public static bool checkIfGoalSame(Vector3 a, Vector3 b)
    {
        if ((int)a.x == (int)b.x && (int)a.y == (int)b.y)
            return true;
        else
            return false;
    }
}
