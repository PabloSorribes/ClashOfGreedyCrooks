using System;
using UnityEngine;

namespace UnityEditor
{
	public class BV_MultiBlendGUI : ShaderGUI
	{

		private static class Titles
		{
			//------------------------------------------------------------Main Foldouts
			public static string mainMapsText = "Main Texture Controls";
			public static string topMapsText = "Top Texture Controls";
			public static string snowMapsText = "Snow Controls";
			//------------------------------------------------------------Headers
			public static string globalHeaderText = "Global Settings";
			public static string mainHeaderText = "Main Textures";
			public static string snowHeaderText = "Snow Textures";



			//-----------------------------------------------------------------Labels
			public static string IntensityText = "Fade Out Effect";
			public static GUIContent maskText = new GUIContent("Mask Source", "R,G,B,A images will be used");

			public static GUIContent baseOcclusionText = new GUIContent("Occlusion", "General Occlusion of Main Texture");
			public static GUIContent baseMapText = new GUIContent("Main Base Texture", "Albedo (RGB) and Transparency (A)");
			public static GUIContent baseNormal1Text = new GUIContent("Base Normal 1 Texture", "Base Normal Map");
			public static GUIContent baseNormalPOWText = new GUIContent("Base Normal 1 Power", "Base Normal Map Intensity");
			public static GUIContent baseGlossText = new GUIContent("Base Smoothness", "General Glossiness of Main Level");
			public static GUIContent specMapText = new GUIContent("Gloss Map", "Map controlling Glossiness");

			public static GUIContent topMapText = new GUIContent("Top Base Texture", "Albedo (RGB) and Transparency (A)");
			public static GUIContent topNormal1Text = new GUIContent("Top Normal 1 Texture", "Top Normal Map 1");
			public static GUIContent topNormal2Text = new GUIContent("Top Normal 2 Texture", "Top Normal Map 2");
			public static GUIContent topNormalPOWText = new GUIContent("Top Normal 1 Power", "Top Normal Map Intensity");

			public static GUIContent snowDirectionText = new GUIContent("Snow Direction", "World Direction of Snow Projection");
			public static GUIContent snowMapText = new GUIContent("Base Snow Albedo", "Albedo (RGB)");
			public static GUIContent snowNormal1Text = new GUIContent("Normal 1", "Base Snow Normal Map");
			public static GUIContent snowNormal2Text = new GUIContent("Normal 2", "Second Base Snow Normal Map");
			public static GUIContent snowNormalPOWText = new GUIContent("Snow Normal Power", "Snow Normal Map Intensity");
			public static GUIContent snowGlossText = new GUIContent("Snow Smoothness", "General Glossiness of Snow");
			public static GUIContent snowGlossTilingText = new GUIContent("Sparkles Tiling", "Tile Amount of Snow Gloss map");
			public static GUIContent snowspecMapText = new GUIContent("Snow Gloss Map", "Sparkles Map");
			public static GUIContent snowLevelText = new GUIContent("Snow Level", "Amount of Snow");
			public static GUIContent snowDepthText = new GUIContent("Snow Depth", "Depth of Snow Level");
			public static GUIContent snowMaskTiling = new GUIContent("1st Mask Tiling", "How much to tile 1st Mask");
			public static GUIContent snowMaskTiling2 = new GUIContent("2nd Mask Tiling", "How much to tile 2nd Mask");

			public static string topMapMaskText = "Top Masking Options";
			public static string snowMapMaskText = "Snow Masking Options";

		}


		//==========================================================================Foldouts
		MaterialProperty _MainShown = null;
		MaterialProperty _TopShown = null;
		MaterialProperty _SnowShown = null;
		MaterialProperty _Mask1Shown = null;
		MaterialProperty _SnowMaskShown = null;
		//========================================================================== Main
		MaterialProperty overallIntensity = null;
		MaterialProperty maskMap = null;

