using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Puffy_Renderer : MonoBehaviour {
	
	public static List<Puffy_Renderer> instances = new List<Puffy_Renderer>();
	
	public bool debug = false;
	public bool render = true;
	public float updateThreshold = 0.01f;
	public Light _light;
	public bool useAmbientColor = true;
	public bool cameraBackgroundAsAmbientColor = true;
	
	public Material particlesMaterial;
	public int TextureColCount = 16;
	public int TextureRowCount = 4;
	public float detailsScaling = 0.55f;
		
	public List<Puffy_Emitter> Emitters;
	
	public bool useThread = true;
	
	private int live = 0;
	private int _coresCount;
	private int _virtualCoresCount;
	private int activeGroups = 0;
	private int TextureFrameCount = 1;
	private int meshUpdateCount = 0;
	private int visibleMeshesCount = 0;
	private int renderStep = 0;
	private int visibleParticles = 0;
	
	private float pDirectionalTextureFrameWidth = 1f;
	
	private double updateMeshTime = 0;
	private double buildTime = 0;
	private double renderTime = 0;
	private double mergeTime = 0;
	
	private DebugTimer frustumTime = new DebugTimer();
	private DebugTimer sortTime = new DebugTimer();
	
	private List<SortGroup> _sortGroups = new List<SortGroup>();
	private List<SortGroup> _sortIndexStock = new List<SortGroup>();
	private List<UnityThreading.Task> taskList = new List<UnityThreading.Task>();
	private List<VariableMesh> meshList = new List<VariableMesh>();
	
	private OrderComparer comp = new OrderComparer();
	
	private Vector2 _UVrotationVector = Vector2.zero;
	
	private Vector3[] _billboardShape = new Vector3[4];
	
	private Camera _camera;
	private Transform _cameraTransform;
	private Transform _lightTransform;
	
	
	public static Puffy_Renderer GetRenderer(){
		if(instances.Count == 0) return null;
		return instances[0];
	}
	
	public static Puffy_Renderer GetRenderer(string rendererName){
		for(int i=0; i<instances.Count; i++){
			if(instances[i].name == rendererName) return instances[i];
		}
		return null;
	}
	
	public static Puffy_Renderer GetRenderer(int index){
		if(index < 0 || index >= instances.Count) return null;
		return instances[index];
	}
	
	void Awake () {
		instances.Add (this);
		
		// number of real cores available in the cpu
		_coresCount = SystemInfo.processorCount;
		
		// virtual cores are used to split the sorting process when too many particles are created
		_virtualCoresCount = _coresCount;
		
		// create initial sort groups
		int i;
		for(i=0; i < _coresCount;i++){
			_sortGroups.Add (new SortGroup());
			_sortIndexStock.Add (new SortGroup());
		}
		
		// identify the main camera
		_camera = Camera.main;
		_cameraTransform = _camera.transform;
		_lightTransform = _light.transform;
		
		TextureFrameCount = (TextureColCount*TextureRowCount)-1;
		
		pDirectionalTextureFrameWidth = 1f/TextureColCount;
		
	}
	
	
	void OnGUI() {
		if(debug){
			float updateTime = 0f;
			live = 0;
			float total = 0;
			
			foreach(Puffy_Emitter e in Emitters){
				total += e.particleTotal;
				live += e.particleLive;
				updateTime += (float)(e.debugTime*1000);
			}
			
			GUILayout.Space(15);
			
			GUILayout.Label("Live : "+live.ToString()+"/"+total.ToString());
			
			if(live > 0){
				GUILayout.Label ("Visible : " + visibleParticles.ToString ());
					
				GUI.color = Color.white;
				if(useThread){
					GUILayout.Label("Cores : "+_virtualCoresCount);
				}else{
					GUILayout.Label("Threading OFF");
				}
				
				GUILayout.Label("Move : "+updateTime.ToString("f2")+"ms");
						
				GUILayout.Label("Frustum check : " + frustumTime.Average().ToString ("f2") + "ms");
				//GUILayout.Label(frustumTime.minimum.ToString("f2")+" - "+frustumTime.maximum.ToString("f2"));
				
				GUILayout.Label("Z Sort : " + sortTime.Average().ToString ("f2") + "ms");
				//GUILayout.Label(sortTime.minimum.ToString("f2")+" - "+sortTime.maximum.ToString("f2"));

				GUILayout.Label ("Convert : " + (convertTime*1000).ToString ("f2") + "ms");
				
				GUILayout.Label ("Merge sort : " + (mergeTime*1000).ToString ("f2") + "ms");
				
				GUILayout.Label ("Rebuild mesh : " + (buildTime*1000).ToString ("f2") + "ms");
				
				GUILayout.Label ("Update mesh : " + (updateMeshTime*1000).ToString ("f2") + "ms");
				
				/*
				string str = "sort groups " +activeGroups+"/"+_sortGroups.Count+" :\n";
				for(int i=0;i<_sortGroups.Count;i++){
					str += "group "+i+" : "+_sortGroups[i].count+" "+(_sortGroups[i].sortTime>0?(_sortGroups[i].sortTime*1000).ToString("f2")+"ms":"")+"\n";
				}
				
				GUILayout.Label (str);
				*/
			}
		}else{
			GUILayout.Space(15);
			GUILayout.Label ("visible :" + visibleParticles.ToString ());
		}
	}
	
	
	void OnDrawGizmos(){
		if(debug){
			int i=0;
			
			Color[] cols = new Color[16];
			
			cols[0] = new Color(1f,0f,0f,1f);
			cols[1] = new Color(1f,0.2f,0f,1f);
			cols[2] = new Color(1f,0.4f,0f,1f);
			cols[3] = new Color(1f,0.6f,0f,1f);
			cols[4] = new Color(1f,0.8f,0f,1f);
			cols[5] = new Color(1f,1f,0f,1f);
			cols[6] = new Color(0.8f,1f,0f,1f);
			cols[7] = new Color(0.6f,1f,0f,1f);
			cols[8] = new Color(0.4f,1f,0f,1f);
			cols[9] = new Color(0.2f,1f,0f,1f);
			cols[10] = new Color(0.0f,1f,0f,1f);
			cols[11] = new Color(0.0f,1f,0.2f,1f);
			cols[12] = new Color(0.0f,1f,0.4f,1f);
			cols[13] = new Color(0.0f,1f,0.6f,1f);
			cols[14] = new Color(0.0f,1f,0.8f,1f);
			cols[15] = new Color(0.0f,1f,1f,1f);
			
			for(i=0; i<meshList.Count;i++){
				if(meshList[i].particleIndex > 0){
					if(i<=15) Gizmos.color = cols[i];
					//Gizmos.DrawWireCube(meshList[i].mesh.bounds.center + meshList[i].center,meshList[i].mesh.bounds.size);
					Gizmos.DrawWireCube(meshList[i].mesh.bounds.center + meshList[i]._transform.position,meshList[i].mesh.bounds.size);
					
				}
			}
		}
	}
	
	
	float elpasedTime = 0f;
	void Update(){
		elpasedTime += Time.deltaTime;
		if(elpasedTime >= Time.fixedDeltaTime || renderStep > 0){
			Render();
			elpasedTime = 0;
		}
	}
	
	public bool AddEmitter(Puffy_Emitter emitter){
		if(emitter.PuffyRenderer != this){
			Emitters.Add (emitter);
			return true;
		}
		
		return false;
	}
	
	public bool RemoveEmitter(Puffy_Emitter emitter){
		return Emitters.Remove(emitter);
	}
	
	public bool RemoveEmitter(int index){
		if(index>-1 && index < Emitters.Count){
			Emitters.RemoveAt(index);
			return true;
		}
		
		return false;
	}
	
	public bool RemoveEmitter(string emitterName){
		for(int i=0; i<Emitters.Count; i++){
			if(Emitters[i].name == emitterName){
				Emitters.RemoveAt(i);
				return true;
			}
		}
		
		return false;
	}
	
	void Render(){

		if(renderStep == 0){
			// total number of alive particles
			live = 0;
			
			// number of visible particles
			visibleParticles = 0;
			
			// get the total of alive particles
			for(int i=0; i < Emitters.Count; i++){
				if(Emitters[i] == null){
					Emitters.RemoveAt(i);
					i--;
				}else{
					live += Emitters[i].particleLive;
					if(Emitters[i].PuffyRenderer == null) Emitters[i].PuffyRenderer = this;
				}
			}
		}
		if(live>0 || renderStep > 0 || visibleMeshesCount > 0){
				
			if(live == 0){
				meshUpdateCount = 0;
				UpdateMeshes();
				renderStep = 0;
			}else{
			
				double time = 0;
				double totalTime = 0;
				
				if(renderStep == 0){
					time = Time.realtimeSinceStartup;
					// define the right billboard shape
					updateDirectionalProjection();
					
					// identify visible particles
					frustumTime.Start();
					FrustumCheck();
					frustumTime.Stop();
										
					// sort visible particles
					sortTime.Start ();
					SortParticles();
					sortTime.Stop();
					
					totalTime += Time.realtimeSinceStartup - time;
				
					if(totalTime < updateThreshold ) renderStep = 1;
					
				}
				
				if(renderStep == 1){
					
					time = Time.realtimeSinceStartup;
					MergeGroups();
					mergeTime = Time.realtimeSinceStartup - time;
					totalTime += mergeTime;
										
					time = Time.realtimeSinceStartup;
					BuildMeshes();
					renderTime = Time.realtimeSinceStartup - time;
					totalTime += renderTime;
				
					if(totalTime < updateThreshold ) renderStep = 2;
					
					// force update meshes, else display is flickering (for now)
					renderStep = 2;
				}
					
				if(renderStep == 2){
					UpdateMeshes();
					
					renderStep = 0;
				}else{
					renderStep++;
				}
			}
				
		}
		
	}
	
	private void updateDirectionalProjection(){
		
		Vector3 lDir;
		if(_light == null){
			lDir = _cameraTransform.InverseTransformDirection(Vector3.down);
		}else{
			lDir = _cameraTransform.InverseTransformDirection(_lightTransform.forward);
		}
		
		// particle Roll
		if (Mathf.Approximately(1f,lDir.z)){
			lDir.x = 0f;
			lDir.y = 1f;
		}else{
			lDir.z = 0f;
			lDir.Normalize();
		}
		
		// billboard shape
		float lAng = Mathf.Atan2(lDir.y,lDir.x) + 0.7853982f; // + 3.141592f * 0.25f;
		lDir.x = Mathf.Cos(lAng) * 0.5f;
		lDir.y = Mathf.Sin(lAng) * 0.5f;
	
		_billboardShape[0] = _cameraTransform.TransformDirection(new Vector3(lDir.y,-lDir.x,0f));
		_billboardShape[1] = _cameraTransform.TransformDirection(new Vector3(-lDir.x,-lDir.y,0f));
		_billboardShape[2] = _cameraTransform.TransformDirection(new Vector3(-lDir.y,lDir.x,0f));
		_billboardShape[3] = _cameraTransform.TransformDirection(new Vector3(lDir.x,lDir.y,0f));
		
		// details roll
		_UVrotationVector.x = lDir.x;
		_UVrotationVector.y = lDir.y;
	}
	
	// ***********************************************************************************************************************
	// STEP 1) Fill the sort groups with the visible particles indices
	// ***********************************************************************************************************************
	
	private void FrustumCheck(){
	
		Vector3 camDir = _cameraTransform.forward;
		Vector3 camPos = _cameraTransform.position;
		float angle = Mathf.Cos(_camera.fieldOfView * Mathf.Deg2Rad * 0.9f);
		
		int i;
		int j;

		int count;
		
		// init the sort groups
		count = _sortGroups.Count;
		for(i=0;i < count; i++){
			_sortGroups[i].index = 0;
			_sortGroups[i].count = 0;
		}
		
		// update the number of virtual cores according to the number of particles
		_virtualCoresCount = Mathf.Max (_coresCount,live / 2048);
		Matrix4x4 lightMatrix = _light.transform.worldToLocalMatrix;
		Matrix4x4 cameraMatrix = _cameraTransform.worldToLocalMatrix;
		
		// too few particles or only one core is available, do the computing on a single thread
		if(_coresCount==1 || live < _coresCount * 25 || !useThread){

			FrustumTask(0,1,camDir,camPos,angle,lightMatrix,cameraMatrix);
			
		}else{
			if(taskList == null) taskList = new List<UnityThreading.Task>();
			
			//List<UnityThreading.Task> taskList = new List<UnityThreading.Task>();
			taskList.Clear();
			int threadCount = 0;
			
			// split the computing accross all virtual cores
			for(i = 0 ; i < _virtualCoresCount ; i++){
				var grp = i;
				
				if(i >= _sortGroups.Count) _sortGroups.Add (new SortGroup());
				
				_sortGroups[i].index = 0;
				_sortGroups[i].count = 0;
				taskList.Add(UnityThreadHelper.TaskDistributor.Dispatch(() => FrustumTask(grp,_virtualCoresCount,camDir,camPos,angle,lightMatrix,cameraMatrix)));
				
				threadCount ++;
				// when all real cores are busy, wait for the end of each process before creating new ones
				if(threadCount == _coresCount){
					for(j = 0; j < threadCount ; j ++){
						taskList[j].Wait();
						taskList[j].Dispose();
					}
					// get ready for the next row of threads
					threadCount = 0;
					taskList.Clear();
				}
			}
			// wait for the last threads
			count = taskList.Count;
			for(i = 0; i < count ; i ++){
				taskList[i].Wait();
				taskList[i].Dispose();
			}
			taskList.Clear();		
			
		}
		
		// total of visible particles
		visibleParticles = 0;
		count = _sortGroups.Count;
		activeGroups = 0;
		for(i = 0 ; i < count; i++){
			_sortGroups[i].index = 0;
			if(_sortGroups[i].count > 0) activeGroups++;
			visibleParticles += _sortGroups[i].count;
		}
	}
		
	private void FrustumTask(int groupIndex, int split, Vector3 camDir,Vector3 camPos,float angle, Matrix4x4 lightMatrix, Matrix4x4 cameraMatrix){
		

			int index;
			int pointer;
			int total = _sortGroups[groupIndex].items.Count;
			int count = _sortGroups[groupIndex].index;
			Vector3 direction;
			
			SortIndex s;
			Puffy_Emitter emitter;
		
			int start = 0;
			int end = 0;
			int emitterIndex;
			int stp;
		
			split = Mathf.Min (Mathf.Max(1,split),_virtualCoresCount);
			
			int row = 1;
			float rowHeight = 1f / TextureRowCount;
			
			int tile_index;
			float tile_U = 0f;
			float tile_V = 0f;
		
			// loop on all emitters		
			for(emitterIndex = 0; emitterIndex < Emitters.Count ; emitterIndex++){
				
				emitter = Emitters[emitterIndex];
				
				// define the chunk of particles to process for this emitter on this thread
				stp = Mathf.CeilToInt((float)emitter.particleLive / split);
				start = groupIndex * stp;
				end = Mathf.Min (start + stp,emitter.particleLive);
				
				if(end > start && end > 0){
					// loop on particles
					for(pointer = start; pointer < end; pointer++){
						index = emitter.particlesPointers[pointer];
						
						// check if the particle is in the camera view cone
						direction = (emitter.particles[index].position - camPos).normalized;
						if( Vector3.Dot(camDir,direction) > angle ){
													
							// the current sortgroup is full, add a new item
							if(count >= total){
								s = new SortIndex();
								_sortGroups[groupIndex].items.Add(s);
								total++;
							}else{
								// update existing item in the current sortgroup
								s = _sortGroups[groupIndex].items[count];
							}
							
							s.particleIndex = index;
							s.emitterIndex = emitterIndex;
							s.order = cameraMatrix.MultiplyPoint(emitter.particles[index].position).z;
						
							tile_index = Mathf.FloorToInt( Mathf.Max(0f,(1f-((lightMatrix.MultiplyVector(direction*-1).z + 1.0f)*0.5f))) * TextureFrameCount );
						
							row = Mathf.FloorToInt((float)tile_index / TextureColCount);
							tile_index = tile_index % TextureColCount;
							
							tile_U = pDirectionalTextureFrameWidth * tile_index;
							tile_V = rowHeight * row;
							
							s.textureTileIndex = tile_index;
							
							s.uvs[0].x = tile_U;
							s.uvs[0].y = tile_V;
							
							s.uvs[1].x = tile_U;
							s.uvs[1].y = tile_V + rowHeight;
							
							s.uvs[2].x = tile_U + pDirectionalTextureFrameWidth;
							s.uvs[2].y = tile_V + rowHeight;
							
							s.uvs[3].x = tile_U + pDirectionalTextureFrameWidth;
							s.uvs[3].y = tile_V;
							
							count++;
						}
					
					}
				}
			
			}
			
			_sortGroups[groupIndex].count = count;
			/*
			if(total > count){
				// delete trailing unused items, may cause a memory leak or GC lags, must be checked
				_sortGroups[groupIndex].items.RemoveRange(count,(total-count));				
			}
			*/
	}
	
	// ***********************************************************************************************************************
	// STEP 2) Sort the visible particles
	// ***********************************************************************************************************************
	
	private void SortParticles(){
		
		int i,j;
		int count = _sortGroups.Count;
		int threadAdded = 0;
		
		
		double tmp;
		
		if(!useThread){
			for(i = 0 ; i < count ; i++){
				if(_sortGroups[i].count > 0){
					tmp = Time.realtimeSinceStartup;
					_sortGroups[i].items.Sort(0 , _sortGroups[i].count , comp);
					_sortGroups[i].sortTime = Time.realtimeSinceStartup - tmp;
				}else{
					_sortGroups[i].sortTime = 0;
				}
			}
		}else{
		
			taskList.Clear();
			// loop on all used sort groups
			tmp = Time.realtimeSinceStartup;
			for(i = 0 ; i < count ; i++){
				
				_sortGroups[i].sortTime = 0;
				if(_sortGroups[i].count > 0){	
					var grp = i;
				
					taskList.Add(UnityThreadHelper.TaskDistributor.Dispatch(() => _sortGroups[grp].items.Sort(0 , _sortGroups[grp].count , comp) ));
					
					threadAdded++;
				}
				
				if(threadAdded == _coresCount){
					for(j = 0 ; j < threadAdded ; j++){
						taskList[j].Wait();
						taskList[j].Dispose();
					}
					_sortGroups[i].sortTime = Time.realtimeSinceStartup - tmp;
					threadAdded = 0;
					taskList.Clear();
					tmp = Time.realtimeSinceStartup;
				}
				
			}

			if(threadAdded > 0){
				for(j=0 ; j < threadAdded ; j++){
					taskList[j].Wait();
					taskList[j].Dispose();
				}
				taskList.Clear();
				_sortGroups[i-1].sortTime = Time.realtimeSinceStartup - tmp;
			}
		}
		
	}
	
	// ***********************************************************************************************************************
	
	
	private VariableMesh AddMesh(){
		VariableMesh vm = new VariableMesh(meshList.Count,particlesMaterial);
		meshList.Add(vm);
		return vm;
	}
	
	private List<SortIndex> mergedList = new List<SortIndex>();
	
	double convertTime = 0;
	
	void MergeGroups(){
		int groupCount = _sortGroups.Count;
		int groupIndex = -1;
		int g,i=0;
		
		float maxValue = -1f;
		float o = 0f;
		
		convertTime = Time.realtimeSinceStartup;
		SortGroup[] local = _sortGroups.ToArray();
			
		for(g = 0; g < groupCount; g++){
			local[g].ConvertToArray();
		}
		convertTime = Time.realtimeSinceStartup - convertTime;
		
		for(i = 0; i < visibleParticles ; i++){
				
			groupIndex = -1;
			maxValue = -1f;
			
			// loop on all sortgroups -> TODO : find an optimisation to not loop on all groups all the time
			
			for(g = 0; g < groupCount; g++){
				
				if(local[g].index < local[g].count){
					
					// find the group with the farthest particle

					o = local[g].CheckOrder(maxValue);
					if(o != -1){
						groupIndex = g;
						maxValue = o;
					}
					
				}
				
			}

			
			if(groupIndex > -1){
				
				if(i >= mergedList.Count){
					mergedList.Add(local[groupIndex].items[ local[groupIndex].index ]);
				}else{
					mergedList[i] = local[groupIndex].items[ local[groupIndex].index ];
				}
				
				local[groupIndex].index++;
			}
			
		}

	}
		
	void BuildMeshes() {
		
		buildTime = Time.realtimeSinceStartup;
		
		if(visibleParticles>0 && render){
			
			int i;
			int firstVertex = 0;
			int rendererMeshParticles = 0;
			int meshListIndex = 0;
			
			float size;
			
			float uvScale = 1f;
			
			Puffy_ParticleData p;
			
			VariableMesh currentMesh;
			if(meshList.Count == 0){
				currentMesh = AddMesh();
			}else{
				currentMesh = meshList[0];
			}
						
			VariableMeshData currentMeshData = currentMesh.Init(visibleParticles);
			
			SortIndex particlesData;
			meshUpdateCount = 0;
			
			particlesMaterial.SetColor("_LightColor",_light.color);
			
			if(useAmbientColor){
				if(cameraBackgroundAsAmbientColor){
					particlesMaterial.SetColor("_AmbientColor",_camera.backgroundColor);
				}else{
					particlesMaterial.SetColor("_AmbientColor",RenderSettings.ambientLight);
				}
			}
			
			particlesMaterial.SetFloat("_LightIntensity",_light.intensity*2);
			
			float bs0x = _billboardShape[0].x;
			float bs0y = _billboardShape[0].y;
			float bs0z = _billboardShape[0].z;
			
			float bs1x = _billboardShape[1].x;
			float bs1y = _billboardShape[1].y;
			float bs1z = _billboardShape[1].z;
			
			float bs2x = _billboardShape[2].x;
			float bs2y = _billboardShape[2].y;
			float bs2z = _billboardShape[2].z;
			
			float bs3x = _billboardShape[3].x;
			float bs3y = _billboardShape[3].y;
			float bs3z = _billboardShape[3].z;
			
			float colorR;
			float colorG;
			float colorB;
			float ageRatio;
			
			float uvOffsetX = 0;
			float uvOffsetY = 0;
			
			float uvRotationVectorX = 0f;
			float uvRotationVectorY = 0f;
			
			float _UVrotationVectorX = _UVrotationVector.x;
			float _UVrotationVectorY = _UVrotationVector.y;
			
			float posX = 0f;
			float posY = 0f;
			float posZ = 0f;
					
			int lv0,lv1,lv2,lv3;
		
			Vector3[] lVerts = currentMeshData.vertices;
			Color[] lColors = currentMeshData.colors;
			Vector2[] lUVs = currentMeshData.uvs;
			Vector2[] lUVs1 = currentMeshData.uvs1;
			
			// Init bounding box depth particle offset
			int lpos0 = 0;
			
			Vector3 lCameraPos = _cameraTransform.position;
			Vector3 lCameraForward = _cameraTransform.forward;
			
			for(i = 0; i < visibleParticles ; i++){
			
				particlesData = mergedList[i];
				
				p = Emitters[ particlesData.emitterIndex ].particles[ particlesData.particleIndex ];
				
				posX = p.position.x;
				posY = p.position.y;
				posZ = p.position.z;
				
				size = p.size;
				
				colorR = p.color.r;
				colorG = p.color.g;
				colorB = p.color.b;
				
				ageRatio = p.ageRatio;
			
				firstVertex = currentMesh.particleIndex * 4;
				
				currentMesh.center.x += posX;
				currentMesh.center.y += posY;
				currentMesh.center.z += posZ;
				
				lv0 = firstVertex;
				lv1 = firstVertex+1;
				lv2 = firstVertex+2;
				lv3 = firstVertex+3;
				
				// position
				lVerts[lv0].x = posX + bs0x * size;
				lVerts[lv0].y = posY + bs0y * size;
				lVerts[lv0].z = posZ + bs0z * size;
				
				lVerts[lv1].x = posX + bs1x * size;
				lVerts[lv1].y = posY + bs1y * size;
				lVerts[lv1].z = posZ + bs1z * size;
				
				lVerts[lv2].x = posX + bs2x * size;
				lVerts[lv2].y = posY + bs2y * size;
				lVerts[lv2].z = posZ + bs2z * size;
				
				lVerts[lv3].x = posX + bs3x * size;
				lVerts[lv3].y = posY + bs3y * size;
				lVerts[lv3].z = posZ + bs3z * size;
				
				// color
				lColors[lv0].r   = colorR;
				lColors[lv0].g   = colorG;
				lColors[lv0].b   = colorB;
				lColors[lv0].a   = ageRatio;
				
				lColors[lv1].r   = colorR;
				lColors[lv1].g   = colorG;
				lColors[lv1].b   = colorB;
				lColors[lv1].a   = ageRatio;
				
				lColors[lv2].r   = colorR;
				lColors[lv2].g   = colorG;
				lColors[lv2].b   = colorB;
				lColors[lv2].a   = ageRatio;
				
				lColors[lv3].r   = colorR;
				lColors[lv3].g   = colorG;
				lColors[lv3].b   = colorB;
				lColors[lv3].a   = ageRatio;
							
				// uv tile
				lUVs[lv0].x = particlesData.uvs[0].x;
				lUVs[lv0].y = particlesData.uvs[0].y;
				
				lUVs[lv1].x = particlesData.uvs[1].x;
				lUVs[lv1].y = particlesData.uvs[1].y;
				
				lUVs[lv2].x = particlesData.uvs[2].x;
				lUVs[lv2].y = particlesData.uvs[2].y;
				
				lUVs[lv3].x = particlesData.uvs[3].x;
				lUVs[lv3].y = particlesData.uvs[3].y;	
			
				// uv smoke details
				uvScale = detailsScaling + 0.5f * ageRatio; // scaling anim
				
				uvOffsetX = uvOffsetY = p.randomSeed;
									
				uvRotationVectorX = _UVrotationVectorX * uvScale;
				uvRotationVectorY = _UVrotationVectorY * uvScale;
				
				lUVs1[lv0].x = uvOffsetX + uvRotationVectorY;
				lUVs1[lv0].y = uvOffsetY - uvRotationVectorX;
				
				lUVs1[lv1].x = uvOffsetX - uvRotationVectorX;
				lUVs1[lv1].y = uvOffsetY - uvRotationVectorY;
				
				lUVs1[lv2].x = uvOffsetX - uvRotationVectorY;
				lUVs1[lv2].y = uvOffsetY + uvRotationVectorX;

				lUVs1[lv3].x = uvOffsetX + uvRotationVectorX;
				lUVs1[lv3].y = uvOffsetY + uvRotationVectorY;
		
				//-----------------------------------------------------------------
				currentMesh.particleIndex ++;
				rendererMeshParticles ++;
				
				// current mesh limit has been reached, use a new mesh
				if(currentMesh.particleIndex >= currentMesh.maxCount){
					
					// compute Z-Depth center for bounding box
					currentMesh.boundMin = lCameraPos + lCameraForward * mergedList[lpos0].order;
					currentMesh.boundMax = lCameraPos + lCameraForward * mergedList[i].order;
					lpos0=i+1;
					
					if(i < visibleParticles-1){
						meshListIndex ++;
						
						if(meshListIndex >= meshList.Count){
							currentMesh = AddMesh();
						}else{
							currentMesh = meshList[meshListIndex];
						}
						
						currentMeshData = currentMesh.Init(visibleParticles - rendererMeshParticles);
						lVerts = currentMeshData.vertices;
						lColors = currentMeshData.colors;
						lUVs = currentMeshData.uvs;
						lUVs1 = currentMeshData.uvs1;
					}
					
				}
				//--------------------------------------------------------------------
				
			}
			
			if (lpos0<visibleParticles){
				currentMesh.boundMin = lCameraPos + lCameraForward * mergedList[lpos0].order;
				currentMesh.boundMax = lCameraPos + lCameraForward * mergedList[visibleParticles-1].order;
			}

			meshUpdateCount = meshListIndex+1;	 	
						
		}
		buildTime = Time.realtimeSinceStartup - buildTime;
	}
	
	private void UpdateMeshes(){
		updateMeshTime = 0;
		visibleMeshesCount = 0;
		if(meshList.Count>0){
			int i;
			for(i=0;i<meshUpdateCount;i++){
				updateMeshTime += meshList[i].UpdateMesh();
				visibleMeshesCount ++;
			}
			
			for(i = meshUpdateCount ; i < meshList.Count ; i++){
				updateMeshTime += meshList[i].ClearMesh();
			}
			
		}
	}
	
	private void UpdateOneMesh(int index){
		meshList[index].UpdateMesh();
	}
	
	private class SortGroup{
		public int index;
		public int count;
		public double sortTime;
		public List<SortIndex> items = new List<SortIndex>();
		
		private SortIndex[] array;
		
		public void ConvertToArray(){
			array = items.ToArray();
		}
		
		public float CheckOrder(float distance){
			if(array[index].order >= distance){
				return array[index].order;
			}
			return -1;
		}
	}
	
	private class SortIndex{
		public int emitterIndex;
		public int particleIndex;
		public int textureTileIndex = 0;
		public Vector2[] uvs = new Vector2[4];
		public float order;
		
		
	}
	
	private class OrderComparer : IComparer<SortIndex>
	{
		public int Compare(SortIndex a, SortIndex b)
		{
			// sort back to front
			return (a.order > b.order)?-1:1;
		}
	}
			
	private class VariableMesh{
		
		private List<VariableMeshData> meshData = new List<VariableMeshData>();
		
		public Vector3 boundMin = Vector3.zero;
		public Vector3 boundMax = Vector3.zero;
		
		public GameObject gameObject;
		public Mesh mesh;
		public Vector3 center = Vector3.zero;		
		
		public int maxCount = 16000;
		public int meshDataIndex = 0;
		public int particleIndex = 0;
		
		public double updateTime = 0f;

		public Transform _transform;
		private bool needUpdate = true;
		
		public VariableMesh(int n=0 , Material mat = null){
			
			int step = Mathf.FloorToInt(maxCount / 8);
			int i = step;
			
			while( i <= maxCount){
				meshData.Add(new VariableMeshData(Mathf.Min(i,maxCount)));
				i += step; 
			}
			
			mesh = new Mesh();
			
			mesh.MarkDynamic();
			
			gameObject = new GameObject();
			gameObject.name = "Puffy_MESH_"+n.ToString();
			MeshFilter mf = gameObject.AddComponent("MeshFilter") as MeshFilter;
			mf.sharedMesh = mesh;
			
			MeshRenderer mr = gameObject.AddComponent("MeshRenderer") as MeshRenderer;
			mr.material = mat;
			
			_transform = gameObject.transform;
		}
		
		public int getMeshDataIndex(int particleCount){
			meshDataIndex = 0;
			while(meshData[meshDataIndex].particleCount < particleCount){
				meshDataIndex++;
				if(meshDataIndex >= meshData.Count){
					meshDataIndex = meshData.Count-1;
					break;
				}
			}
			return meshDataIndex;
		}
		
		public VariableMeshData getMeshData(int particleCount){
			int i = getMeshDataIndex(particleCount);
			if(i < 0) return null;
			return meshData[i];
		}
		
		public VariableMeshData Init(int particleCount){
			particleIndex = 0;
			center = Vector3.zero;
			needUpdate = true;
			return getMeshData(particleCount);
		}
			
		
		public double ClearMesh(){
			updateTime = Time.realtimeSinceStartup;
			
			mesh.Clear(false);
			mesh.RecalculateBounds();
			particleIndex = 0;
			
			updateTime = Time.realtimeSinceStartup - updateTime;
			return updateTime;
		}
		
		public double UpdateMesh(){
			if(needUpdate && particleIndex > 0){
				updateTime = Time.realtimeSinceStartup;
				
				needUpdate = false;
				
				bool sameMesh = mesh.triangles.Length == meshData[meshDataIndex].triangles.Length;
				
				if(!sameMesh) mesh.Clear(false);
				
				if(center.sqrMagnitude > 0) center /= particleIndex;
				
				int i;
				int start = particleIndex * 4;
				int cnt = meshData[meshDataIndex].vertexCount;
				
				float posX = center.x;
				float posY = center.y;
				float posZ = center.z;
				
				Vector3[] lVerts = meshData[meshDataIndex].vertices;
				
				
				for(i = 0 ; i < start ; i++){
					lVerts[i].x -= posX;
					lVerts[i].y -= posY;
					lVerts[i].z -= posZ;
				}
				
				posX = lVerts[0].x;
				posY = lVerts[0].y;
				posZ = lVerts[0].z;
				
				for(i = start ; i < cnt ; i++){
					lVerts[i].x = posX;
					lVerts[i].y = posY;
					lVerts[i].z = posZ;
				}
				
				mesh.vertices = lVerts;
				mesh.uv = meshData[meshDataIndex].uvs;
				mesh.uv1 = meshData[meshDataIndex].uvs1;
				mesh.colors = meshData[meshDataIndex].colors;
				if(!sameMesh) mesh.triangles = meshData[meshDataIndex].triangles;
						
				_transform.position = center;

				mesh.bounds = new Bounds((boundMax+boundMin)*.5f-center,Vector3.one);
				
				updateTime = Time.realtimeSinceStartup - updateTime;
				return updateTime;
			}
			return 0;
			
		}
			
	}
	
	private class VariableMeshData{
		
		public int particleCount = 0;
		public int vertexCount = 0;
		public int[] triangles;
		public Vector2[] uvs;
		public Vector2[] uvs1;
		public Vector3[] vertices;
		//public Vector4[] extraData;
		public Color[] colors;
		
		public VariableMeshData(int _particleCount){
			
			particleCount = _particleCount;
			
			vertexCount = particleCount * 4;
			
			vertices = new Vector3[vertexCount];
			uvs = new Vector2[vertexCount];
			uvs1 = new Vector2[vertexCount];
			colors = new Color[vertexCount];
			//extraData = new Vector4[vertexCount];
			
			triangles = new int[particleCount * 6];
			
			int v,j,k;
			for(v = 0 ; v < particleCount ; v++){
				
				j = v*4;
				k = v*6;
				
				triangles[k]   = j;
				triangles[k+1] = j+1;
				triangles[k+2] = j+2;
				triangles[k+3] = j;
				triangles[k+4] = j+2;
				triangles[k+5] = j+3;
		
				uvs1[j]   = new Vector2(-0.5f,-0.5f); // bottom left
				uvs1[j+1] = new Vector2(0.5f,-0.5f); // bottom right
				uvs1[j+2] = new Vector2(0.5f,0.5f); // top right
				uvs1[j+3] = new Vector2(-0.5f,0.5f); // top left
			}
		}
	}
}

