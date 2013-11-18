using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//[DoNotSerialize]
public class SubmarineUnitMovement : NavalUnitMovement
{
    private GameObject targetSelamEffectObject, targetApungEffectObject;

    public List<Vector3> selampoints;
    public List<Vector3> apungpoints;

    void Start()
    {
        base.Start();
        targetSelamEffectObject = Resources.Load("TargetSelamEffect") as GameObject;
        targetApungEffectObject = Resources.Load("TargetApungEffect") as GameObject;
    }
    
    public void addSubmergepoint(Vector3 tp)
    {
        //if (selampoints == null) return;
        //GameObject go = Instantiate(targetSelamEffectObject, tp, targetSelamEffectObject.transform.rotation) as GameObject;
        //go.name = "TargetObj" + go.GetInstanceID();

        //tarPointObjects.Add(go);
        //tarpoints.Add(tp);

        //add to history
        //HistoryManager.addToHistory(new HistoryItem(HistoryManager.HISTORY_ADD_TARPOINT, getCleanName(myTransform, "name"), getCleanName(myTransform, "prefab"), tp));
    }

}
