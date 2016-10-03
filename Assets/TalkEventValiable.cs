using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

[SerializeField]
public class TalkEventValiable : MonoBehaviour,ISerializationCallbackReceiver{
	
	[SerializeField]
	[Tooltip("変数。イベント時などに手軽に扱うことができる。")]
	public Variable[] variables = {};

	public Variable getVariable(int id) {
		return variables[id];
	}
	
	public Variable getVariable(string name) {
		foreach (Variable v in variables) {
			if (v.variableName == name)
				return v;
		}
		return null;
	}

	public void OnAfterDeserialize() {
		for (int i = 0; i < variables.Length; i++) {
			variables[i].id = i;

		}
	}

	public void OnBeforeSerialize() {
		
	}

	[Serializable]
	public class Variable {
		[SerializeField]
		[Tooltip("一意的なID。自動で定まる。")]
		public int id;
		[SerializeField]
		[Tooltip("変数名。重複可能だが、名前から変数を検索する際、\nIDの小さいもののみ返却される。\n変数名に()とか[]とか\"\"とかプログラムで使いそうな文字列を使うとバグるときがあるかもしれない。")]
		public string variableName = "";
        [SerializeField]
		[Tooltip("変数の説明。メモ書きにでも使ってくらさい。特に意味は（ないです）")]
		public string description = "";
		[SerializeField]
		[Tooltip("変数の値。Int,Float,Bool,Stringの型の値を保持できる。\n仕様として値の保持はstring型だが、各型へキャストして返却するメソッドがある。")]
		public string value = "";

		public Variable(string value) {
			this.value = value;
		}


		public bool isInt(string value) {
			int o;
			if (int.TryParse(value, out o)) {
				return true;
			} else {
				Debug.Log("値[" + value + "]はInt型じゃ（ないです）");
				return false;
			}
		}
		public float? isFloat() {
			float o;
			if (float.TryParse(value, out o)) {
				return o;
			} else {
				Debug.Log("変数値[" + value + "]はFloat型じゃ（ないです）");
				return null;
			}
		}
		public bool isFloat(string value) {
			float o;
			if (float.TryParse(value, out o)) {
				return true;
			} else {
				Debug.Log("値[" + value + "]はFloat型じゃ（ないです）");
				return false;
			}
		}
		public bool? isBool() {
			bool o;
			if (bool.TryParse(value, out o)) {
				return o;
			} else {
				Debug.Log("変数値[" + value + "]はBool型じゃ（ないです）");
				return null;
			}
		}
		public bool isBool(string value) {
			bool o;
			if (bool.TryParse(value, out o)) {
				return true;
			} else {
				Debug.Log("値[" + value + "]はBool型じゃ（ないです）");
				return false;
			}
		}

		public void setInt(string value) {
			setString(isInt(value) ? value:null);
		}
		public void setInt(int value) {
			setString(value.ToString());
		}
		public void setFloat(string value) {
			setString(isFloat(value) ? value : null);
		}
		public void setFloat(float value) {
			setString(value.ToString());
		}
		public void setBool(string value) {
			setString(isBool(value) ? value : null);
		}
		public void setBool(bool value) {
			setString(value.ToString());
		}

		public void setString(string value) {
			if(value != null) this.value = value;
		}

		public int? getInt() {
			int o;
			if (int.TryParse(value, out o)) {
				return o;
			} else {
				Debug.Log("変数値[" + value + "]はInt型じゃ（ないです）");
				return null;
			}
		}
		public float getFloat() {
			float o;
			if (float.TryParse(value, out o)) {
				return o;
			} else {
				throw new System.ArgumentException("変数:[" + value + "]はFloat型じゃ（ないです）");
			}
		}
		public bool getBool() {
			bool o;
			if (bool.TryParse(value, out o)) {
				return o;
			} else {
				throw new System.ArgumentException("変数:[" + value + "]はBool型じゃ（ないです）");
			}
		}
		public string getString() {
			return value;
		}

		/*public int addInt(string value,bool isSet=false) {
			if (isInt() && isInt(value)) {
				if (isSet)
			}
		}*/

	}

}
