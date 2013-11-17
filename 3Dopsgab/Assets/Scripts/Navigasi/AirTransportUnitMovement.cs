using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//[DoNotSerialize]
public class AirTransportUnitMovement : BasicUnitMovement
{

    Animation animationEngine;
    private bool isCargo = false;
    private bool targetInLeft = false; //posisi target apakah di kiri unit atau kanan unit, khusus penerjunan.
    GameObject paraTroopObject;
    GameObject infanteriObject;

    void Start()
    {
        isCargo = GetComponent<UnitAction>().UNIT_AIRCARGO;
        paraTroopObject = Resources.Load("Models/Miscellaneous/Parachute_ready") as GameObject;
        infanteriObject = Resources.Load("Models/Units/Infantri/Soldier_prefab") as GameObject;
        initEngineAnimation();
        base.Start();
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void adjustMainGun()
    {
        if (isUnitDarat)
        {
            if (turret != null)
            {
                //Vector3 turTarget = (tarpoints.Count>0)?tarpoints[0]:Vector3.zero;
                Vector3 turTarget = getNearestTarget(tarpoints);
                if (turTarget != Const.FARTHEST_VECTOR)
                {
                    //Debug.Log("Target turret: " + turTarget.ToString());
                    //turret.transform.LookAt(turTarget);
                    //Debug.DrawRay(turret.transform.position, turTarget - turret.transform.position, Color.green);
                    //Debug.DrawRay(barrel.transform.forward, Vector3.forward * 10, Color.red);
                    //Debug.DrawRay(barrel.transform.localPosition, Vector3.up * 30, Color.green);
                    //Debug.DrawRay(barrel.transform.localPosition, Vector3.right * 30, Color.cyan);

                    // Rotation (Yaw) of the turret
                    //Vector3 eulerAngles = turret.transform.rotation.eulerAngles;
                    //eulerAngles = new Vector3(0, eulerAngles.y, 0);
                    //turret.transform.rotation = Quaternion.Euler(eulerAngles);

                    Vector3 targetVectorTurret = turTarget - turret.transform.position;
                    //targetVectorTurret.y = 0;
                    //Quaternion rot = Quaternion.LookRotation(targetVectorTurret);
                    Quaternion rot = Quaternion.LookRotation(targetVectorTurret);

                    //rot.x = 0; rot.z = 90;

                    turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rot, Time.deltaTime * turretRotationSpeed);
                    Vector3 eulerAngles = turret.transform.rotation.eulerAngles;
                    eulerAngles = new Vector3(0, eulerAngles.y, -90);
                    turret.transform.rotation = Quaternion.Euler(eulerAngles);

                    //Debug.Log("turret rot = " + turret.transform.rotation.eulerAngles.ToString());
                    return;

                    ////////Vector3 localTurretHeading = turret.transform.InverseTransformDirection(targetVectorTurret);
                    ////////float requiredYaw = Mathf.Rad2Deg * Mathf.Atan2(localTurretHeading.x, localTurretHeading.z);
                    //var requiredPitch : float = Vector3.Angle(Vector3.up, localTurretHeading) - 90.0;
                    //var deltaYaw = (requiredYaw / 10) * turretRotationForce * Time.deltaTime;
                    //deltaYaw = Mathf.Clamp(deltaYaw, -2.0, 2.0);
                    ////////var deltaYaw = Mathf.Clamp((requiredYaw / 10f) * moveSpeed * 1000, -45.0f, 45.0f) * Time.deltaTime;
                    //turret.transform.Rotate(Vector3.up, deltaYaw, Space.Self);
                    ////////turret.transform.Rotate(Vector3.right, deltaYaw, Space.Self); // harusnya UP, tapi karena modelnya jungkir jadinya pake RIGHT
                    //Debug.Log("rotate yoho: turtarget="+turTarget.ToString());
                }
                else
                {
                    Vector3 targetVectorTurret = myTransform.forward;
                    Quaternion rot = Quaternion.LookRotation(targetVectorTurret);
                    turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rot, Time.deltaTime * turretRotationSpeed);
                    Vector3 eulerAngles = turret.transform.rotation.eulerAngles;
                    eulerAngles = new Vector3(0, eulerAngles.y, -90);
                    turret.transform.rotation = Quaternion.Euler(eulerAngles);
                }
            }
        }
    }

    public override void addWaypoint(Vector3 wpItem)
    {
        if (waypoints == null) return;
        //unit udara!
        if (waypoints.Count > 0)
        {
            wpItem.y = UNIT_UDARA_Y + sampleHeight(wpItem);
        }

        goal = wpItem;

        lineRenderer = (this.lineRenderer != null) ? this.lineRenderer : gameObject.GetComponent<LineRenderer>();
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
        HistoryManager.addToHistory(new HistoryItem(HistoryManager.HISTORY_ADD_WAYPOINT, getCleanName(myTransform, "prefab"), getCleanName(myTransform, "prefab"), wpItem));

    }


    public override void followWaypoint()
    {
        //Debug.Log("execute movement of: " + gameObject.name);
        if (waypoints.Count > 0)
        {
            //cek if posisi awal sama dengan posisi waypoint terakhir
            //if (myTransform.position == waypoints[waypoints.Count - 1])
            if (Vector3.Distance(myTransform.position, waypoints[waypoints.Count - 1]) <= 0.3f)
            {
                //Debug.Log("Udah ada di GOAL!");
                stopEngine();
                return;
            }
            if (curWaypointIdx < waypoints.Count)
            {
                startEngineOnce();
                // unit transport udara

                Vector3 target = waypoints[curWaypointIdx];
                Vector3 moveDir = target - myTransform.position;


                //mulai hitung2an belok mode, kalo jaraknya udah sepersepuluh dari target point, belok mode ON!
                float distToNextPoint = Vector3.Distance(myTransform.position, target);
                //float distToTuj;
                /* curWaypointIdx>0 penting agar dia ga gelinjang2 pas pertama kali gerak */
                if ((distToNextPoint < 10f) && (curWaypointIdx < waypoints.Count - 1) && curWaypointIdx > 0)
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

                // BELOK MODE ON!
                #region belok
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

                }
                #endregion belok
                //BELOK MODE OFF, LURUS2 saja
                else
                {
                    //Debug.Log("Lurus..");

                    //gerakan normal
                    if (!isUnitDarat)
                    {
                        myTransform.LookAt(target);
                        myTransform.position = Vector3.MoveTowards(myTransform.position, target, Time.deltaTime * moveSpeed);
                    }
                    else
                    {
                        //myTransform.LookAt(target);
                        if (PointingAtTarget(target))
                        {
                            myTransform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
                            SetAngles();
                        }
                    }


                    if (distToNextPoint <= 0.1f)
                    {
                        curWaypointIdx++;
                        if (curWaypointIdx < waypoints.Count)
                            myTransform.LookAt(waypoints[curWaypointIdx]);
                    }

                } //end lurus

            }// end curwpidx < count
            else
            {
                //Debug.Log("engine stop harusnya");
                //Debug.Log("Udah ada di GOAL!");
                stopEngine();
            }
        }

    }

    public override void startEngineOnce()
    {
        base.startEngineOnce();
        //if (!audioEngineHasPlayed)
        //{
        //    if (audioEngine != null)
        //    {
        //        audioEngineHasPlayed = true;
        //        audioEngine.Play();
        //    }
        //}
        if (!animationEngineHasPlayed)
        {
            if (animationEngine != null)
            {
                animationEngineHasPlayed = true;
                animationEngine.Play();
            }
        }
    }

    public override void stopEngine()
    {
        base.stopEngine();
        //if (audioEngine != null && audioEngine.isPlaying)
        //{
        //    audioEngine.Stop(); audioEngineHasPlayed = false;
        //    Debug.Log("engine stop!!!");
        //}

        if (animationEngine != null)
        {
            animationEngine.Stop(); animationEngineHasPlayed = false;

        }
    }

    private bool PointingAtTarget(Vector3 target)
    {
        Vector3 forwardVector = transform.forward;
        Vector3 targetVector = target - transform.position;

        forwardVector.y = 0;
        targetVector.y = 0;

        float angle = Vector3.Angle(forwardVector, targetVector);
        Vector3 crossProduct = Vector3.Cross(forwardVector, targetVector);

        if (crossProduct.y < 0) angle *= -1;

        if (Mathf.Abs(angle) < 2.0f)
        {
            return true;
        }
        else
        {
            //We need to rotate
            int direction = 1;
            if (angle < 0)
            {
                direction = -1;
            }

            transform.Rotate(0, RotationalSpeed * Time.deltaTime * direction, 0);
        }

        return false;
    }

    private void SetAngles()
    {
        //Update Height and x-z rotation


        Vector3 checkLeft = transform.position - transform.right;
        Vector3 checkRight = transform.position + transform.right;
        Vector3 checkForward = transform.position + transform.forward;
        Vector3 checkBack = transform.position - transform.forward;

        float heightValCenter, heightValLeft, heightValRight, heightValForward, heightValBack;

        //if ((m_CurrentTile.IsBridge || m_CurrentTile.IsTunnel) && m_CurrentTile.BridgeOrTunnelCollider != null)
        //{
        //    //We're on a bridge or in a tunnel
        //    Ray rayCenter = new Ray(transform.position + (Vector3.up * 10), Vector3.down);
        //    Ray rayLeft = new Ray(checkLeft + (Vector3.up * 10), Vector3.down);
        //    Ray rayRight = new Ray(checkRight + (Vector3.up * 10), Vector3.down);
        //    Ray rayForward = new Ray(checkForward + (Vector3.up * 10), Vector3.down);
        //    Ray rayBack = new Ray(checkBack + (Vector3.up * 10), Vector3.down);

        //    RaycastHit hit;

        //    if (m_CurrentTile.BridgeOrTunnelCollider.Raycast(rayCenter, out hit, Mathf.Infinity))
        //    {
        //        heightValCenter = hit.point.y;
        //    }
        //    else
        //    {
        //        heightValCenter = Terrain.activeTerrain.SampleHeight(transform.position);
        //    }

        //    if (m_CurrentTile.BridgeOrTunnelCollider.Raycast(rayLeft, out hit, Mathf.Infinity))
        //    {
        //        heightValLeft = hit.point.y;
        //    }
        //    else
        //    {
        //        heightValLeft = Terrain.activeTerrain.SampleHeight(checkLeft);
        //    }

        //    if (m_CurrentTile.BridgeOrTunnelCollider.Raycast(rayRight, out hit, Mathf.Infinity))
        //    {
        //        heightValRight = hit.point.y;
        //    }
        //    else
        //    {
        //        heightValRight = Terrain.activeTerrain.SampleHeight(checkRight);
        //    }

        //    if (m_CurrentTile.BridgeOrTunnelCollider.Raycast(rayForward, out hit, Mathf.Infinity))
        //    {
        //        heightValForward = hit.point.y;
        //    }
        //    else
        //    {
        //        heightValForward = Terrain.activeTerrain.SampleHeight(checkForward);
        //    }

        //    if (m_CurrentTile.BridgeOrTunnelCollider.Raycast(rayBack, out hit, Mathf.Infinity))
        //    {
        //        heightValBack = hit.point.y;
        //    }
        //    else
        //    {
        //        heightValBack = Terrain.activeTerrain.SampleHeight(checkBack);
        //    }
        //}
        //else
        {
            //We're on terrain, sample heights
            heightValCenter = Terrain.activeTerrain.SampleHeight(transform.position);
            heightValLeft = Terrain.activeTerrain.SampleHeight(checkLeft);
            heightValRight = Terrain.activeTerrain.SampleHeight(checkRight);
            heightValForward = Terrain.activeTerrain.SampleHeight(checkForward);
            heightValBack = Terrain.activeTerrain.SampleHeight(checkBack);
        }

        //Now we have our height values, we need to set the height and calculate the desired rotation
        //Set height
        m_Position.x = myTransform.position.x;
        m_Position.y = heightValCenter;
        m_Position.z = myTransform.position.z;

        transform.position = m_Position;

        //Rotation along x axis
        float xHeight = heightValForward - heightValBack;
        float xDistance = Vector3.Distance(checkForward, checkBack);
        float xAngle = Mathf.Atan(xHeight / xDistance) * Mathf.Rad2Deg * -1;

        //along z axis
        float zHeight = heightValLeft - heightValRight;
        float zDistance = Vector3.Distance(checkLeft, checkRight);
        float zAngle = Mathf.Atan(zHeight / zDistance) * Mathf.Rad2Deg * -1;

        Vector3 rotation = new Vector3(xAngle, transform.rotation.eulerAngles.y, zAngle);

        //Set rotation
        transform.rotation = Quaternion.Euler(rotation);
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

    public override Vector3 getNearestTarget(List<Vector3> vList)
    {
        Vector3 vret = Const.FARTHEST_VECTOR; //random far away vector
        for (int i = 0; i < vList.Count; i++)
        {
            float distToTarget = Vector3.Distance(myTransform.position, vList[i]);
            if (distToTarget < Vector3.Distance(myTransform.position, vret) && distToTarget <= ATTACK_RANGE && tarPointObjects[i].activeSelf == true)
            {
                vret = vList[i];
            }
        }
        return vret;
    }

    public override IEnumerator attackMove()
    {
        while (true)
        {
            yield return null;
            if (MenuUnit.testMovementMode)
            {
                if (tarpoints.Count > 0)
                {
                    adjustMainGun();
                    while (curTarpointIdx < tarPointObjects.Count)
                    {

                        if (isCargo)
                            StartCoroutine(deployParatrooper(tarPointObjects[curTarpointIdx]));
                        else //fighter maybe?
                            StartCoroutine(fireMissile(tarPointObjects[curTarpointIdx]));
                        curTarpointIdx++;
                    }
                }

            }

        }
    }

    public IEnumerator deployParatrooper(GameObject targetObj)
    {
        Debug.Log("TERJUNKAN! : " + targetObj.transform.position.ToString());
        Vector3 target = targetObj.transform.position;
        distMinToTar = Vector3.Distance(myTransform.position, target);

        while (true)
        {

            yield return null;

            if (IsPointingAtTarget(target))
            {
                //Vector3 target;
                GameObject paraTroop = Instantiate(paraTroopObject, myTransform.position, myTransform.rotation) as GameObject;
                //paraTroop.transform.parent = myTransform; //dijadiin anak biar pas dihapus unitnya, missilenya kehapus juga.
                Transform mt = paraTroop.transform;
                if (audioCannon != null)
                    audioCannon.Play();
                while (true)
                {
                    yield return null;
                    if (mt != null)
                    {
                        //mt.LookAt(target);
                        //Debug.DrawRay(mt.position, mt.forward * 30);
                        mt.position = Vector3.MoveTowards(mt.position, target, Time.deltaTime * moveSpeed);
                        //mt.Translate(target-mt.position, Space.World);

                        Vector3 eulerAngles = mt.rotation.eulerAngles;
                        eulerAngles = new Vector3(-90, eulerAngles.y, eulerAngles.z);// biar parasutnya ga jungkir balik
                        mt.rotation = Quaternion.Euler(eulerAngles);

                        float distToTar = Vector3.Distance(mt.position, target);


                        //mt.Translate(mt.forward * moveSpeed * Time.deltaTime);
                        if (distToTar <= 0.1f)
                        {
                            //isMoving = false;
                            //break; // kalo break doang, nanti dia nembak berkali2
                            targetObj.SetActive(false);//diaktivasi target object kalo udah kena, jangan dihapus ntar exception!
                            
                            //paraTroop diganti jadi infanteri
                            GameObject infanteri = Instantiate(infanteriObject, mt.position, myTransform.rotation) as GameObject;
                            //dimasukkin ke unit container
                            GameObject unitConObject = GameObject.Find("UnitContainer");
                            if (unitConObject != null)
                            {
                                infanteri.transform.parent = unitConObject.transform; 
                            }
                            //parasutnya dihapus
                            Destroy(paraTroop);

                            //break;
                            yield return null; // kalo diyield, dia nembak sekali aja begitu kena, beres.
                        }
                    }
                    //Debug.Log("firing to " + target.ToString());

                }//endwhile
            }//endif

        }//endwhile

    }

    //static List<HomingMissile> missiles = new List<HomingMissile>();
    //private Puffy_Emitter emitter;
    //public int launchCount = 1;

    public override IEnumerator fireMissile(GameObject targetObj)
    {
        Debug.Log("TEMBAK! : " + targetObj.transform.position.ToString());
        Vector3 target = targetObj.transform.position;
        distMinToTar = Vector3.Distance(myTransform.position, target);

        while (true)
        {

            yield return null;

            float distUnitToTar = Vector3.Distance(myTransform.position, target);

            //Debug.LogError("DistUnitToTar: " + distUnitToTar);
            Debug.DrawRay(myTransform.position, (target - myTransform.position).normalized * ATTACK_RANGE, Color.yellow);

            //if (distUnitToTar <= ATTACK_RANGE)// && distUnitToTar<=distMinToTar)
            if (IsPointingAtTarget(target))
            {
                //begin. integrasi PuffySmoke
                

                //end. integrasi PuffySmoke
                
                //Vector3 target;
                GameObject missile = Instantiate(missileObject, myTransform.position, myTransform.rotation) as GameObject;
                missile.tag = "puffymesh";
                //puffysmoke tak menghendaki;// missile.transform.parent = myTransform; //dijadiin anak biar pas dihapus unitnya, missilenya kehapus juga.
                Transform mt = missile.transform;


                //PuffySmokeRelated
                Puffy_Emitter emitter = mt.GetComponent<Puffy_Emitter>();
                //if (emitter == null) emitter = mt.gameObject.AddComponent<Puffy_Emitter>();
                if (emitter)
                {
                    if (emitter.PuffyRenderer == null)
                    {
                        Puffy_Renderer smoke_renderer = mt.GetComponent<Puffy_Renderer>();//Puffy_Renderer.GetRenderer();
                        if (smoke_renderer) smoke_renderer.AddEmitter(emitter);
                    }
                }

                if (audioCannon != null)
                    audioCannon.Play();
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
                            targetObj.SetActive(false);//diaktivasi target object kalo udah kena, jangan dihapus ntar exception!

                            //Destroy(missile); // missilenya juga lah..
                            //missile.SetActive(false);
                            //break;
                            missile.particleSystem.loop = false;

                            blowTheTarget(mt,target);

                            yield return new WaitForSeconds(3);
                            Destroy(missile); // missilenya juga lah..
                            yield return null; // kalo diyield, dia nembak sekali aja begitu kena, beres.
                        }
                    }
                    //Debug.Log("firing to " + target.ToString());

                }//endwhile
            }//endif

        }//endwhile

    }

    public override bool IsPointingAtTarget(Vector3 target)
    {
        if (!isCargo) //unit nonkargo (fighter) begitu deteksi target langsung tembak.
        {
            float distUnitToTar = Vector3.Distance(myTransform.position, target);
            return distUnitToTar <= ATTACK_RANGE;
        }
        else //khusus kargo ada perhitungan khusus, tujuannya agar terjunnya pas target udah di belakang unit.
        {
            //Debug.LogError("DistUnitToTar: " + distUnitToTar);
            Vector3 targetDir = (target - myTransform.position);

            //Debug.DrawRay(myTransform.position, targetDir.normalized * ATTACK_RANGE, Color.yellow);
            //Debug.DrawRay(myTransform.position, myTransform.right * ATTACK_RANGE, Color.red);
            //Debug.DrawRay(myTransform.position, -myTransform.right * ATTACK_RANGE, Color.green);
            //Debug.DrawRay(myTransform.position, -myTransform.up * ATTACK_RANGE, Color.cyan);

            //Debug.DrawRay(myTransform.position, (-myTransform.up * ATTACK_RANGE) + (-myTransform.right * ATTACK_RANGE), Color.cyan);

            // cari sudut antara vektor ke target dengan plane kiri dan kanan (diwakili resultan vektor arah bawah dan kiri/kanan)
            float angleLeft = Vector3.Angle(targetDir.normalized, (-myTransform.up) + (-myTransform.right));
            float angleRight = Vector3.Angle(targetDir.normalized, (-myTransform.up) + (myTransform.right));

            targetInLeft = (angleLeft <= angleRight); // cek posisi target apakah ada di sisi kiri unit?

            // cek arah positif atau negatif sudutnya, pakai operasi cross. (sumber: http://answers.unity3d.com/questions/181867/is-there-way-to-find-a-negative-angle.html)
            Vector3 crossDownLeft = Vector3.Cross(targetDir.normalized, (-myTransform.up) + (-myTransform.right));
            Vector3 crossDownRight = Vector3.Cross(targetDir.normalized, (-myTransform.up) + (myTransform.right));
            if (crossDownLeft.y < 0) angleLeft = -angleLeft;
            if (crossDownRight.y < 0) angleRight = -angleRight;

            //Debug.Log("arahTarget? "+(targetInLeft?"kiri":"kanan")+" angleLeft=" + angleLeft + " angleRight=" + angleRight);

            float distUnitToTar = Vector3.Distance(myTransform.position, target);
            float halfFireingLeftArc = 35.0f;
            float halfFireingRightArc = -35.0f;

            //jika target ada di arah yg sesuai dan sudutnya udah berada di belakang unit (biar terjunnya ke belakang), dan ada di dalam jangkauan, maka true.
            return ((targetInLeft && (angleLeft > halfFireingLeftArc))
                    || (!targetInLeft && (angleRight < halfFireingRightArc))
                    && distUnitToTar <= ATTACK_RANGE);
        }
    }

    //INIT KHUSUS ANIMASI
    private void initEngineAnimation()
    {
        Animation[] anms = GetComponents<Animation>();
        if (anms.Length > 0)
        {
            Debug.Log("animation length?? " + anms.Length);
            animationEngine = anms[0];
        }

    }


    /* end of class */
}
