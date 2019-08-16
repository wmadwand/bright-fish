using UnityEditor;
using UnityEngine;

namespace BrightFish
{
	[CustomEditor(typeof(Level))]
	public class LevelEditor : Editor
	{
		private Level _target;

		private void OnEnable()
		{
			_target = (Level)target;
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);

			if (GUILayout.Button("Open level"))
			{
				GameProgress.Save(_target.ID);
			}

			DrawDefaultInspector();
		}
	}
}