using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//[DoNotSerialize]
public class BasicUnitMovement : MonoBehaviour
{
    public bool debug;
    
    Transform myTransform;
    
    public float moveSpeed;// = 2.0f;
    private float waterUnitLandDetectRange = 30f;
    public static float UNIT_LAUT_Y = 1.7f;
    public static float UNIT_UDARA_Y = 20f;
    public float ATTACK_RANGE = 50f;

    public bool isSelected = false;
    public bool isUnitLaut; //diset di Editor prefabnya
    public bool isUnitUdara; //diset di Editor prefabnya
    public bool isUnitDarat;

    // UNIT MANAGER
    private UnitManager unitManager;
    private GameObject unitManagerObject;

    // WAYPOINTING
    LineRenderer lineRenderer;
    [SerializeThis]
    public Vector3 lastAddedWayPoint; // posisi terakhir unit, untuk kepentingan History
    public int lastAddedWaypointIdx; // indeks history pergerakan unit
    [SerializeThis]
    public List<Vector3> waypoints;
    [SerializeThis]
    public int curWaypointIdx = 0;
    public Vector3 goal;

    // TARGETING
    public List<Vector3> tarpoints;
    [SerializeThis]
    public List<GameObject> tarPointObjects;
    [SerializeThis]
    public int curTarpointIdx = 0;
    private GameObject targetEffectObject; //diambil dari Resources
    private GameObject missileObject;
    private float distMinToTar = float.MaxValue; // jarak minimum unit ke sasaran, diupdate terus

    // BELOK VARS
    private bool belokMode = false;
    private Vector3[] belokPoints;
    private int belokIdx;
    private Vector3 startBelokPoint = Vector3.zero; //mulai belok di mana, catet sekali aja. kalo berkali2 ntar beloknya tetep di ujung juga, biarpun smooth.
    //smooth rotate belok
    private Quaternion startRot;
    private Quaternion rotation;
    private float step;
    private float lastStep;
    private float LINE_WIDTH = 0.7f;
    


    void Start()
    {
        unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
        unitManager = unitManagerObject.GetComponent<UnitManager>();
        
        myTransform = this.transform;

        //if (LevelSerializer.IsDeserializing) return; // skip initialization when loading saved game
        targetEffectObject = Resources.Load("TargetEffect") as GameObject;
        missileObject = Resources.Load("Missile Udara") as GameObject;

        goal = transform.position;
        initWaypoint();
        initTarpoint();
        lastAddedWaypointIdx = 0;

        StartCoroutine(attackMove());

    }

    private void initWaypoint()
    {
        if (waypoints == null)
            waypoints = new List<Vector3>();
        lineRenderer = (this.lineRenderer != null) ? this.lineRenderer : gameObject.GetComponent<LineRenderer>();
        if (lineRenderer != null && waypoints.Count > 0)
        {
            waypoints = waypoints.Distinct<Vector3>().ToList<Vector3>();
            /*
            lineRenderer.SetVertexCount(waypoints.Count + 1);
            //if (lastPoint == Vector3.zero)
            //    { lastPoint = gameObject.transform.position; }

            lineRenderer.SetPosition(0, gameObject.transform.position);
            for (int i = 0, len = waypoints.Count; i < len; i++)
            {
                lineRenderer.SetPosition(i + 1, (Vector3)waypoints[i]);
            }
             * */
            lineRenderer.SetVertexCount(waypoints.Count);
            //if (lastPoint == Vector3.zero)
            //    { lastPoint = gameObject.transform.position; }

            for (int i = 0, len = waypoints.Count; i < len; i++)
            {
                lineRenderer.SetPosition(i, (Vector3)waypoints[i]);
            }
        }
    }

    private void initTarpoint()
    {
        if (tarpoints == null)
            tarpoints = new List<Vector3>();
        if (tarPointObjects == null)
            tarPointObjects = new List<GameObject>();
        if (tarpoints.Count > 0)
        {
            for (int i = 0, len = tarpoints.Count; i < len; i++)
            {
                GameObject go = Instantiate(targetEffectObject, (Vector3)tarpoints[i], targetEffectObject.transform.rotation) as GameObject;
                tarPointObjects.Add(go);
            }
        }
    }

