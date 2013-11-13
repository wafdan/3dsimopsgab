using UnityEngine;
using System.Collections;

public class PesawatLanding : MonoBehaviour {

    private Transform myTransform;
    public float kecepatanAwal;
   
    private RaycastHit hit;
    private float range = 30f;

    private bool berhenti = false;

	// Use this for initialization
	void Start () {
        myTransform = transform;

	}
	
	// Update is called once per frame
	void Update () {

        if (Physics.Raycast(myTransform.position, myTransform.forward, out hit, range))
        {
            Debug.Log("Bener2 siap landing");
            kecepatanAwal = 70;
            if (hit.transform.tag == "target")
            {
                Debug.Log("ok");
                if (myTransform.rotation.x != 0)
                {
                    myTransform.rotation = Quaternion.Euler(0, 69.96387f, 0);
                }
                StartCoroutine(PesawatBerhenti());
            }
            
        }

        if (berhenti == false)
        {
            myTransform.Translate(Vector3.forward * kecepatanAwal * Time.deltaTime);
        }
        else
        {
            kecepatanAwal = 0;
            //audio.volume -= (int)0.1 * Time.deltaTime;
        }

	}

    IEnumerator PesawatBerhenti()
    {
        yield return new WaitForSeconds(5);
        berhenti = true;
    }
}
