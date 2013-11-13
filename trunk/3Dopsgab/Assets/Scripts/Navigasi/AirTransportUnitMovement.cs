using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//[DoNotSerialize]
public class AirTransportUnitMovement : BasicUnitMovement
{

    Animation animationEngine;

    void Start()
    {
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

    public override void followWaypoint()
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
                startEngineOnce();
                // unit transport udara

                Vector3 target = waypoints[curWaypointIdx];
                Vector3 moveDir = target - myTransform.position;


                //mulai hitung2an belok mode, kalo jaraknya udah sepersepuluh dari target point, belok mode ON!
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

                // BELOK MODE ON!
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

                } //BELOK MODE OFF, LURUS2 saja
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
        if (!audioEngineHasPlayed)
        {
            if (audioEngine != null)
            {
                audioEngineHasPlayed = true;
                audioEngine.Play();
            }
        }
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
        if (audioEngine != null)
        {
            audioEngine.Stop(); audioEngineHasPlayed = false;
            Debug.Log("engine stop!!!");
        }

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
                    //foreach (Vector3 target in tarpoints)
                    //{
                    /* serangan serial */
                    //yield return StartCoroutine(fireMissile(target)); 
                    /* serangan paralel */
                    adjustMainGun();
                    while (curTarpointIdx < tarPointObjects.Count)
                    {
                        //RadicalRoutineExtensions.StartExtendedCoroutine(gameObject,(fireMissile(tarPointObjects[curTarpointIdx]))); 
                        //Debug.Log("Armature child? [" + turret.name + "]: " + turret.transform.GetChild(0).GetChild(0).name);
                        StartCoroutine(fireMissile(tarPointObjects[curTarpointIdx]));
                        curTarpointIdx++;
                    }
                }

            }

        }
    }

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
                //Vector3 target;
                GameObject missile = Instantiate(missileObject, myTransform.position, myTransform.rotation) as GameObject;
                missile.transform.parent = myTransform; //dijadiin anak biar pas dihapus unitnya, missilenya kehapus juga.
                Transform mt = missile.transform;
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

    public override bool IsPointingAtTarget(Vector3 target)
    {

        if (!isUnitDarat)
        {
            float distUnitToTar = Vector3.Distance(myTransform.position, target);
            return distUnitToTar <= ATTACK_RANGE;
        }
        else
        {
            Vector3 barrelForwardDir = -barrel.transform.right;//harusnya forward, tapi right yg dipake karena mungkin kesalahan di modeling.
            target.y = barrel.transform.position.y;
            Vector3 barrelPos = barrel.transform.position;
            Vector3 barrelDir = target - barrelPos;
            float distUnitToTar = barrelDir.magnitude;

            //Debug.DrawRay(barrelPos, barrel.transform.forward*50, Color.green);
            Debug.DrawRay(barrelPos, barrelForwardDir * 50, Color.cyan);
            //Debug.DrawRay(barrelPos, barrel.transform.up * 50, Color.blue);
            //Debug.DrawRay(barrelPos, myTransform.forward * 50, Color.blue);
            //Debug.DrawRay(barrelPos, barrelDir, Color.red);

            float angle = Vector3.Angle(barrelForwardDir, barrelDir);
            float halfFireingArc = 5.0f;
            //Debug.Log("firingArc = " + angle + " attackArc=" + halfFireingArc + " dist=" + distUnitToTar+" range="+ATTACK_RANGE);
            return (angle < halfFireingArc && distUnitToTar <= ATTACK_RANGE);
            //else false;
            //return distUnitToTar <= ATTACK_RANGE;
        }
    }

    //INIT KHUSUS ANIMASI
    private void initEngineAnimation()
    {
        Animation[] anms = GetComponents<Animation>();
        if (anms.Length > 0)
        {
            animationEngine = anms[0];
        }

    }


    /* end of class */
}
