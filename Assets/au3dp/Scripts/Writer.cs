using UnityEngine;
using UnityEngine.UI;
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

	[Tooltip("表示するテキストの最大行数（これに合わせてウィンドウがリサイズされることはない。そっちは手動でやってね♪）")]
	public int maxLine = 3;

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
	//実際に文字を表示してくれるコンポーネント
    public Text textComponent;
	//コマンドの検出等を行うインスタンス
	private CommandFunction commandFunction;
	//コマンド処理を行うためのテキストのバッファ(一時保管庫)
	private string textBuff = "";
	//スキップ状態を表す変数
	private bool isSkip = false;
	//現在の行
	private int nowLine = 0;

	public WaitArrowScript waitArrow = null;


    /* Start
     * 　（１）TextComponentの取得
     * 　（２）AudioSourceComponentの取得
     * 　（３）コマンドファンクションのインスタンス化
     */
    void Start() {
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
		if (isTextActive && text != "") {

			if (waitArrow != null) {
				if (waitArrow.isVisible) {
					waitArrow.setVisible(false);
				}
			}
			
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
				if (isSound(nextString)) {
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
			if (!waitArrow.isVisible) {
				waitArrow.setVisible(true);
			}
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
		time = 0.0f;
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

	/* 文字の挿入
	 * 　（１）与えられた文字列を現在表示中の文字の次に挿入する。
	 * 　（２）なお、もしも特定の位置に文字を挿入したい場合は通常の文字列関数Insertを任意に使って、どうぞ。
	 */
	public void insertString(string str) {
		text = text.Insert(i,str);
	}

	/* 表示文字列の削除
	 * 　（１）表示しているすべての文字と、これから表示するすべての文字列を全削除する。
	 * 　（２）一度削除したら元に戻せなくなる。
	 */
	public void removeText(){
		textComponent.text = "";
		textBuff = "";
	}

	/* ウィンドウの削除
	 * 　（１）親となるCanvasもろとも削除してしまう。
	 * 　（２）GameObjectをそもそも削除するのでその後の、操作はすべて無効になる。
	 */
	public void removeWindow(){
		Destroy (transform.parent.gameObject);
	}


	

	//文中に登場させることのできるコマンド（<b>など）に関するクラス
	class CommandFunction {
		//すべてのコマンドを一度インスタンス化する。
		private BoldCommand boldCommand;
		private ItalicCommand italicCommand;
		private SizeCommand sizeCommand;
		private ColorCommand colorCommand;
		private WaitCommand waitCommand;
		private SpeedCommand speedCommand;
		private VariableCommand variableCommand;

		//判定するコマンドはこの配列に格納する。
		private Command[] commands;

		//現在適応中のコマンドのリスト
		private List<Command> applyCommands = new List<Command>();

		//コマンドでWriterクラスにアクセスしたい場合（例えばウェイトなどの処理の場合）のため、Writerを保持する
		private Writer writer;

		/* コンストラクタ
		 * 　（１）各コマンドのインスタンスを作成
		 * 　（２）作成したインスタンスをコマンド配列に格納
		 * 　（３）引数として取得したwriterクラスを保持する
		 */
		public CommandFunction(Writer writer) {
			boldCommand = new BoldCommand();
			italicCommand = new ItalicCommand();
			sizeCommand = new SizeCommand();
			colorCommand = new ColorCommand();
			waitCommand = new WaitCommand(writer);
			speedCommand = new SpeedCommand(writer);
			variableCommand = new VariableCommand(writer);
			Command[] commands = { boldCommand, italicCommand, sizeCommand, colorCommand, waitCommand, speedCommand,variableCommand };
			this.commands = commands;
			this.writer = writer;
		}

		/* 開始コマンド(<b>など)の取得
		 * 　（１）適応中のコマンドがある場合、その開始コマンドをリスト内の昇順で返却文字列に加算していく。
		 * 　（２）すべての適応中の開始コマンドを加算したらそれを返却。
		 * 　（例えば"<b>秋田<i>大学</i></b>なら適当中のコマンドはbold -> italicの順。返却コマンドは<b><i>となる。"）
		 */
		public string getStartCommand() {
			string retCommand = "";
			foreach (Command c in applyCommands) {
				retCommand += c.getStartCommand();
			}
			return retCommand;
		}

		/* 終了コマンド(</b>など)の取得
		 * 　（１）適応中のコマンドがある場合、その終了コマンド文字列をリスト内の降順で返却文字列に加算していく。
		 * 　（２）すべての適応中の終了コマンドを加算したらそれを返却。
		 * 　（例えば"<b>秋田<i>大学</i></b>なら適当中のコマンドはbold -> italicの順。返却コマンドは</i></b>となる。"）
		 */
		public string getEndCommand() {
			string retCommand = "";
			foreach (Command c in applyCommands) {
				if (retCommand == "") retCommand = c.getEndCommand();
				else retCommand = c.getEndCommand() + retCommand;
			}
			return retCommand;
		}

		/* 文字列へコマンドの追加
		 * 　（１）受け取った文字列に開始コマンドと終了コマンドを上の2つのメソッドを用いて追加する。
		 * 　（２）それを返却
		 */
		public string addCommands(string target) {
			return getStartCommand() + target + getEndCommand();
		}

		/* コマンドの開始と終了の判定
		 * 　（１）もし、受け取った文字列が終了コマンドなら終了コマンド処理をして終了
		 * 　（２）もし、受け取った文字列が終了コマンドでないなら、開始コマンドの処理をして終了。
		 */
		public void checkFunctionStartEnd(string target) {
			if (isFunctionEnd(target) == false) isFunctionStart(target);
		}

		/* 開始コマンド判定
		 * 　（１）配列に格納したコマンド達の開始コマンド判定を行う。
		 * 　（２）開始コマンドだった場合、適応コマンドリストにそのコマンドを追加する。
		 * 　（３）ただし、開始のみのフラグが立っていたら、追加せずに終了する。
		 * 　（４）開始コマンドじゃなかった場合何もせず終了。
		 */
		public bool isFunctionStart(string target) {
			foreach (Command c in commands) {
				if (c.isStart(target) == true) {
					if(c.isStartOnly==false)applyCommands.Add(c);
					return true;
				}
			}
			return false;

		}

		/* 終了コマンド判定
		 * 　（１）コマンドが終了コマンドなら適応コマンドリストの一番後ろを削除する。
		 */
		public bool isFunctionEnd(string target) {
			foreach (Command c in commands) {
				if (c.isEnd(target) == true) {
					if (c.isStartOnly == false) applyCommands.RemoveAt(applyCommands.Count-1);
					return true;
				}
			}
			return false;

		}

		/* コマンド適応中判定
		 * 　（１）何か適応中のコマンドがある場合はTrue
		 */
		public bool isCommand() {
			if (applyCommands.Count > 0) {
				return true;
			} else {
				return false;
			}
		}

		//コマンドのクラス
		//新規コマンドの作り方
		//基本的に<~~~>という形のコマンドを作成できます。
		//<b>ABCD</b>などの開いて閉じるコマンドはUnityのリッチテキストの機能なので作ってもバグります。
		//なので自作コマンドは基本的に開始オンリーコマンドになります。（<wait=10>だけとかで</wait>はいらない。）
		//まずCommandクラスを継承した新たなMyCommandクラスを作成します。
		//作るメソッドは主に2つでコンストラクタとisStartクラスです。
		//コンストラクタでは基本的にstartRegexとendRegexの初期化と、isStartOnlyフラグをTrueにすること、startCommandとendCommandの初期化をします。
		//isStartではstartRegexがマッチしたときの処理を行います。
		//startRegexを検知した瞬間に特定の動作（例えばWaitやらSpeedChangeやらを）行います。
		//この時、もし上位クラスのWriterにアクセスしたい場合はコンストラクタの引数からWriterインスタンスを取得するようにしましょう。
		//詳しいやり方はWaitCommandあたりを見ながら実装してみてください。
		//また、Writerクラスにしてほしい処理がある場合はその都度、Writerクラスにメソッドを追加しましょう。
		//isStartでコマンドにマッチし、処理を行ったあとはTrue、マッチしなかった場合のみFalseを返却させます。
		//また、isStartメソッドは親クラスCommandではvirtual宣言されているため、overrideをすることで、完全な上書きをします。
		//この辺はC# overrideあたりで勉強してください。
		//また、追加したいコマンドはもしかしたらイベントスクリプトの方で実行したほうが良い場合があります。
		//部分的な文字列の状態、表示速度等に関するコマンド以外はそちらに書いたほうが良いと思うので注意しましょう。（別にやってもいいけど冗長する気しかしない）
		//正規表現は便利ですが少し実行速度に難ありの可能性があります。が、コマンドの文字列判定や抽出はコード化するとかなり長くなるし、正規表現で書いたほうが気持ちいいのじゃ。
		//読んでもわからない方はコード理解して、どうぞ。
		//（長文ニキおっつおっつ）
		class Command {
			//開始コマンドの正規表現
			protected Regex startRegex;
			//終了コマンドの正規表現
			protected Regex endRegex;
			//終了コマンド返却の際、返却する文字列
			public string endCommand = null;
			//開始コマンド返却の際、返却する文字列
			public string startCommand = null;
			//開始のみのコマンドかどうか
			public bool isStartOnly = false;

			/* 開始コマンド判定
			 * 　（１）開始コマンドの正規表現にマッチしていた場合
			 * 　（２）もし開始コマンド文字列が空なら、受け取った文字列を開始コマンド文字列とする。
			 * 　（３）そしてTrueを返却する。
			 * 　（４）マッチしなかったらFalse
			 */
			public virtual bool isStart(string target) {
				if (startRegex.Match(target).Success) {
					if (startCommand == "") startCommand = target;
					return true;
				} else {
					return false;
				}
			}
			/* 終了コマンド判定
			 * 　（１）終了コマンドの正規表現にマッチしていた場合
			 * 　（２）もし終了コマンド文字列が空なら、受け取った文字列を終了コマンド文字列とする。
			 * 　（３）そしてTrueを返却する。
			 * 　（４）マッチしなかったらFalse
			 */
			public virtual bool isEnd(string target) {
				if (endRegex.Match(target).Success) {
					if (endCommand == "") endCommand = target;
					return true;
				} else {
					return false;
				}
			}
			/* 開始コマンド返却
			 * 　（１）開始コマンドがNullじゃなかったら返却
			 */
			public virtual string getStartCommand() {
				if (startCommand != null) return startCommand;
				else return "";
			}
			/* 終了コマンド返却
			 * 　（１）終了コマンドがNullじゃなかったら返却
			 */
			public virtual string getEndCommand() {
				if (endCommand != null) return endCommand;
				else return "";
			}
		}

		//文字を太字にするボールドコマンド
		class BoldCommand : Command {
			public BoldCommand() {
				startRegex = new Regex(@"<b>");
				endRegex = new Regex(@"</b>");
				startCommand = "<b>";
				endCommand = "</b>";
			}
		}
		//文字をイタリックにするイタリックコマンド
		class ItalicCommand : Command {
			public ItalicCommand() {
				startRegex = new Regex(@"<i>");
				endRegex = new Regex(@"</i>");
				startCommand = "<i>";
				endCommand = "</i>";
			}
		}
		//文字のサイズを変更するサイズコマンド
		class SizeCommand : Command {
			public SizeCommand() {
				startRegex = new Regex(@"<size=\d+>");
				endRegex = new Regex(@"</size>");
				startCommand = "";
				endCommand = "</size>";
			}
		}
		//文字の色を変更するカラーコマンド
		class ColorCommand : Command {
			public ColorCommand() {
				startRegex = new Regex(@"<color=.+>");
				endRegex = new Regex(@"</color>");
				startCommand = "";
				endCommand = "</color>";
			}
		}
		//文字表示中に表示待ちを行うウェイトコマンド
		class WaitCommand : Command {
			//ウェイトはWriterのメソッドで行うためコンストラクタから取得する。
			private Writer writer;
			//ウェイト時間
			private float waitTime = 0.0f;
			
			/* コンストラクタ
			 * 　（１）Writerを取得
			 * 　（２）正規表現を記述
			 * 　（３）開始限定コマンドとする。
			 */
			public WaitCommand(Writer writer) {
				startRegex = new Regex(@"<wait=\d+(\.\d+)?>");
				endRegex = new Regex(@"</wait>");
				this.writer = writer;
				isStartOnly = true;
			}
			/* 開始判定
			 * 　（１）もし、開始コマンド正規表現と受け取った文字列がマッチしていたら
			 * 　（２）受け取った文字列の中からウェイト時間を取得
			 * 　（３）writerのウェイトメソッドにウェイト時間を渡して実行
			 */
			public override bool isStart(string target) {
				if (startRegex.Match(target).Success) {
					Regex num = new Regex(@"[^\.*0-9]");
					writer.wait(float.Parse(num.Replace(target, "")));
					return true;
				} else {
					return false;
				}
			}
		}
		//文字表示のスピードを変更するスピードコマンド
		class SpeedCommand : Command {
			private Writer writer;
			private float waitTime = 0.0f;
			public SpeedCommand(Writer writer) {
				startRegex = new Regex(@"<speed=\d+(\.\d+)?>");
				endRegex = new Regex(@"</speed>");
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
		//イベント変数に格納された文字列を表示するバリアブルコマンド
		class VariableCommand : Command {
			private Writer writer;
			public VariableCommand(Writer writer) {
				startRegex = new Regex(@"<var=.+>");
				endRegex = new Regex(@"</var>");
				this.writer = writer;
				isStartOnly = true;
			}
			public override bool isStart(string target) {
				if (startRegex.Match(target).Success) {
					string value = "";
					value = TalkEventValiableManager.Instance.getVariable_Command(target).value;
					writer.insertString(value);
					return true;
				} else {
					return false;
				}
			}
		}
	}

}
