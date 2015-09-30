using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rotorz.ReorderableList;

namespace Fungus
{
	
	[CustomEditor (typeof(Say))]
	public class SayEditor : CommandEditor
	{
		static public bool showTagHelp;
		public Texture2D blackTex;
		
		static public void DrawTagHelpLabel()
		{
			string tagsText = TextTagParser.GetTagHelp();

			if (CustomTag.activeCustomTags.Count > 0)
			{
				tagsText += "\n\n\t-------- CUSTOM TAGS --------";
				List<Transform> activeCustomTagGroup = new List<Transform>();
				foreach (CustomTag ct in CustomTag.activeCustomTags)
				{
					if(ct.transform.parent != null)
					{
						if (!activeCustomTagGroup.Contains(ct.transform.parent.transform))
						{
							activeCustomTagGroup.Add(ct.transform.parent.transform);
						}
					}
					else
					{
						activeCustomTagGroup.Add(ct.transform);
					}
				}
				foreach(Transform parent in activeCustomTagGroup)
				{
					string tagName = parent.name;
					string tagStartSymbol = "";
					string tagEndSymbol = "";
					CustomTag parentTag = parent.GetComponent<CustomTag>();
					if (parentTag != null)
					{
						tagName = parentTag.name;
						tagStartSymbol = parentTag.tagStartSymbol;
						tagEndSymbol = parentTag.tagEndSymbol;
					}
					tagsText += "\n\n\t" + tagStartSymbol + " " + tagName + " " + tagEndSymbol;
					foreach(Transform child in parent)
					{
						tagName = child.name;
						tagStartSymbol = "";
						tagEndSymbol = "";
						CustomTag childTag = child.GetComponent<CustomTag>();
						if (childTag != null)
						{
							tagName = childTag.name;
							tagStartSymbol = childTag.tagStartSymbol;
							tagEndSymbol = childTag.tagEndSymbol;
						}
							tagsText += "\n\t      " + tagStartSymbol + " " + tagName + " " + tagEndSymbol;
					}
				}
			}
			tagsText += "\n";
			float pixelHeight = EditorStyles.miniLabel.CalcHeight(new GUIContent(tagsText), EditorGUIUtility.currentViewWidth);
			EditorGUILayout.SelectableLabel(tagsText, GUI.skin.GetStyle("HelpBox"), GUILayout.MinHeight(pixelHeight));
		}
		
		protected SerializedProperty characterProp;
		protected SerializedProperty portraitProp;
		protected SerializedProperty storyTextProp;
		protected SerializedProperty descriptionProp;
		protected SerializedProperty voiceOverClipProp;
		protected SerializedProperty showAlwaysProp;
		protected SerializedProperty showCountProp;
		protected SerializedProperty extendPreviousProp;
		protected SerializedProperty fadeWhenDoneProp;
		protected SerializedProperty waitForClickProp;
		protected SerializedProperty setSayDialogProp;

		protected virtual void OnEnable()
		{
			if (NullTargetCheck()) // Check for an orphaned editor instance
				return;

			characterProp = serializedObject.FindProperty("character");
			portraitProp = serializedObject.FindProperty("portrait");
			storyTextProp = serializedObject.FindProperty("storyText");
			descriptionProp = serializedObject.FindProperty("description");
			voiceOverClipProp = serializedObject.FindProperty("voiceOverClip");
			showAlwaysProp = serializedObject.FindProperty("showAlways");
			showCountProp = serializedObject.FindProperty("showCount");
			extendPreviousProp = serializedObject.FindProperty("extendPrevious");
			fadeWhenDoneProp = serializedObject.FindProperty("fadeWhenDone");
			waitForClickProp = serializedObject.FindProperty("waitForClick");
			setSayDialogProp = serializedObject.FindProperty("setSayDialog");

			if (blackTex == null)
			{
				blackTex = CustomGUI.CreateBlackTexture();
			}
		}
		
		protected virtual void OnDisable()
		{
			DestroyImmediate(blackTex);
		}
		
		public override void DrawCommandGUI() 
		{
			serializedObject.Update();

			bool showPortraits = false;

			CommandEditor.ObjectField<Character>(characterProp, 
			                                     new GUIContent("Character", "Character that is speaking"), 
			                                     new GUIContent("<None>"),
			                                     Character.activeCharacters);

			Say t = target as Say;

			// Only show portrait selection if...
			if (t.character != null &&              // Character is selected
			    t.character.portraits != null &&    // Character has a portraits field
			    t.character.portraits.Count > 0 )   // Selected Character has at least 1 portrait
			{
				showPortraits = true;    
			}

			if (showPortraits) 
			{
				CommandEditor.ObjectField<Sprite>(portraitProp, 
			                                	     new GUIContent("Portrait", "Portrait representing speaking character"), 
			                                    	 new GUIContent("<None>"),
			                                     	t.character.portraits);
			}
			else
			{
				if (!t.extendPrevious)
				{
					t.portrait = null;
				}
			}
			
			EditorGUILayout.PropertyField(storyTextProp);

			EditorGUILayout.PropertyField(descriptionProp);

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.PropertyField(extendPreviousProp);

			GUILayout.FlexibleSpace();

			if (GUILayout.Button(new GUIContent("Tag Help", "View available tags"), new GUIStyle(EditorStyles.miniButton)))
			{
				showTagHelp = !showTagHelp;
			}
			EditorGUILayout.EndHorizontal();
			
			if (showTagHelp)
			{
				DrawTagHelpLabel();
			}
			
			EditorGUILayout.Separator();
			
			EditorGUILayout.PropertyField(voiceOverClipProp, 
			                              new GUIContent("Voice Over Clip", "Voice over audio to play when the text is displayed"));

			EditorGUILayout.PropertyField(showAlwaysProp);
			
			if (showAlwaysProp.boolValue == false)
			{
				EditorGUILayout.PropertyField(showCountProp);
			}

			GUIStyle centeredLabel = new GUIStyle(EditorStyles.label);
			centeredLabel.alignment = TextAnchor.MiddleCenter;
			GUIStyle leftButton = new GUIStyle(EditorStyles.miniButtonLeft);
			leftButton.fontSize = 10;
			leftButton.font = EditorStyles.toolbarButton.font;
			GUIStyle rightButton = new GUIStyle(EditorStyles.miniButtonRight);
			rightButton.fontSize = 10;
			rightButton.font = EditorStyles.toolbarButton.font;

			EditorGUILayout.PropertyField(fadeWhenDoneProp);
			EditorGUILayout.PropertyField(waitForClickProp);
			EditorGUILayout.PropertyField(setSayDialogProp);

			if (showPortraits && t.portrait != null)
			{
				Texture2D characterTexture = t.portrait.texture;
				float aspect = (float)characterTexture.width / (float)characterTexture.height;
				Rect previewRect = GUILayoutUtility.GetAspectRect(aspect, GUILayout.Width(100), GUILayout.ExpandWidth(true));
				if (characterTexture != null)
				{
					GUI.DrawTexture(previewRect,characterTexture,ScaleMode.ScaleToFit,true,aspect);
				}
			}
			
			serializedObject.ApplyModifiedProperties();
		}
	}
	
}