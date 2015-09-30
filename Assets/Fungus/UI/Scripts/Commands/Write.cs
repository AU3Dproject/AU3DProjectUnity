using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Serialization;

namespace Fungus
{
	[CommandInfo("UI", 
	             "Write", 
	             "Writes content to a UI Text or Text Mesh object.")]

	[AddComponentMenu("")]
	public class Write : Command, ILocalizable
	{
		[Tooltip("Text object to set text on. Text, Input Field and Text Mesh objects are supported.")]
		public GameObject textObject;

		[Tooltip("String value to assign to the text object")]
		public StringData text;

		[Tooltip("Notes about this story text for other authors, localization, etc.")]
		public string description;

		[Tooltip("Clear existing text before writing new text")]
		public bool clearText = true;

		[Tooltip("Wait until this command finishes before executing the next command")]
		public bool waitUntilFinished = true;

		public enum TextColor
		{
			Default,
			SetVisible,
			SetAlpha,
			SetColor
		}

		public TextColor textColor = TextColor.Default;

		public FloatData setAlpha = new FloatData(1f);

		public ColorData setColor = new ColorData(Color.white);

		public override void OnEnter()
		{
			if (textObject == null)
			{
				Continue();
				return;
			}
		
			Writer writer = FindWriter(textObject);
			if (writer == null)
			{
				Continue();
				return;
			}

			switch (textColor)
			{
			case TextColor.SetAlpha:
				writer.SetTextAlpha(setAlpha);
				break;
			case TextColor.SetColor:
				writer.SetTextColor(setColor);
				break;
			case TextColor.SetVisible:
				writer.SetTextAlpha(1f);
				break;
			}

			Flowchart flowchart = GetFlowchart();
			string newText = flowchart.SubstituteVariables(text.Value);

			if (!waitUntilFinished)
			{
				writer.Write(newText, clearText, false, null, null);
				Continue();
			}
			else
			{
				writer.Write(newText, clearText, false, null,
				             () => { Continue (); }
				);
			}
		}

		public override string GetSummary()
		{
			if (textObject != null)
			{
				return textObject.name + " : " + text.Value;
			}

			return "Error: No text object selected";
		}

		public override Color GetButtonColor()
		{
			return new Color32(235, 191, 217, 255);
		}

		protected Writer FindWriter(GameObject textObject)
		{
			Writer writer = textObject.GetComponent<Writer>();
			if (writer == null)
			{
				writer = textObject.AddComponent<Writer>() as Writer;
			}

			return writer;
		}

		//
		// ILocalizable implementation
		//

		public virtual string GetStandardText()
		{
			return text;
		}

		public virtual void SetStandardText(string standardText)
		{
			text.Value = standardText;
		}

		public virtual string GetDescription()
		{
			return description;
		}
		
		public virtual string GetStringId()
		{
			// String id for Write commands is WRITE.<Localization Id>.<Command id>
			return "WRITE." + GetFlowchartLocalizationId() + "." + itemId;
		}
	}

}