    public void MoveOrder(Vector3 newGoal)
    {
        goal = newGoal;
    }
      
    void FixedUpdate()
    {
        if (MenuUnit.testMovementMode)
        {
            followWaypoint();
        }
        
        if(lineRenderer!=null)
        lineRenderer.enabled = unitManager.IsSelected(this.gameObject);
    }

    public void followWaypoint()
    {
        //Debug.Log("execute movement of: " + gameObject.name);
        if (waypoints.Count > 0)
        {
            //cek if posisi awal sama dengan posisi waypoint terakhir
            if (myTransform.position == waypoints[waypoints.Count - 1])
            {
                //Debug.Log("Udah ada di GOAL!");
                return;
            }
            if (curWaypointIdx < waypoints.Count)
            {

                //water unit handling
                if (isUnitLaut)
                {

                    //Debug.Log("unit laut bergerak");
                    //RaycastHit hit;
                    //Debug.DrawRay(myTransform.position, ((Quaternion.Euler(0, 0, 7)) * myTransform.forward).normalized * waterUnitLandDetectRange, Color.red);

                    /*
                    if (Physics.Raycast(myTransform.position, ((Quaternion.Euler(0, 0, 7)) * myTransform.forward).normalized, out hit, waterUnitLandDetectRange))
                    {
                        if (hit.collider.gameObject.tag == "daratan")
                        {
                            Debug.Log("Daratan!!");
                            return;
                        }
                    }
                     * */
                }

                Vector3 target = waypoints[curWaypointIdx];
                Vector3 moveDir = target - myTransform.position;


                float distToNextPoint = Vector3.Distance(myTransform.position, target);
                //float distToTuj;

                if (distToNextPoint < 10f && curWaypointIdx < waypoints.Count - 1)
                {
                    if (startBelokPoint == Vector3.zero)
                    {
                        startBelokPoint = myTransform.position;
                        //init rotate
                        if (isUnitUdara)
                        {
                            startRot = myTransform.rotation;
                            rotation = Quaternion.AngleAxis(40f, myTransform.forward);
                            step = 0.0f;
                        }
                    }
                    belokMode = true;
                }
                else
                {
                    belokMode = false;
                    belokIdx = 0;
                }

                if (belokMode)
                {

                    belokPoints = getBelokPoints(startBelokPoint, target, 7f);
                    //Debug.Log("Belook..");
                    if (belokPoints.Length > 0)
                    {
                        if (belokIdx < belokPoints.Length)
                        {
                            Vector3 targetBelok = belokPoints[belokIdx];
                            myTransform.LookAt(targetBelok);
                            myTransform.position = Vector3.MoveTowards(myTransform.position, targetBelok, Time.deltaTime * moveSpeed);

                            if (isUnitUdara)
                            {
                                //rotate banking
                                float rotAm = 0f;
                                if (myTransform.InverseTransformPoint(target).x < 0.0f)
                                {
                                    rotAm = -70f; //kiri
                                }
                                else if (myTransform.InverseTransformPoint(target).x > 0.0f)
                                {
                                    rotAm = 70f; //kanan
                                }



                                Debug.DrawRay(myTransform.position, myTransform.forward * 30, Color.red);
                                step += Time.deltaTime * moveSpeed; //1.0f itu rate
                                transform.RotateAround(myTransform.position, myTransform.forward,
                                   rotAm);//* (Mathf.SmoothStep(0.0f, 1.0f, step) - lastStep));

                                lastStep = Mathf.SmoothStep(0.0f, 1.0f, step);
                                //transform.rotation = startRot * Quaternion.Slerp(Quaternion.identity,
                                //rotation,Mathf.SmoothStep(0.0f, 1.0f, step));
                                //end rotate
                            }
                            float dist = Vector3.Distance(myTransform.position, targetBelok);
                            if (dist <= 0.5f)
                                belokIdx++;
                            if (belokIdx >= belokPoints.Length)
                            {
                                curWaypointIdx++; //pas nyampe, ganti target, kalo enggak, dia bisa bolak-balik doang di tempat
                                belokMode = false;
                                belokIdx = 0;
                                startBelokPoint = Vector3.zero;
                                //reinit rotate
                                startRot = Quaternion.identity;
                                step = 0.0f;
                            }

                        }

                    }

                } //end belokMode
                else
                {
                    //Debug.Log("Lurus..");

                    myTransform.LookAt(target);
                    myTransform.position = Vector3.MoveTowards(myTransform.position, target, Time.deltaTime * moveSpeed);

                    if (distToNextPoint <= 0.1f)
                    {
                        curWaypointIdx++;
                        if (curWaypointIdx < waypoints.Count)
                            myTransform.LookAt(waypoints[curWaypointIdx]);
                    }

                } //end lurus

                ////cek target serangan
                //if (tarpoints.Count > 0)
                //{
                //    if (curTarpointIdx < tarpoints.Count)
                //    {

                //        Vector3 curTarget = tarpoints[curTarpointIdx];
                //        float distToTar = Vector3.Distance(myTransform.position, curTarget);
                //        //Debug.Log("dist: " + distToTar.ToString());

                //        StartCoroutine(fire(curTarget));


                //    }
                //}

            }// end curwpidx < count

        }

    }

