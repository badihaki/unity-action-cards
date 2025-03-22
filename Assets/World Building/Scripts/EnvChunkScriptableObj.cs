#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "envChunk_", menuName = "WorldGen/Create new Environment Chunk")]
public class EnvChunkScriptableObj : ScriptableObject
{
	[field: Header("Positioning Options"), SerializeField]
	public bool isBorder { get; private set; }
	[HideInInspector]
	public bool isCorner { get; private set; }
	[HideInInspector]
	public bool north { get; private set; }
	[HideInInspector]
	public bool south { get; private set; }
	[HideInInspector]
	public bool east { get; private set; }
	[HideInInspector]
	public bool west { get; private set; }

	[field: SerializeField, Header("Env building")]
	public GameObject chunkGameObj { get; private set; }
	public List<GameObject> buildingTemplates { get; private set; }
	public List<GameObject> floraTemplates { get; private set; }
	public List<NonPlayerCharacter> regularNPCs { get; private set; }
	public List<NonPlayerCharacter> specialNPCs { get; private set; }
	public List<NonPlayerCharacter> enemies { get; private set; }
	public List<NonPlayerCharacter> specialEnemies { get; private set; }


	// util class
#if UNITY_EDITOR
	[CustomEditor(typeof(EnvChunkScriptableObj))]
	public class EnvChunkEditorExtension : Editor
	{
		// editor ext here
		public override void OnInspectorGUI()
		{
			EnvChunkScriptableObj script = (EnvChunkScriptableObj)target;

			script.isBorder = EditorGUILayout.Toggle("Is a Border?", script.isBorder);

			if (script.isBorder)
			{
				//DrawPropertiesExcluding(serializedObject, "isCorner");
				script.isCorner = false;
				DrawBorderToggles(script);
			}
			else if (script.isBorder == false)
			{
				script.isCorner = EditorGUILayout.Toggle("Is Corner?", script.isCorner);
				if (script.isCorner)
				{
					DrawBorderToggles(script);
				}
			}
		}

		private static void DrawBorderToggles(EnvChunkScriptableObj script)
		{
			script.north = EditorGUILayout.Toggle("North Border", script.north);
			script.south= EditorGUILayout.Toggle("South Border", script.south);
			script.east= EditorGUILayout.Toggle("East Border", script.east);
			script.west= EditorGUILayout.Toggle("West Border", script.west);
		}
	}
#endif
}