public class DebugTimer{
	private float[] times = new float[50];
	
	private double timestamp;
	
	public float elapsed;
	public float maximum;
	public float minimum;
	
	public void Start(){
		timestamp = Time.realtimeSinceStartup;
	}
	
	public void Stop(){
		timestamp = Time.realtimeSinceStartup - timestamp;
		elapsed = (float)(timestamp*1000);
		times[49] = elapsed;
		for(int i = 0; i < 49; i++){
			times[i] = times[i+1];
		}
		
	}
	
	public float Average(){
		float a = 0f;
		maximum = 0f;
		minimum = 99999;
		for(int i = 0; i < 50; i++){
			a += times[i];
			maximum = Mathf.Max (maximum,times[i]);
			minimum = Mathf.Min (minimum,times[i]);
		}
		
		return a/50f;
	}
	
	public void ShowGraph(float offsetY = 0f){
		
		GL.Begin(GL.LINES); 
		Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward + Camera.main.transform.up*offsetY;
		Vector3 up = Camera.main.transform.up;
		Vector3 right = Camera.main.transform.right;
		
		Color[] colors = new Color[5];
		
		colors[0] = new Color(1f,1f,1f,1f);
		colors[1] = new Color(0f,1f,0f,1f);
		colors[2] = new Color(1f,1f,0f,1f);
		colors[3] = new Color(1f,0.5f,0f,1f);
		colors[4] = new Color(1f,0f,0f,1f);
		
		GL.Color(new Color(1f,1f,1f,0.5f));
		
		GL.Vertex(pos);
		GL.Vertex(pos + right * 0.25f);
		
		GL.Vertex(pos + up / 50f);
		GL.Vertex(pos + up / 50f + right * 0.25f);
		
		GL.Vertex(pos + (up*2) / 50f);
		GL.Vertex(pos + (up*2) / 50f + right * 0.25f);
		
		pos = Camera.main.transform.position + Camera.main.transform.forward + Camera.main.transform.up*offsetY;
		int c = 0;
		for(int i = 0; i < 50; i++){
			
			c = Mathf.Min (4,Mathf.FloorToInt(times[i]));
			GL.Color(colors[c]);
			GL.Vertex( pos);
	    	GL.Vertex( pos + up * times[i]/50f );
			pos += right*0.005f;
		}
		GL.End();
		
	}
}
