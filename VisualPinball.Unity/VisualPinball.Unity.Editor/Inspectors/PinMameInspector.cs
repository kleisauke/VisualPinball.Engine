using UnityEditor;
using UnityEngine;
using VisualPinball.Unity.PinMame;

namespace VisualPinball.Unity.Editor.Inspectors
{
	[CustomEditor(typeof(PinMameBehavior))]
	[CanEditMultipleObjects]
	public class PinMameInspector : UnityEditor.Editor
	{
		private PinMameBehavior _pinMameBehavior;

		private void OnEnable()
		{
			_pinMameBehavior = (PinMameBehavior) target;
			_pinMameBehavior.Init();
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Start Game")) {
				_pinMameBehavior.StartGame();
			}

			if (GUILayout.Button("Stop Game")) {
				_pinMameBehavior.PinMame.StopGame();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}
