using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class TalkEventScript : MonoBehaviour {

	[SerializeField]
	[Tooltip("実行のタイプ\nattach:対象オブジェクトに話しかけた際に実行\nauto:条件下で一度だけ自動実行\nparallel:条件下で永遠に自動実行")]
	public ExecuteType executeType = ExecuteType.attach;
	[Tooltip("このイベントに関するただの説明")]
	public string description;

	//テキストウィンドウを操作するためのGameObject
	private GameObject textWindow = null;
	//入力待ち判定
	private bool isWait = false;
	//選択待ち判定
	private bool isSelect = false;
	//実行可能状態
	public bool isExecute = false;
	//スクリプトを行単位で格納しておくリスト
	private List<string> scriptLines = new List<string>();
	//ラベルをラベル名と行数とで格納しておくリスト
	private Dictionary<string, int> scriptLabel = new Dictionary<string, int>();
	private int lineNum = 0;
	private Writer writer = null;
	private TalkEventSelectButton selectButtonManager = null;
	private BGMManager audioSourceBGM = null;
	private Function[] functions;
	//実行のタイプ
	public enum ExecuteType {
		attach, auto, parallel,
	}

	[TextArea(10, 10000)]
	[Tooltip(
		"イベント本文です。\n" +
		"command\n" +
		"[p] : 入力待ち\n" +
		"[l] : 改ページ\n" +
		"[add_select ボタン名 ジャンプラベル] : 選択肢の追加\n" +
		"[open_select] : 選択肢の表示\n"
	)]
	public string eventScript = "";
	private string eventScriptTooltip = "";

	/*　Start
	 * 　（１）文字列を一行ずつ読み込むためのStringReaderを初期化
	 * 　（２）話す時に表示するTextWindowとなるPrefabをResourcesフォルダからロード
	 */
	void Start() {
		//canvas = Instantiate((GameObject)Resources.Load("TalkEvent/TalkEventCanvas"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		textWindow = GameObject.Find("/Canvases").transform.Find("TalkEventCanvas").gameObject;
		writer = textWindow.transform.GetChild(0).GetComponent<Writer>();
		selectButtonManager = textWindow.transform.GetChild(1).GetComponent<TalkEventSelectButton>();
		audioSourceBGM = GameObject.Find("/Manager").GetComponent<Manager>().BGMManager;
		textWindow.SetActive(false);
		loadEventScript();

		Function[] tmp = {
			new AddSelectFunction(this),new OpenSelectFunction(this),new PutWaitFunction(this),new LinesNewFunction(this),new GoToFunction(this),new BGMFunction(this)
		};

		functions = tmp;
	}

	/* Update
	 * 　（１）もしも実行可能状態であれば
	 * 　（２）スクリプトを実行する
	 */
	void Update() {

		//実行タイプがAttach以外なら常に実行可能状態にする
		if (executeType != ExecuteType.attach) {
			isExecute = true;
		}

		//実行可能状態にあれば常に実行する
		if (isExecute) {
			Execute();
		}

	}

	/* 実行メソッド
	 * 　（１）テキスト表示ウィンドウが表示されていない時、Instantiate（生成）する
	 * 　（２）決定ボタンが押された時の挙動として、テキスト表示中であればすべての文字を表示し、それ以外で入力待ち中ならば入力待ちを解除する
	 * 　（３）入力待ちになるまで、かつ、スクリプトを読み終えるまで（４）をループする。
	 * 　（４）一行ずつ読み込み、関数なら関数処理の実行、入力待ちなら入力待ち状態に移行、改ページならテキストの削除、それ以外ならテキスト表示を行う。
	 * 　（５）もし、入力待ちでなく、スクリプトを読み終え、テキストが表示中でないならば、テキスト表示ウィンドウを削除し、stringReaderの初期化、実行可能状態を解除して終了
	 */
	void Execute() {

		//TalkEvent中の入力操作
		if (Input.GetButtonDown("Submit") && !isSelect) {
			if (writer.isTextActive) {
				writer.allVisible();
			} else if (isWait) {
				isWait = false;
			}
		}

		//スクリプト読み込み・実行
		while (!isWait && !isSelect && lineNum < scriptLines.Count) {
			string line = scriptLines[lineNum];
			if (!functionExecute(line) && line != "") {
				if (!textWindow.activeInHierarchy) {
					textWindow.SetActive(true);
				}
				writer.text += (line + "\n");
				writer.isTextActive = true;
			}
			lineNum++;
		}

		if (isSelect && !writer.isTextActive) {
			string answer = selectButtonManager.getAnswer();
			if (answer != "") {
				goLabel(answer);
				isSelect = false;
				writer.text = "";
				selectButtonManager.closeButtons();
			}
		}

		//スクリプト終了処理
		if (!isWait && scriptLines.Count <= lineNum && !writer.isTextActive && !isSelect) {
			writer.removeText();
			writer.text = "";
			textWindow.SetActive(false);
			lineNum = 0;
			isExecute = false;
			isWait = false;
		}
	}

	private void loadEventScript() {
		StringReader stringReader = new StringReader(eventScript);
		int i = 0;
		while (stringReader.Peek() > -1) {
			string line = stringReader.ReadLine();
			if (Regex.IsMatch(line, @"^[a-zA-z]+?\w*:$")) {
				scriptLabel.Add(line.Substring(0, line.Length - 1), i);
				line = "";
			}
			scriptLines.Add(line);
			i++;
		}
		stringReader.Close();
	}

	public bool functionExecute(string target) {
		foreach (Function f in functions) {
			if (f.isFunction(target)) {
				f.Execute();
				return true;
			}
		}
		return false;
	}

	/* 入力待ち設定
	 * 　（１）外部から入力待ちを設定可能にする（使うかどうかはわからぬ）
	 */
	public void setWait(bool wait) {
		isWait = wait;
	}

	public void setSelectMode(bool select) {
		isSelect = select;
	}

	public void activeWindow(bool visible) {
		textWindow.SetActive(visible);
	}

	public void goLabel(string label) {
		int num = 0;
		if (scriptLabel.TryGetValue(label, out num)) {
			lineNum = num;
		}
	}

	public class Function{
		protected string regexString = "";
		protected TalkEventScript eventScript;
		public string description = "";
		public virtual bool isFunction(string target) {
			return Regex.IsMatch(target,regexString);
		}
		public virtual string subBracket(string target) {
			return target.Substring(1, target.Length - 2);
		}
		public virtual void Execute() {

		}
		//argument to variable ：　引数が変数指定されていたら変数を返却。変数指定は<var=変数名>。もしくは<var=@ID>でのID指定が可能。
		protected string a2v(string arg) {
			if (GameObject.FindWithTag("variable").GetComponent<TalkEventValiable>().isVariable(arg)) {
				return GameObject.FindWithTag("variable").GetComponent<TalkEventValiable>().getVariable_Command(arg).value;
			} else {
				return arg;
			}
		}
	}

	public class AddSelectFunction : Function {
		private string[] arguments = new string[2];
		public AddSelectFunction(TalkEventScript script){
			regexString = @"^\[add_select\s+.+\s+.+\]$";
			eventScript = script;
		}
		public override bool isFunction(string target) {
			if (base.isFunction(target)) {
				string[] tmp = subBracket(target).Split(' ');
				arguments[0] = a2v(tmp[1]);
				arguments[1] = a2v(tmp[2]);
				return true;
			}
			return false;
		}
		public override void Execute(){
			eventScript.selectButtonManager.addButton(arguments[0],arguments[1]);
		}
	}

	public class OpenSelectFunction : Function {
		private string[] arguments = new string[2];
		public OpenSelectFunction(TalkEventScript script) {
			regexString = @"^\[open_select\]$";
			eventScript = script;
		}
		public override void Execute() {
			eventScript.selectButtonManager.openButtons();
			eventScript.setSelectMode(true);
		}
	}

	public class PutWaitFunction : Function {
		public PutWaitFunction(TalkEventScript script) {
			regexString = @"^\[p\]$";
			eventScript = script;
		}
		public override void Execute() {
			eventScript.setWait(true);
		}
	}

	public class LinesNewFunction : Function {
		public LinesNewFunction(TalkEventScript script) {
			eventScript = script;
			regexString = @"^\[l\]$";
		}
		public override void Execute() {
			eventScript.writer.text = "";
			eventScript.writer.removeText();
		}
	}

	public class GoToFunction : Function {
		private string argument = "";
		public GoToFunction(TalkEventScript script) {
			eventScript = script;
			regexString = @"^\[goto\s.+\]$";
		}
		public override bool isFunction(string target) {
			if (base.isFunction(target)) {
				string[] tmp = subBracket(target).Split(' ');
				argument = a2v(tmp[1]);
				return true;
			}
			return false;
		}
		public override void Execute() {
			eventScript.goLabel(argument);
		}
	}

	public class BGMFunction : Function {
		string[] arguments = new string[2];
		public BGMFunction(TalkEventScript script) {
			eventScript = script;
			regexString = @"^\[bgm\s+.+\s+.+\]$";
		}
		public override bool isFunction(string target) {
			if (base.isFunction(target)) {
				string[] tmp = subBracket(target).Split(' ');
				arguments[0] = a2v(tmp[1]);
				arguments[1] = a2v(tmp[2]);
				return true;
			}
			return false;
		}
		public override void Execute() {
			switch (arguments[0]) {
				case "play":
					eventScript.audioSourceBGM.Play(arguments[1]);
					break;
				case "stop":
					eventScript.audioSourceBGM.Stop(int.Parse(arguments[1]));
					break;
				case "volume":
					eventScript.audioSourceBGM.setVolume(int.Parse(arguments[1]));
					break;
				case "pitch":
					eventScript.audioSourceBGM.setPitch(int.Parse(arguments[1]));
					break;
				case "pan":
					eventScript.audioSourceBGM.setPan(int.Parse(arguments[1]));
					break;
				default:
					Debug.Log("未設定の構文 : " + arguments[0]);
					break;
			}
		}
	}

}