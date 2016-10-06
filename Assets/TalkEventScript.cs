using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TalkEventScript : MonoBehaviour {

	[SerializeField]
	[Tooltip("実行のタイプ\nattach:対象オブジェクトに話しかけた際に実行\nauto:条件下で一度だけ自動実行\nparallel:条件下で永遠に自動実行")]
	public ExecuteType executeType = ExecuteType.attach;
	[Tooltip("このイベントに関するただの説明")]
	public string description;

	//テキストウィンドウのGameObjectを入れておく
	private GameObject textWindow;
	//イベントスクリプトを一行ずつ読むReader
	private System.IO.StringReader stringReader;
	//テキストウィンドウを操作するためのGameObject
	GameObject canvas = null;
	//入力待ち判定
	private bool isWait = false;
	//選択待ち判定
	private bool isSelect = false;
	//実行可能状態
	public bool isExecute = false;
	//未実装（スクリプトを行単位で格納しておくリスト）
	private List<string> scriptLines = new List<string>();
	//実行のタイプ
	public enum ExecuteType {
		attach, auto, parallel,
	}

	[TextArea(10, 10000)]
	[Tooltip(
		"イベント本文です。\n" +
		"command\n" +
		"[p] : 入力待ち\n" +
		"[l] : 改ページ"
	)]
	public string eventScript = "";

	/*　Start
	 * 　（１）文字列を一行ずつ読み込むためのStringReaderを初期化
	 * 　（２）話す時に表示するTextWindowとなるPrefabをResourcesフォルダからロード
	 */
	void Start() {
		stringReader = new System.IO.StringReader(eventScript);
		textWindow = (GameObject)Resources.Load("TalkEvent/TalkEventCanvas");
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

		//textWindowの生成処理
		if (canvas == null) {
			canvas = Instantiate(textWindow, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		}

		//TalkEvent中の入力操作
		if (Input.GetButtonDown("Submit")) {
			if (canvas.transform.GetChild(0).GetComponent<Writer>().isTextActive) {
				canvas.transform.GetChild(0).GetComponent<Writer>().allVisible();
			}else if (isWait) {
				isWait = false;
			}
		}

		//スクリプト読み込み・実行
		while (isWait == false && stringReader.Peek() > -1) {
			string line = stringReader.ReadLine();
			if (isFunction(line)) {
				functionExecute();
			} else if (line == "[p]") {
				isWait = true;
			} else if (line == "[l]") {
				canvas.transform.GetChild(0).GetComponent<Writer>().text = "";
				canvas.transform.GetChild(0).GetComponent<Writer>().removeText();
			} else {
				canvas.transform.GetChild(0).GetComponent<Writer>().text += (line + "\n");
				canvas.transform.GetChild(0).GetComponent<Writer>().isTextActive = true;
			}
			
		}

		//スクリプト終了処理
		if (isWait == false && stringReader.Peek() <= -1 && canvas.transform.GetChild(0).GetComponent<Writer>().isTextActive == false) {
			Destroy(canvas);
			stringReader = new System.IO.StringReader(eventScript);
			isExecute = false;
		}
	}

	void functionExecute() {

	}

	bool isFunction(string target) {
		return false;
	}

	/* 入力待ち設定
	 * 　（１）外部から入力待ちを設定可能にする（使うかどうかはわからぬ）
	 */
	public void setWait(bool wait) {
		isWait = wait;
	}



	public class Function{
		protected Regex regex;
		protected TalkEventScript eventScript;
		public virtual bool isFunction(string target) {
			return regex.IsMatch(target);
		}
		public virtual void Execute() {

		}
	}

	public class SelectFunction : Function {
		public SelectFunction(TalkEventScript script){
			regex = @"\[select.+\]";
			eventScript = script;
		}
		public override void Execute(){
			
		}
	}
}
/*

[select ここはどこ？ where]
[select BGMかけて！ bgm]
[select さよなら goodbye]

where:
秋田大学だよ。
[goto end]

bgm:
[bgm_start ac]
[goto end]

goodbye:
さよなら。

end:




[select 表示文字 ジャンプラベル]

select命令を検知し、
次の行がselectならば2行分、更にその次の行がselectなら3行分と、最大８回次の行を見に行きインクリメントする。

TalkEventSelectButtonにアクセスし検知した数分のselectをボタンとして作成
その時、ターゲット文字列の最初と最後の文字を削除し、残った部分をスペースでスプリットする。
２つめの文字を表示文字としてTalkEventSelectButtonへ渡す。
この時isSelectをTrueにし、ウェイト状態にする。
TalkEventSelectButtonからAnswerとして正の整数を取得できたならその整数回目に出てきたボタンの３つめの引数であるジャンプラベルを実行する。
つまりAnswer（解答）が出たらgoto命令を実行して終了

[goto ジャンプラベル]

ジャンプラベルへ飛ぶ
考えたんだけどやっぱり、文字列のリストとしてスクリプトを保管しておき、利用したほうが良いのではないか？
また、ラベルに関してはMapだかDictionaryだかをつかってラベル文字に対する行数という形で文字列と整数で保存すれば良いのでは？
そして文字列リストから手に入れた整数を使いindexOfでその行からスタートみたいにしたほうが良さそう
それなら行移動も簡単だし面倒なstringReaderも使わなくてすむよね。

*/