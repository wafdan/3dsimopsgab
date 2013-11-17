using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//[DoNotSerialize]
public class NavalUnitMovement : BasicUnitMovement
{
    
    private int firingAngle = 60;
    private float gravity = 9.8f;

    void Start()
    {
        base.Start();
        missileObject = Resources.Load("NavalMissileEffect") as GameObject;
    }

    //yg beda dengan BasicUnitMovement cuma gerakan missilenya aja, parabola.
    // sumber: http://answers.unity3d.com/questions/58096/parabola-angle-doesnt-work.html
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
                //missile.transform.parent = myTransform; //dijadiin anak biar pas dihapus unitnya, missilenya kehapus juga.
                Transform mt = missile.transform;
                if (audioCannon != null)
                    audioCannon.Play();

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

                // Calculate distance to target
                float target_Distance = Vector3.Distance(mt.position, target);

                // Calculate the velocity needed to throw the object to the target at specified angle.
                float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

                // Extract the X & Y componenent of the velocity
                float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
                float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

                // Calculate flight time.
                float flightDuration = target_Distance / Vx;

                // Rotate projectile to face the target.
                mt.rotation = Quaternion.LookRotation(target - mt.position);

                float elapse_time = 0;

                //while (elapse_time < flightDuration)
                while (true)
                {

                    mt.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

                    elapse_time += Time.deltaTime;

                    yield return null;
                    if (mt == null) break;
                    float distToTar = Vector3.Distance(mt.position, target);
                    //Debug.Log("distToTar:" + distToTar);
                    if (distToTar <= 2.5f)
                    {
                        //isMoving = false;
                        //break; // kalo break doang, nanti dia nembak berkali2
                        targetObj.SetActive(false);//diaktivasi target object kalo udah kena, jangan dihapus ntar exception!
                        mt.particleSystem.loop = false;
                        blowTheTarget(mt,target);

                        yield return new WaitForSeconds(3);
                        Destroy(missile); // missilenya juga lah..
                        //break;
                        yield break; // kalo diyield, dia nembak sekali aja begitu kena, beres.
                    }

                }
                ////////

            }//endif

        }//endwhile

    }
}
