using UnityEngine;
using System.Collections;

public class BuildingPlacement : MonoBehaviour {
	
	public float scrollSensitivity;
	
	private PlaceableBuilding placeableBuilding;
	private Transform currentBuilding;

    private Transform daratan;

    private Color normalColor = Color.gray;//new Color(0.846f, 0.808f, 0.808f, 1.000f);

	public static bool hasPlaced;
	
	public LayerMask buildingsMask;
	
	private PlaceableBuilding placeableBuildingOld;
    private float unitAltitude;
    private BasicUnitMovement curBum;

    void Start()
    {
        GameObject darat = GameObject.FindGameObjectWithTag("daratan");
        if (darat == null) darat = GameObject.Find("Peta_Indonesia");
        if (darat != null) daratan = darat.transform;

    }

	// Update is called once per frame
	void Update () {
        
		Vector3 m = Input.mousePosition;
        m = new Vector3(m.x,m.y,transform.position.y);
		Vector3 p = camera.ScreenToWorldPoint(m);
        
        if (currentBuilding != null)
        {
            BasicUnitMovement bm = currentBuilding.GetComponent<BasicUnitMovement>();
            if (bm.isUnitUdara)
            {
                unitAltitude = BasicUnitMovement.UNIT_UDARA_Y + sampleHeight(currentBuilding.position);
                //Debug.DrawRay(currentBuilding.position, Vector3.down);
            }
            if (bm.isUnitLaut)
            {
                unitAltitude = BasicUnitMovement.UNIT_LAUT_Y;// +sampleHeight(currentBuilding.position);
                //Debug.DrawRay(currentBuilding.position, Vector3.down * Terrain.activeTerrain.SampleHeight(currentBuilding.position));
            }
            if (bm.isUnitDarat)
            {
                unitAltitude = sampleHeight(currentBuilding.position); //BasicUnitMovement.UNIT_UDARA_Y;
                
            }
        }
        else
        {
            return;
        }

		if (!hasPlaced) {
			currentBuilding.position = new Vector3(p.x,unitAltitude,p.z);

            checkLegalPos();

			if (Input.GetMouseButtonDown(0)) {
				if (IsLegalPosition()) {
					hasPlaced = true;

                    //handling unit laut
                    if (currentBuilding.gameObject.GetComponent<BasicUnitMovement>().isUnitLaut)
                    {
                        currentBuilding.position = new Vector3(p.x, BasicUnitMovement.UNIT_LAUT_Y, p.z);
                    }
                    //handling unit darat
                    if (currentBuilding.gameObject.GetComponent<BasicUnitMovement>().isUnitDarat)
                    {
                        //currentBuilding.gameObject.collider.isTrigger = false; GA JADI RIGID DULU
                        Debug.Log("IS TRIGGER??? " + currentBuilding.gameObject.collider.isTrigger);
                    }
                    //add to history
                    HistoryManager.addToHistory(new HistoryItem(HistoryManager.HISTORY_ADD_UNIT,getCleanName(currentBuilding,"name"),getCleanName(currentBuilding,"prefab"),currentBuilding.position));
                    //change name of the new added unit
                    currentBuilding.collider.gameObject.name = getCleanName(currentBuilding, "name");
				}
			}
            //klik kanan untuk cancel
            if (Input.GetMouseButtonDown(1)) 
            {
                Destroy(currentBuilding.gameObject);
            }
		}
		else {
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit hit = new RaycastHit();
				Ray ray = new Ray(new Vector3(p.x,unitAltitude,p.z), Vector3.down);
				if (Physics.Raycast(ray, out hit,Mathf.Infinity,buildingsMask)) {
					if (placeableBuildingOld != null) {
						placeableBuildingOld.SetSelected(false);
					}
                    PlaceableBuilding pb = hit.collider.gameObject.GetComponent<PlaceableBuilding>();
                    if(pb!=null) pb.SetSelected(true);
					placeableBuildingOld = pb;
				}
				else {
					if (placeableBuildingOld != null) {
						placeableBuildingOld.SetSelected(false);
					}
				}
			}
		}
	}
    float heightAboveGround = 0;
    private float sampleHeight(Vector3 vector3)
    {

        if (Terrain.activeTerrain != null)
        {
            return Terrain.activeTerrain.SampleHeight(vector3);
        }
        else
        {
            // harusnya ini return ketinggian berdasarkan mesh peta indonesia
            //return 0;
            RaycastHit hit;
            //float heightAboveGround = currentBuilding.position.y;// = 0;
            if (Physics.Raycast(currentBuilding.position,Vector3.down,out hit,20)) //currentBuilding.TransformDirection(Vector3.down),out hit))
            {
                heightAboveGround = 19.995f - hit.distance;
            }
            //Debug.DrawRay(currentBuilding.position, currentBuilding.TransformDirection(Vector3.down)*Mathf.Infinity);
            Debug.Log("height: " + heightAboveGround);
            return heightAboveGround;
        }
    }

    private void checkLegalPos()
    {
        if (!IsLegalPosition())
        {
            currentBuilding.gameObject.renderer.material.color = Color.red;
            if (currentBuilding.childCount > 0)
            {
                colorTheChilds(currentBuilding,Color.red);
            }
        }
        else
        {
            currentBuilding.gameObject.renderer.material.color = normalColor;
            if (currentBuilding.childCount > 0)
            {
                Renderer[] co = currentBuilding.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < co.Length; i++)
                {
                    co[i].material.color = normalColor;
                }
            }
        }
    }

    private Color getOriginalColor()
    {
        Color color = normalColor;
        BasicUnitMovement bm = GetComponent<BasicUnitMovement>();
        if (bm != null)
        {
            if (bm.isUnitLaut)
            {
                color = new Color(0.419f, 0.419f, 0.419f, 1.000f);
            }
        }
        return color;
    }

    private void colorTheChilds(Transform curTransform, Color color)
    {
        if (curTransform.childCount > 0)
        {
            Renderer[] co = curTransform.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < co.Length; i++)
            {
                co[i].material.color = color;
                //colorTheChilds(co[i].transform, color);
            }
        }
    }
    

	bool IsLegalPosition() {
		if (placeableBuilding.colliders.Count > 0) {
            //Debug.Log("Collider Count: " + placeableBuilding.colliders.Count);
            //Debug.Log("Collider 0 name: " + placeableBuilding.colliders[0].name);
			return false;	
		}
		return true;
	}

    public string getCleanName(Transform myTransform, string which)
    {
        return HistoryManager.getCleanName(myTransform, which);
    }

	public void SetItem(GameObject b) {
		hasPlaced = false;
		currentBuilding = ((GameObject)Instantiate(b)).transform;
		placeableBuilding = currentBuilding.GetComponent<PlaceableBuilding>();
        
        //GameObject unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
        //if (unitManagerObject != null)
        //{
        //    currentBuilding.parent = unitManagerObject.transform;
        //}
        GameObject unitConObject = GameObject.Find("UnitContainer");
        if (unitConObject == null)
        {
            unitConObject = new GameObject("UnitContainer");
        }
        currentBuilding.parent = unitConObject.transform;
	}

}
