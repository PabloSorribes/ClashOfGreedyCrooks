@script AddComponentMenu ("Enviro Spawn/ Enviro Spawn")
var gizmoColor : Color = Color(0,0,1);
public var prefabs : GameObject[];
public var population : int = 1;
public var dimensions : Vector2 = Vector2(2,2);
public var scaleVariation : Vector2 = Vector2(0.5,1.5);
public var rotationVariation : Vector2 = Vector2(0,360);
public var layerMask : LayerMask = -1;
//public var snapToGround : boolean = false;
public var offset : float = 0;
@HideInInspector
public var instanceObjects : GameObject[];
@HideInInspector
public var raycastPositions : Vector3[];
@HideInInspector
public var raycastPositionsBeta : Vector3[];
public var followNormalsOrientation : boolean = true;

@HideInInspector
public var fixedPositioning : boolean;
@HideInInspector
public var offsetInEachCell : boolean = true;
@HideInInspector
public var fixedGridScale : float = 1;
@HideInInspector
public var scatterMode : int = 0; //random, fixed, equal

@HideInInspector
public var yRotations : float[];
private var updateCallCount : int = 0;
private var updateSkip : int = 4;
//progress bar
@HideInInspector
static var progressBar : int = 0;

@HideInInspector
var cCheck : boolean = false;

function OnDrawGizmos () {
	var rotationMatrix : Matrix4x4 = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
	Gizmos.matrix = rotationMatrix;

	Gizmos.color = gizmoColor;
	Gizmos.DrawWireCube(Vector3.zero,Vector3(dimensions.x * transform.localScale.x,0.1,dimensions.y * transform.localScale.z));
	Gizmos.color = Color(gizmoColor.r,gizmoColor.g,gizmoColor.b,gizmoColor.a * 0.1);
	Gizmos.DrawCube(Vector3.zero,Vector3(dimensions.x * transform.localScale.x,0.1,dimensions.y * transform.localScale.z));
	//Gizmos.color = Color(1,1,1,1);
	//if(raycastPositionsBeta != null) 
	//for(var pp = 0; pp < raycastPositionsBeta.length; pp++){
	//	Gizmos.DrawSphere(transform.position + raycastPositions[pp], 0.1);
	//	Gizmos.DrawSphere(raycastPositionsBeta[pp], 0.1);
	//}
	if (Application.isEditor)
		if(updateCallCount < updateSkip)
			updateCallCount++;
		else{
			updateCallCount = 0;
			UpdateData();
		}
}

function MassInstantiateNew () {
	var objs : EnviroSpawn[] = GameObject.FindObjectsOfType(EnviroSpawn) as EnviroSpawn[];
	for(var i = 0; i < objs.length; i++){
		objs[i].InstantiateNew();
		objs[i].UpdateData();
	}
}

