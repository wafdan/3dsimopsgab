using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//[DoNotSerialize]
public abstract class UnitMovement : MonoBehaviour
{

    public abstract Vector3 getNearestTarget(List<Vector3> vList);
    public abstract IEnumerator attackMove();
    public abstract void adjustMainGun();
    public abstract IEnumerator fireMissile(GameObject targetObj);
    public abstract bool IsPointingAtTarget(Vector3 target);
    public abstract void followWaypoint();
    public abstract void stopEngine();
    public abstract void startEngineOnce();

}
