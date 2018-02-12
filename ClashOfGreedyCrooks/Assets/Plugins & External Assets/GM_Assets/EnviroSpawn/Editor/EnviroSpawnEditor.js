@CustomEditor (EnviroSpawn)
#if (UNITY_EDITOR)
public class EnviroSpawnEditor extends Editor
{
	var scatterModeOption : String[] = ["Random","Fixed Grid","Even Spread"];
	
	function OnInspectorGUI() {
        DrawDefaultInspector();
		
		var script : EnviroSpawn = target;
	
		script.scatterMode = EditorGUILayout.Popup("Scatter Mode", script.scatterMode, scatterModeOption);
		
		if(script.scatterMode == 1){
			//script.offsetInEachCell = EditorGUILayout.Toggle("Offset In Each Cell", script.offsetInEachCell);
			script.fixedGridScale = EditorGUILayout.FloatField("Grid Scale ", script.fixedGridScale);
		}
		
		if(GUILayout.Button("Generate")){
			script.InstantiateNew();
		}
		if(GUILayout.Button("Re-Generate All In Scene")){
			script.MassInstantiateNew();
		}
		
		if(script.cCheck)
			GUILayout.Box("WARNING: The prefabs may overlap! Grid cycles > 0!");
	}
	
	function OnEditorGUI () {
		var scriptg : EnviroSpawn = target;
		
	}
	
	function OnInspectorUpdate() {
		Repaint();
	}
}
#endif