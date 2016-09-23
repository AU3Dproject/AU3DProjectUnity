using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class Writer : MonoBehaviour {

    [SerializeField]

	
	[TextArea(5,10)]
	[Tooltip("表示するテキスト")]
	public string text = "";

	[Tooltip("テキスト表示開始の合図(trueなら表示開始)")]
    public bool isTextActive = true;

	[Tooltip("テキストの表示速度[秒]（デフォルトは0.05秒）")]
    public float textVisibleTime = 0.05f;

	[Tooltip("効果音再生を除外する文字")]
	public char[] noSoundCharacters = {' ','　','\n'};

	//<wait>コマンドによるウェイト時間[秒]
	private float waitTime = 0.0f;
	//時間関係の処理を行うための時間
    private float time = 0.0f;
	//表示文字の位置を意味するインデックス
    private int i = 0;
	//検知したコマンドを格納する変数
    private string command;
	//文字表示の際の音声
	private AudioSource textSound;
	//実際に文字を表示してくれるコンポーネント（通常はこのgameObjectの子に存在するTextComponent）
    private Text textComponent;
	//コマンドの検出等を行うインスタンス
	private CommandFunction commandFunction;
	//コマンド処理を行うためのテキストのバッファ(一時保管庫)
	private string textBuff = "";
	//スキップ状態を表す変数
	private bool isSkip = false;


    /* Start
     * 　（１）TextComponentの取得
     * 　（２）AudioSourceComponentの取得
     * 　（３）コマンドファンクションのインスタンス化
     */
    void Start() {
        textComponent = transform.FindChild("Text").GetComponent<Text>();
		textSound = GetComponent<AudioSource>();
		commandFunction = new CommandFunction(this);
    }

    /* Update
     * 　（１）
     */
    void Update() {

		//各文字列変数の初期化
		string nextString = "";
		textBuff = "";

		//表示開始であれば処理開始
		if (isTextActive) {

			//時間の計測を行う
            time += Time.deltaTime;

			//文字表示時間に達した時、テキストの順次表示処理
            if (time > textVisibleTime) {


				do{
					//次の表示文字の抽出
					nextString = getStringSingle (text, i++);

					//文字列"<"を検出した時コマンド検出とするため、コマンドファンクションインスタンスを用いて種類の検出等を行う
					while (nextString == "<") {
						//コマンド文字列へ初期値の代入
						command = "<";
						//コマンド終了を示す">"が出てくるまでその間の文字列を抽出
						while (nextString != ">") {
							//1文字の抽出を実行
							nextString = getStringSingle (text, i++);
							//最低1文字はコマンドであるため最初の１回は必ずコマンドに抽出文字を挿入
							//最終的に">"を挿入してループを抜けることとなる
							command += nextString;
						}
						//検出したコマンドが開始コマンドなのか終了コマンドなのか判定し、
						//開始コマンドであればコマンドの登録　その後、登録されたコマンドをつけて文字が表示される
						//終了コマンドであればコマンドの解除　この後、コマンドは付加されずに文字が表示される
						commandFunction.checkFunctionStartEnd (command);

						//再度次の表示文字の抽出（コマンド検出した場合現在の表示する文字は">"になっているため）
						nextString = getStringSingle (text, i++);

					}

					//全文一括表示のためにここで一旦textBuffにコマンドをつけた文字を格納しておき、
					//もし全文一括表示中ならすべての文字を
					if(commandFunction.isCommand() == true){
						textBuff += commandFunction.addCommands(nextString);
					}else{
						textBuff += nextString;
					}

				}while((textVisibleTime <= 0 || isSkip) && i < text.Length);

				//コマンドファンクションインスタンスで何かのコマンドが登録されている場合、登録コマンドを文字に付加して文字を表示
				//コマンドが一つも登録されていなければ通常通り文字だけ表示
				/*if (commandFunction.isCommand () == true) {
					textComponent.text += commandFunction.addCommands (nextString);
				} else {
					textComponent.text += nextString;
				}*/
				textComponent.text += textBuff;

				//テキスト表示に伴う効果音の再生（）
				if (nextString != " " && nextString != "　" && nextString != "\n") {
					textSound.Play ();
				}

				//時間を検知時間分シフト
                time -= textVisibleTime;

				//参照位置が文字列の長さを超えていたら終了
				if (i >= text.Length) {
					isTextActive = false;
				}

            }
        } else {
			//値の初期化
            i = 0;
            time = 0.0f;
			isSkip = false;
        }

    } 

	/* 文字列から任意の１文字を文字列として抽出する
	 * 　（１）SubString関数を用いてindex文字から1文字抽出し、返却する
	 */
	private string getStringSingle(string target,int index) {
		return (target.Substring(index,1));
	}

	/* 文字列から任意の一文字を抽出する（未使用）
	 * 　（１）文字列をchar型配列とした場合、index番目の文字は以下の返却値の通りである。
	 */
	private char getCharacterSingle(string target,int index) {
		return (target[index]);
	}

	/* 表示待ち延長
	 * 　（１）計測時間を一回だけ引数second秒マイナスすることで次のI文字の表示を約second秒待つことができる
	 * 　（２）計測時間を-2秒から始めることで2秒のウェイトをかけてから文字表示を行うことができる。
	 */
	public void wait(float second) {
		if(textVisibleTime > 0.0f)time -= second;
	}

	/* 音声再生可能文字判定
	 * 　（１）音声再生をしない文字が含まれていたらfalse
	 * 　（２）そうでないならtrue
	 */
	private bool isSound(string target){
		foreach (char c in noSoundCharacters) {
			//target文字列にnoSoundCharactersが含まれているかどうか
			if (target.Contains (c.ToString ())) {
				return false;
			}
		}
		return true;
	}

	/* 全文字一括表示
	 * 　（１）スキップ状態に移行する（入力待ちに入ると解除）
	 */
	public void allVisible(){
		isSkip = true;
	}

	/* 文字表示速度の調整
	 * 　（１）textVisibleTimeを調整可能
	 * 　（２）0を入力することで一括表示のようなことも可能
	 * 　（３）表示を遅くしたい文字の一つ手前で実行する必要がある
	 */
	public void speedChange(float spd){
		textVisibleTime = spd;
		time = 0.0f;
	}


	public void removeText(){
		textComponent.text = "";
		textBuff = "";
	}

	public void removeWindow(){
		GameObject.Destroy (this.transform.parent.gameObject);
	}


	

	//文中に登場させることのできるコマンド（<b>など）に関するクラス
	class CommandFunction {
		private BoldCommand boldCommand;
		private ItalicCommand italicCommand;
		private SizeCommand sizeCommand;
		private ColorCommand colorCommand;
		private WaitCommand waitCommand;
		private SpeedCommand speedCommand;

		private Command[] commands;
		private List<Command> applyCommands = new List<Command>();

		private Writer writer;

		public CommandFunction(Writer writer) {
			boldCommand = new BoldCommand();
			italicCommand = new ItalicCommand();
			sizeCommand = new SizeCommand();
			colorCommand = new ColorCommand();
			waitCommand = new WaitCommand(writer);
			speedCommand = new SpeedCommand(writer);
			Command[] commands = { boldCommand, italicCommand, sizeCommand, colorCommand, waitCommand, speedCommand };
			this.commands = commands;
			this.writer = writer;
		}

		public string getStartCommand() {
			string retCommand = "";
			foreach (Command c in applyCommands) {
				retCommand += c.getStartCommand();
			}
			return retCommand;
		}

		public string getEndCommand() {
			string retCommand = "";
			foreach (Command c in applyCommands) {
				if (retCommand == "") retCommand = c.getEndCommand();
				else retCommand = c.getEndCommand() + retCommand;
			}
			return retCommand;
		}

		public string addCommands(string target) {
			return getStartCommand() + target + getEndCommand();
		}

		public void checkFunctionStartEnd(string target) {
			if (isFunctionEnd(target) == false) isFunctionStart(target);
		}

		//Unityのリッチテキストに標準装備のファンクションを検知
		public bool isFunctionStart(string target) {
			foreach (Command c in commands) {
				if (c.isStart(target) == true) {
					if(c.isStartOnly==false)applyCommands.Add(c);
					return true;
				}
			}
			return false;

		}

		//Unityのリッチテキストに標準装備のファンクションの終了を検知
		public bool isFunctionEnd(string target) {
			foreach (Command c in commands) {
				if (c.isEnd(target) == true) {
					if (c.isStartOnly == false) applyCommands.RemoveAt(applyCommands.Count-1);
					return true;
				}
			}
			return false;

		}

		public bool isCommand() {
			if (applyCommands.Count > 0) {
				return true;
			} else {
				return false;
			}
		}

		class Command {
			protected Regex startRegex;
			protected Regex endRegex;
			public string endCommand = null;
			public string startCommand = null;
			public bool isStartOnly = false;

			public virtual bool isStart(string target) {
				if (startRegex.Match(target).Success) {
					if (startCommand == "") startCommand = target;
					return true;
				} else {
					return false;
				}
			}
			public virtual bool isEnd(string target) {
				if (endRegex.Match(target).Success) {
					if (endCommand == "") endCommand = target;
					return true;
				} else {
					return false;
				}
			}
			public virtual string getStartCommand() {
				if (startCommand != null) return startCommand;
				else return "";
			}
			public virtual string getEndCommand() {
				if (endCommand != null) return endCommand;
				else return "";
			}
		}

		class BoldCommand : Command {
			public BoldCommand() {
				startRegex = new Regex(@"<\s*b\s*>");
				endRegex = new Regex(@"<\s*/b\s*>");
				startCommand = "<b>";
				endCommand = "</b>";
			}
		}
		class ItalicCommand : Command {
			public ItalicCommand() {
				startRegex = new Regex(@"<\s*i\s*>");
				endRegex = new Regex(@"<\s*/i\s*>");
				startCommand = "<i>";
				endCommand = "</i>";
			}
		}
		class SizeCommand : Command {
			public SizeCommand() {
				startRegex = new Regex(@"<\s*size\s*=\s*\d+>");
				endRegex = new Regex(@"<\s*/size\s*>");
				startCommand = "";
				endCommand = "</size>";
			}
		}
		class ColorCommand : Command {
			public ColorCommand() {
				startRegex = new Regex(@"<\s*color\s*=\s*.+>");
				endRegex = new Regex(@"<\s*/color\s*>");
				startCommand = "";
				endCommand = "</color>";
			}
		}
		class WaitCommand : Command {
			private Writer writer;
			private float waitTime = 0.0f;
			public WaitCommand(Writer writer) {
				startRegex = new Regex(@"<\s*wait\s*=\s*\d+(\.\d+)?>");
				endRegex = new Regex(@"<\s*/wait\s*>");
				this.writer = writer;
				isStartOnly = true;
			}
			public override bool isStart(string target) {
				if (startRegex.Match(target).Success) {
					if (startCommand == "") startCommand = target;
					Regex num = new Regex(@"[^\.*0-9]");
					writer.wait(float.Parse(num.Replace(target, "")));
					return true;
				} else {
					return false;
				}
			}
		}
		class SpeedCommand : Command {
			private Writer writer;
			private float waitTime = 0.0f;
			public SpeedCommand(Writer writer) {
				startRegex = new Regex(@"<\s*speed\s*=\s*\d+(\.\d+)?>");
				endRegex = new Regex(@"<\s*/speed\s*>");
				this.writer = writer;
				isStartOnly = true;
			}
			public override bool isStart(string target) {
				if (startRegex.Match(target).Success) {
					if (startCommand == "") startCommand = target;
					Regex num = new Regex(@"[^\.*0-9]");
					writer.speedChange(float.Parse(num.Replace(target, "")));
					return true;
				} else {
					return false;
				}
			}
		}
	}

}