		MaterialProperty baseMap = null;
		MaterialProperty baseMapColor = null;
		MaterialProperty baseGlossMap = null;
		MaterialProperty baseGlossColor = null;
		MaterialProperty mainGloss = null;
		MaterialProperty normalMain1 = null;
		MaterialProperty normalMainPOW = null;
		MaterialProperty mainNormalMultiplier = null;
		MaterialProperty mainNormalDetailBlend = null;
		MaterialProperty mainNormalBlendChoice = null;
		//========================================================================== Top
		MaterialProperty topDirection = null;
		MaterialProperty topBase = null;
		MaterialProperty topColor = null;
		MaterialProperty topLevel = null;
		MaterialProperty topDepth = null;
		MaterialProperty topGlossAmount = null;
		MaterialProperty topNorm = null;

		MaterialProperty topNormPOW = null;
		MaterialProperty topNormBlendChoice = null;
		MaterialProperty topDetailMultiplier = null;
		MaterialProperty topBlendNorm = null;
		MaterialProperty topBlendAlbedo = null;
		MaterialProperty topMaskChoice= null;
		MaterialProperty showTopMask = null;
		MaterialProperty topMaskDepth = null;
		MaterialProperty topMaskIntensityDepth = null;
		MaterialProperty topMaskChoice2= null;

		MaterialProperty topMaskDepth2 = null;
		MaterialProperty topMaskIntensityDepth2 = null;
		MaterialProperty topMaskTiling = null;
		MaterialProperty topMaskTiling2 = null;
		MaterialProperty topMask2BlendAmount = null;
		MaterialProperty topMaskBlendStyle = null;

		//========================================================================== Snow
		MaterialProperty snowDirection = null;
		MaterialProperty snowDisplace = null;
		MaterialProperty snowBase = null;
		MaterialProperty snowColor = null;
		MaterialProperty snowLevel = null;
		MaterialProperty snowDepth = null;
		MaterialProperty snowGlossAmount = null;
		MaterialProperty snowGlossMap = null;
		MaterialProperty snowGlossTiling = null;
		MaterialProperty snowNorm = null;
		MaterialProperty snowNorm2 = null;
		MaterialProperty snowNormPOW = null;
		MaterialProperty snowBlendNorm = null;
		MaterialProperty snowMaskChoice= null;
		MaterialProperty showSnowMask = null;
		MaterialProperty snowMaskDepth = null;
		MaterialProperty snowMaskIntensityDepth = null;
		MaterialProperty snowMaskChoice2= null;
		MaterialProperty snowMaskDepth2 = null;
		MaterialProperty snowMaskIntensityDepth2 = null;
		MaterialProperty snowMaskTiling = null;
		MaterialProperty snowMaskTiling2 = null;
		MaterialProperty snowMask2BlendAmount = null;
		MaterialProperty snowMaskBlendStyle = null;


		MaterialEditor m_MaterialEditor;


