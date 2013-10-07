using UnityEngine;
using System.Collections;

/// <summary>
/// Box movement, script ini diattach kepada box yang menjadi
/// target dari si pesawat. pergerakan box diatur sedemikian rupa
/// sehingga pesawat terlihat seperti patroli.
/// </summary>

public class boxMovement : MonoBehaviour {
	
	Transform myTransform;
	
	// banyak melingkar
	public static bool banyakPatroli = false;
		
	public static bool boxDetected = false;

    public static float kecepatanBox = 60;
    public static float rotation = 70;

	private float x;
	private float z;
	// Use this for initialization
	void Start () {
		myTransform = transform;
        StartCoroutine(waktuPatrolPesawat());
	}
	
	// Update is called once per frame
	void Update () {

        if (boxDetected == true)
        {
            gerakMelingkar();
        }
	}
	
	void gerakMelingkar()
	{
        myTransform.Translate(0, 0, kecepatanBox * Time.deltaTime);

        myTransform.Rotate(0, rotation * Time.deltaTime, 0);
	}

    IEnumerator waktuPatrolPesawat()
    {
        yield return new WaitForSeconds(17.9f);
        banyakPatroli = true;
    }
}
