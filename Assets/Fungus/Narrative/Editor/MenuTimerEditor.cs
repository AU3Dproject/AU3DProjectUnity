using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
	[CustomEditor (typeof(MenuTimer))]
	public class MenuTimerEditor : CommandEditor 
	{
		protected SerializedProperty durationProp;
		protected SerializedProperty targetBlockProp;

		protected virtual void OnEnable()
		{
			if (NullTargetCheck()) // Check for an orphaned editor instance
				return;

			durationProp = serializedObject.FindProperty("duration");
			targetBlockProp = serializedObject.FindProperty("targetBlock");
		}
		
		public override void DrawCommandGUI()
		{
			Flowchart flowchart = FlowchartWindow.GetFlowchart();
			if (flowchart == null)
			{
				return;
			}
			
			serializedObject.Update();
			
			EditorGUILayout.PropertyField(durationProp);
			
			BlockEditor.BlockField(targetBlockProp,
			                             new GUIContent("Target Block", "Block to call when timer expires"), 
			                             new GUIContent("<None>"), 
			                             flowchart);
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}