		// associate above properties with shader parameters
		public void FindProperties (MaterialProperty[] props)
		{
			//-------------------------------------------Foldouts
			_MainShown = FindProperty ("_MainShown", props);
			_TopShown = FindProperty ("_TopShown", props);
			_Mask1Shown = FindProperty ("_Mask1Shown", props);
			_SnowShown = FindProperty ("_SnowShown", props);
			_SnowMaskShown = FindProperty ("_SnowMaskShown", props);
			//-------------------------------------------Maps
			overallIntensity = FindProperty("_Intensity", props);
			maskMap = FindProperty ("_Mask", props);

			//baseOcclusion = FindProperty ("_OcclusionStrength", props);
			baseMap = FindProperty ("_MainTex", props);
			baseMapColor = FindProperty ("_Color", props);
			normalMain1 = FindProperty("_Bump", props);
			normalMainPOW = FindProperty("_BumpPower", props);
			mainNormalMultiplier = FindProperty("_MainDetailMultiplier", props);
			mainNormalDetailBlend = FindProperty("_MainBlendNormals", props);
			mainNormalBlendChoice = FindProperty("_MainNormalBlendChoice", props);
			mainGloss = FindProperty("_Glossiness", props);
			baseGlossMap = FindProperty("_GlossMap", props);
			baseGlossColor = FindProperty("_GlossColor", props);
			//------------------------------------------------------------ TOP
			topDirection = FindProperty("_TopDirection", props);
			topBase = FindProperty("_TopTex", props);
			topColor = FindProperty("_TopColor", props);
			topLevel = FindProperty("_TopLevel", props);
			topDepth = FindProperty("_TopDepth", props);
			topGlossAmount = FindProperty("_TopGlossiness", props);
			topNorm = FindProperty("_TopNorm", props);
			topNormPOW = FindProperty("_TopNMPower", props);
			topNormBlendChoice = FindProperty("_TopNormalBlendChoice", props);
			topDetailMultiplier = FindProperty("_TopDetailMultiplier", props);
			topBlendNorm = FindProperty("_TopBlendNormals", props);
			topBlendAlbedo = FindProperty("_TopAlbedoBlend", props);
			topMaskChoice = FindProperty("_TopMaskChoice", props);
			showTopMask = FindProperty("_showTopMask", props);
			topMaskDepth  = FindProperty("_TopMaskDepth", props);
			topMaskIntensityDepth = FindProperty("_TopMaskIntensityDepth", props);
			topMaskChoice2 = FindProperty("_TopMaskChoice2", props);
			topMaskBlendStyle = FindProperty("_TopMaskBlendStyle", props);
			topMaskDepth2 = FindProperty("_TopMaskDepth2", props);
			topMaskIntensityDepth2 = FindProperty("_TopMaskIntensityDepth2", props);
			topMaskTiling = FindProperty("_TopMaskTiling", props);
			topMaskTiling2 = FindProperty("_TopMask2Tiling", props);
			topMask2BlendAmount = FindProperty("_TopMask2Blend", props);

			//------------------------------------------------------------ SNOW
			snowDirection = FindProperty("_SnowDirection", props);
			snowDisplace = FindProperty("_snowDisplace", props);
			snowBase = FindProperty("_SnowTex", props);
			snowColor = FindProperty("_SnowColor", props);
			snowLevel = FindProperty("_SnowLevel", props);
			snowDepth = FindProperty("_SnowDepth", props);
			snowGlossAmount = FindProperty("_SnowGlossiness", props);
			snowGlossMap = FindProperty("_SnowSpec", props);
			snowGlossTiling = FindProperty("_SnowSpecTiling", props);
			snowNorm = FindProperty("_SnowNorm", props);
			snowNorm2 =FindProperty("_SnowNorm2", props);
			snowNormPOW = FindProperty("_SnowNMPower", props);
			snowBlendNorm = FindProperty("_SnowBlendNormals", props);

			snowMaskChoice = FindProperty("_SnowMaskChoice", props);
			showSnowMask = FindProperty("_showSnowMask", props);
			snowMaskDepth  = FindProperty("_SnowMaskDepth", props);
			snowMaskIntensityDepth = FindProperty("_SnowMaskIntensityDepth", props);
			snowMaskChoice2 = FindProperty("_SnowMaskChoice2", props);
			snowMaskBlendStyle = FindProperty("_SnowMaskBlendStyle", props);
			snowMaskDepth2 = FindProperty("_SnowMaskDepth2", props);
			snowMaskIntensityDepth2 = FindProperty("_SnowMaskIntensityDepth2", props);
			snowMaskTiling = FindProperty("_SnowMaskTiling", props);
			snowMaskTiling2 = FindProperty("_SnowMask2Tiling", props);
			snowMask2BlendAmount = FindProperty("_SnowMask2Blend", props);

		}