    private Vector3[] getBelokPoints(Vector3 start, Vector3 edge, float smoothness)
    {
        if (curWaypointIdx >= waypoints.Count - 1) return new Vector3[0];

        // jadi ada 3 point, belokPoint, targetPoint, dan tujuanPoint

        Vector3 end = 10f * Vector3.Normalize(waypoints[curWaypointIdx + 1] - edge) + edge;
        float distToTuj = Vector3.Distance(myTransform.position, end);

        //Debug.DrawRay(Vector3.zero, start, Color.red);
        //Debug.DrawRay(Vector3.zero, end, Color.red);

        //ketiganya di-smooth-kan biar beloknya ga tajem2 amat, jadi ga nyentuh target
        Vector3[] belokPoints = new Vector3[] { start, edge, end };


        int pointsLength = 3; // jadi ada 3 point, belokPoint, targetPoint, dan tujuanPoint
        int curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1; // banyaknya control points
        List<Vector3> curvedPoints = new List<Vector3>(curvedLength); // list hasil
        List<Vector3> points;

        float t = 0.0f;
        // pioc = pointInTimeOnCurve
        for (int pitoc = 0; pitoc < curvedLength + 1; pitoc++)
        {
            t = myInverseLerp(0, curvedLength, pitoc);

            points = new List<Vector3>(belokPoints);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return curvedPoints.ToArray();
    }

    public void addWaypoint(Vector3 wpItem)
    {
        if (waypoints == null) return;
        if (isUnitLaut) { wpItem.y = UNIT_LAUT_Y; }
        if (isUnitUdara) { wpItem.y = UNIT_UDARA_Y; }

        goal = wpItem;

        lineRenderer = (this.lineRenderer!=null)?this.lineRenderer:gameObject.GetComponent<LineRenderer>();
        lineRenderer.SetWidth(LINE_WIDTH, LINE_WIDTH);
        lineRenderer.SetVertexCount(waypoints.Count + 2);
        if (lastAddedWayPoint == Vector3.zero)
            lastAddedWayPoint = myTransform.position;
        if (waypoints.Count > 0)
            lastAddedWayPoint = waypoints[waypoints.Count - 1];
        lineRenderer.SetPosition(waypoints.Count, lastAddedWayPoint);
        lineRenderer.SetPosition(waypoints.Count + 1, goal);
        lastAddedWayPoint = goal;
        lastAddedWaypointIdx++;

        waypoints.Add(wpItem);

        //add to history
        HistoryManager.addToHistory(new HistoryItem(HistoryManager.HISTORY_ADD_WAYPOINT, getCleanName(myTransform,"prefab"), getCleanName(myTransform,"prefab"), wpItem));
        
    }

    public void addTargetpoint(Vector3 tp)
    {
        if (tarpoints == null) return;
        GameObject go = Instantiate(targetEffectObject, tp, targetEffectObject.transform.rotation) as GameObject;
        tarPointObjects.Add(go);
        
        tarpoints.Add(tp);

        //add to history
        HistoryManager.addToHistory(new HistoryItem(HistoryManager.HISTORY_ADD_TARPOINT, getCleanName(myTransform,"name"), getCleanName(myTransform,"prefab"), tp));

    }

    public void removeLastWayPoint()
    {
        if (waypoints.Count > 0)
        {
            waypoints.RemoveAt(waypoints.Count - 1);
            lastAddedWayPoint = (Vector3)waypoints[waypoints.Count - 1];
            lastAddedWaypointIdx--;
        }
    }

    public string getCleanName(Transform myTransform, string which)
    {
        return HistoryManager.getCleanName(myTransform, which);
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

    IEnumerator attackMove()
    {
        while (true)
        {
            yield return null;
            if (MenuUnit.testMovementMode)
            {
                if (tarpoints.Count > 0)
                {
                    //foreach (Vector3 target in tarpoints)
                    //{
                    /* serangan serial */
                    //yield return StartCoroutine(fireMissile(target)); 
                    /* serangan paralel */

                    while (curTarpointIdx < tarPointObjects.Count)
                    {
                        //RadicalRoutineExtensions.StartExtendedCoroutine(gameObject,(fireMissile(tarPointObjects[curTarpointIdx]))); 
                        StartCoroutine(fireMissile(tarPointObjects[curTarpointIdx]));
                        curTarpointIdx++;
                    }
                }

            }

        }
    }

    IEnumerator fireMissile(GameObject targetObj)
    {
        //Debug.Log("TEMBAK! : " + targetObj.transform.position.ToString());
        Vector3 target = targetObj.transform.position;
        distMinToTar = Vector3.Distance(myTransform.position, target);

        while (true)
        {

            yield return null;

            float distUnitToTar = Vector3.Distance(myTransform.position, target);

            //Debug.LogError("DistUnitToTar: " + distUnitToTar);
            Debug.DrawRay(myTransform.position, (target - myTransform.position).normalized * ATTACK_RANGE, Color.yellow);

            if (distUnitToTar <= ATTACK_RANGE)// && distUnitToTar<=distMinToTar)
            {
                //Vector3 target;
                GameObject missile = Instantiate(missileObject, myTransform.position, myTransform.rotation) as GameObject;
                Transform mt = missile.transform;

                while (true)
                {
                    yield return null;
                    if (mt != null)
                    {
                        mt.LookAt(target);
                        //Debug.DrawRay(mt.position, mt.forward * 30);
                        mt.position = Vector3.MoveTowards(mt.position, target, Time.deltaTime * moveSpeed * 5f);


                        float distToTar = Vector3.Distance(mt.position, target);


                        //mt.Translate(mt.forward * moveSpeed * Time.deltaTime);
                        if (distToTar <= 0.1f)
                        {
                            //isMoving = false;
                            //break; // kalo break doang, nanti dia nembak berkali2
                            Destroy(targetObj);//hapus target object kalo udah kena

                            Destroy(missile); // missilenya juga lah..
                            //break;
                            yield return null; // kalo diyield, dia nembak sekali aja begitu kena, beres.
                        }
                    }
                    //Debug.Log("firing to " + target.ToString());

                }//endwhile
            }//endif

        }//endwhile

    }

    private float myInverseLerp(int p, int curvedLength, int pitoc)
    {
        return MainScript.myInverseLerp(p, curvedLength, pitoc);
    }


    /* end of class */
}