function InstantiateNew () {
	cCheck = false;
	Reset(); // make sure the object is reset
		
		raycastPositions = new Vector3[population];
		raycastPositionsBeta = new Vector3[population];
		instanceObjects = new GameObject[population];
		yRotations = new float[population];
		
		GenerateRaycastPoints();
		
		for(var p = 0; p < population; p ++ ){
			if(!instanceObjects[p]){
				if(prefabs.length > 0){
					var randomInstId : int = Random.Range(0,prefabs.length);
					if(prefabs[randomInstId])
						var prop = Instantiate(
						prefabs[randomInstId]
						,raycastPositionsBeta[p]
						,Quaternion.identity);
					if(prop){
						yRotations[p] = Random.Range(rotationVariation.x, rotationVariation.y);
						
						var _scale = Random.Range(scaleVariation.x, scaleVariation.y);
						prop.transform.localScale = Vector3(_scale,_scale ,_scale);
						prop.transform.parent = this.transform;
						instanceObjects[p] = prop;
					}
				}
			}
		}
}
private function UpdateData () {
	
	/* NEW VERSION VARIABLE UPDATE PROCEDURE */
	//if the old 'fixed grid variable(boolean)' is used, transfer the information to the new one without damage done to the user.
	if(fixedPositioning) 
	{
		fixedPositioning = false;
		scatterMode = 1;
	}
	/*---------------------------------------*/
	
	
	//Debug.Log("update data called");
	if(population < 0){
		population = 0;
		return;
	}
	if(raycastPositions)
	if(raycastPositions.length != population){ //Instantiate
		InstantiateNew();
	}
	else{
		for(var r = 0; r < population; r ++){
				var hit : RaycastHit;
				if(Physics.Raycast(transform.position + transform.right*raycastPositions[r].x + transform.up*raycastPositions[r].y + transform.forward*raycastPositions[r].z, Vector3.down, hit, Mathf.Infinity, layerMask)){
					raycastPositionsBeta[r] = hit.point;
				}else
					raycastPositionsBeta[r] = transform.position + raycastPositions[r];
				if(instanceObjects.length > 0) if(instanceObjects[r])
				instanceObjects[r].transform.position = raycastPositionsBeta[r] + hit.normal*offset;
				
				var normalQuaternion : Quaternion = Quaternion.FromToRotation (Vector3.up, hit.normal);
				if(!followNormalsOrientation){ //if follow normals is :off: reset the rotation
					normalQuaternion = Quaternion.identity;
				}
				if(instanceObjects.length > 0) if(instanceObjects[r]){
					instanceObjects[r].transform.eulerAngles = Vector3(normalQuaternion.eulerAngles.x,normalQuaternion.eulerAngles.y,normalQuaternion.eulerAngles.z);
					instanceObjects[r].transform.Rotate(0,yRotations[r],0,Space.Self);
				}
		}
	}
}

private function GenerateRaycastPoints () {
	var x = dimensions.x;
	var y = dimensions.y;
		var lc : int = 0; //loop count
	
	 if(scatterMode == 0)
		for(var r = 0; r < population; r ++){
			//get flat plane positions for each population id
			raycastPositions[r] = Vector3(Random.Range(-dimensions.x/2 * transform.localScale.x, dimensions.x/2 * transform.localScale.z)
			,0
			,Random.Range(-dimensions.y/2 * transform.localScale.x, dimensions.y/2 * transform.localScale.z));
		}
		
	if(scatterMode == 1){
		var tp : float = parseFloat(population); //r
		var c = tp/parseFloat(x*y); //expected cycles
		cCheck = false; if(Mathf.Floor(c) > 0) cCheck = true;
		lc = 0;
		for(var cn = 0; cn < c; cn++) //na - cycle number
		{
			var localCellOffset : float = fixedGridScale / c * cn; //p
			for(var ay = 0; ay < y; ay++)
				for(var ax = 0; ax < x; ax++){
					if(lc < raycastPositions.length) raycastPositions[lc] = Vector3(ax * fixedGridScale - x/2 + fixedGridScale/2,0,ay * fixedGridScale - y/2 + fixedGridScale/2);
					lc++;
				}
		}
	}
	
	if(scatterMode == 2){
		var a : int = Mathf.Sqrt(parseFloat(population)*dimensions.x/dimensions.y); //horizontal cell count
		var b : int = parseFloat(population)/parseFloat(a); //vertical cell count
		lc = 0;
		for(var a1 = 0; a1 < a; a1++)
			for(var b1 = 0; b1 < b; b1++){
				raycastPositions[lc] = Vector3(dimensions.x/a*a1 - (dimensions.x/2 - dimensions.x/a/2),0,dimensions.y/b*b1 - (dimensions.y/2 - dimensions.y/b/2));
				lc++;
			}
	}
}
function Reset () {
	if(!instanceObjects)
		return;
	for(var n = 0; n < instanceObjects.length; n++){
		if(instanceObjects[n] != null){
			DestroyImmediate(instanceObjects[n].gameObject);
		}
	}
	
	raycastPositions = new Vector3[0];
	raycastPositionsBeta = new Vector3[0];
	instanceObjects = new GameObject[0];
}