		public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] props)
		{
			m_MaterialEditor = materialEditor;
			Material material = materialEditor.target as Material;
			FindProperties (props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
			ShaderPropertiesGUI (material, props);

		}//end onGUI

		public void ShaderPropertiesGUI (Material material, MaterialProperty[] props)
		{
			// Use default labelWidth
			EditorGUIUtility.labelWidth = 0f;
			
			// Detect any changes to the material
			EditorGUI.BeginChangeCheck ();
			{
				Color bCol = GUI.backgroundColor;
				GUI.backgroundColor = new Color (0.9f, 1.0f, 0.9f, 0.9f);
				GUI.backgroundColor = bCol;	


				//===============================================================================================
				EditorGUILayout.BeginVertical ("Button");/////////////////////////////////////////////////// MAIN
				GUILayout.Label (Titles.mainMapsText, EditorStyles.boldLabel);
				// show/hide block
				if (true) {//_MainShown.floatValue==1) {
					Color col = GUI.color;
					GUI.color = new Color (1.0f, 0.0f, 0.0f, 1f);
					Rect rect = GUILayoutUtility.GetLastRect ();
					rect.x += EditorGUIUtility.currentViewWidth - 50;
					EditorGUI.BeginChangeCheck ();
					float nval = EditorGUI.Foldout (rect, _MainShown.floatValue == 1, "") ? 1 : 0;
					if (EditorGUI.EndChangeCheck ()) {
						_MainShown.floatValue = nval;
					}
					GUI.color = col;
				}
				if (_MainShown.floatValue == 1) {//------------------------------ready to reveal parameters

					GUILayout.Label (Titles.globalHeaderText, EditorStyles.boldLabel);
					m_MaterialEditor.RangeProperty(overallIntensity, "Fade Out Effects on Material");
					m_MaterialEditor.TexturePropertySingleLine (Titles.maskText, maskMap);
					m_MaterialEditor.TextureScaleOffsetProperty (maskMap);


					GUILayout.Space (25);

					GUILayout.Label (Titles.mainHeaderText, EditorStyles.boldLabel);
					m_MaterialEditor.TexturePropertySingleLine (Titles.baseMapText, baseMap, baseMapColor);
					m_MaterialEditor.TexturePropertySingleLine (Titles.baseNormal1Text, normalMain1, normalMainPOW);
					m_MaterialEditor.TexturePropertySingleLine(Titles.specMapText, baseGlossMap, baseGlossColor);


					if (baseGlossMap.textureValue == null)
						m_MaterialEditor.ShaderProperty( mainGloss, "Smoothness", 2);
					    m_MaterialEditor.ShaderProperty(mainNormalBlendChoice, "Choose Normal Channel", 2);
					m_MaterialEditor.ShaderProperty(mainNormalMultiplier, "Blend Tile Multiplier", 2);
					m_MaterialEditor.ShaderProperty(mainNormalDetailBlend, "Blend Normals Amount", 2);

					m_MaterialEditor.TextureScaleOffsetProperty (baseMap);


					//m_MaterialEditor.RangeProperty (mainGloss, "Main Smoothness");




					
					//albedoMap2.textureScaleAndOffset = albedoMap.textureScaleAndOffset; // Apply the main texture scale and offset to the emission texture as well, for Enlighten's sake
										
				}
				EditorGUILayout.EndVertical ();/////////////////////////////////////////////////////////////////
				//===============================================================================================

				EditorGUILayout.Space ();

				//===============================================================================================
				EditorGUILayout.BeginVertical ("Button");////////////////////////////////////////////////// TOP
				GUI.backgroundColor = bCol;
				GUILayout.Label (Titles.topMapsText, EditorStyles.boldLabel);
				// show/hide block
				if (true) {
					Color col = GUI.color;
					GUI.color = new Color (1.0f, 1.0f, 0f, 1f);
					Rect rect = GUILayoutUtility.GetLastRect ();
					rect.x += EditorGUIUtility.currentViewWidth - 47;
								
					EditorGUI.BeginChangeCheck ();
					float nval = EditorGUI.Foldout (rect, _TopShown.floatValue == 1, "") ? 1 : 0;
					if (EditorGUI.EndChangeCheck ()) {
						_TopShown.floatValue = nval;
					}
					
					GUI.color = col;
				}
				if (_TopShown.floatValue == 1) {
					//====================================================================Actual properties to dislay
					EditorGUILayout.BeginVertical ("box");
					m_MaterialEditor.ShaderProperty(topDirection,"Top direction");
					m_MaterialEditor.RangeProperty(topGlossAmount, "Top Smoothness");
					m_MaterialEditor.RangeProperty(topLevel, "Top Level");
					m_MaterialEditor.RangeProperty(topDepth, "Top Depth");

					EditorGUILayout.EndVertical ();
					GUILayout.Space (10);
					m_MaterialEditor.TexturePropertySingleLine (Titles.topMapText, topBase, topColor);
					m_MaterialEditor.RangeProperty(topBlendAlbedo, "Top Albedo Multiply");
					m_MaterialEditor.TexturePropertySingleLine (Titles.topNormal1Text, topNorm, topNormPOW);
					m_MaterialEditor.ShaderProperty(topNormBlendChoice,"Normal Blend Channel");
					m_MaterialEditor.ShaderProperty(topDetailMultiplier,"Tile Normal Channel");
					m_MaterialEditor.RangeProperty(topBlendNorm, "Blend Top Normals");

					m_MaterialEditor.TextureScaleOffsetProperty (topBase);
					GUILayout.Space (10);


					//===============================================================================================
					EditorGUILayout.BeginVertical ("Button");/////////////////////////////////////////////////// TOP MASK
					GUILayout.Label (Titles.topMapMaskText, EditorStyles.boldLabel);
					// show/hide block
					if (true) {
						Color col = GUI.color;
						GUI.color = new Color (1.0f, 0.0f, 0.0f, 1f);
						Rect rect = GUILayoutUtility.GetLastRect ();
						rect.x += EditorGUIUtility.currentViewWidth - 57;
						EditorGUI.BeginChangeCheck ();
						float nval = EditorGUI.Foldout (rect, _Mask1Shown.floatValue == 1, "") ? 1 : 0;
						if (EditorGUI.EndChangeCheck ()) {
							_Mask1Shown.floatValue = nval;
						}
						GUI.color = col;
					}
					if (_Mask1Shown.floatValue == 1) {

						m_MaterialEditor.ShaderProperty(topMaskChoice,"Top Mask Channel 1");
						m_MaterialEditor.ShaderProperty(showTopMask,"Visualize Mask");
						m_MaterialEditor.ShaderProperty(topMaskDepth,"Mask Depth");
						m_MaterialEditor.ShaderProperty(topMaskIntensityDepth,"Intensity Depth");
						m_MaterialEditor.ShaderProperty(topMaskTiling,"Mask 1 Tiling");
						GUILayout.Space (10);

						m_MaterialEditor.ShaderProperty(topMaskChoice2,"Top Mask Channel 2");
						m_MaterialEditor.ShaderProperty(topMaskTiling2,"Mask 2 Tiling");
						m_MaterialEditor.ShaderProperty(topMaskBlendStyle,"Blend Style");
						m_MaterialEditor.ShaderProperty(topMask2BlendAmount,"Blend Amount");
						m_MaterialEditor.ShaderProperty(topMaskDepth2,"2nd Mask Depth");
						m_MaterialEditor.ShaderProperty(topMaskIntensityDepth2,"2nd Intensity Depth");
						GUILayout.Space (10);
						
					}
					EditorGUILayout.EndVertical ();///////////////////////////////////////////////////////////////// END TOP MASK
					
				}
				EditorGUILayout.EndVertical ();/////////////////////////////////////////////////////// END TOP
				//===============================================================================================

				EditorGUILayout.Space ();
				

				//===============================================================================================
				EditorGUILayout.BeginVertical ("Button");////////////////////////////////////////////////// SNOW
				GUI.backgroundColor = bCol;
				GUILayout.Label (Titles.snowMapsText, EditorStyles.boldLabel);
				// show/hide block
				if (true) {
					Color col = GUI.color;
					GUI.color = new Color (1.0f, 1.0f, 0f, 1f);
					Rect rect = GUILayoutUtility.GetLastRect ();
					rect.x += EditorGUIUtility.currentViewWidth - 47;
					
					EditorGUI.BeginChangeCheck ();
					float nval = EditorGUI.Foldout (rect, _SnowShown.floatValue == 1, "") ? 1 : 0;
					//float nval =EditorGUILayout.ToggleLeft (Titles.snowMapsText, _SnowShown.floatValue == 1, EditorStyles.boldLabel) ? 1 : 0;
					if (EditorGUI.EndChangeCheck ()) {
						_SnowShown.floatValue = nval;
					}
					
					GUI.color = col;
				}
				if (_SnowShown.floatValue == 1) {

					//====================================================================Actual properties to dislay
					EditorGUILayout.BeginVertical ("box");
					m_MaterialEditor.ShaderProperty(snowDisplace,"Vertex Snow Displacement?");
					m_MaterialEditor.ShaderProperty(snowDirection,"Snow direction");
					m_MaterialEditor.RangeProperty(snowGlossAmount, "Smoothness");
					m_MaterialEditor.RangeProperty(snowLevel, "Snow Level");
					m_MaterialEditor.RangeProperty(snowDepth, "Snow Depth");
					EditorGUILayout.EndVertical ();
					GUILayout.Space (10);
					m_MaterialEditor.TexturePropertySingleLine (Titles.snowMapText, snowBase, snowColor);
					m_MaterialEditor.TexturePropertySingleLine (Titles.snowNormal1Text, snowNorm, snowNormPOW);
					m_MaterialEditor.TexturePropertySingleLine (Titles.snowNormal2Text, snowNorm2);
					m_MaterialEditor.RangeProperty(snowBlendNorm, "Blend Normals");
					m_MaterialEditor.TexturePropertySingleLine (Titles.snowspecMapText, snowGlossMap, snowGlossTiling);
					m_MaterialEditor.TextureScaleOffsetProperty (snowBase);
					GUILayout.Space (10);


					
					//===============================================================================================
					EditorGUILayout.BeginVertical ("Button");/////////////////////////////////////////////////// SNOW MASK
					GUILayout.Label (Titles.snowMapMaskText, EditorStyles.boldLabel);
					// show/hide block
					if (true) {
						Color col = GUI.color;
						GUI.color = new Color (1.0f, 0.0f, 0.0f, 1f);
						Rect rect = GUILayoutUtility.GetLastRect ();
						rect.x += EditorGUIUtility.currentViewWidth - 57;
						EditorGUI.BeginChangeCheck ();
						float nval = EditorGUI.Foldout (rect, _SnowMaskShown.floatValue == 1, "") ? 1 : 0;
						if (EditorGUI.EndChangeCheck ()) {
							_SnowMaskShown.floatValue = nval;
						}
						GUI.color = col;
					}
					if (_SnowMaskShown.floatValue == 1) {
						//====================================================== Now show parameters
						m_MaterialEditor.ShaderProperty(snowMaskChoice,"Snow Mask Channel 1");
						m_MaterialEditor.ShaderProperty(showSnowMask,"Visualize Snow Mask");
						m_MaterialEditor.ShaderProperty(snowMaskDepth,"Main Mask Depth");
						m_MaterialEditor.ShaderProperty(snowMaskIntensityDepth,"Intensity Depth");
						m_MaterialEditor.ShaderProperty(snowMaskTiling,"1st Mask Tiling");
						GUILayout.Space (10);

						m_MaterialEditor.ShaderProperty(snowMaskChoice2,"Snow Mask Channel 2");
						m_MaterialEditor.ShaderProperty(snowMaskTiling2,"2nd Mask Tiling");
						m_MaterialEditor.ShaderProperty(snowMaskBlendStyle,"Blend Style");
						m_MaterialEditor.ShaderProperty(snowMask2BlendAmount,"Blend Amount");
						m_MaterialEditor.ShaderProperty(snowMaskDepth2,"2nd Mask Depth");
						m_MaterialEditor.ShaderProperty(snowMaskIntensityDepth2,"2nd Intensity Depth");
						GUILayout.Space (10);
					}
					EditorGUILayout.EndVertical ();///////////////////////////////////////////////////////////////// END SNOW MASK
					
				}
				EditorGUILayout.EndVertical ();/////////////////////////////////////////////////////// END SNOW
				//===============================================================================================
				
				
			}///END BeginChangeCheck
			
			if (EditorGUI.EndChangeCheck ()) {
				//
				
			}
		}//end ShaderPropertiesGUI



	}//end class
		
}//end namespace
