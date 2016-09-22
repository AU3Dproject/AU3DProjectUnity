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
    public bool isTextVisible = true;
	[Tooltip("テキストの表示速度[秒]")]
    public float textVisibleTime = 0.05f;
	public float waitTime = 0.0f;

    private float time = 0.0f;
    private int i = 0;
    private string command;
	private string endCommand;
	private bool commandMode = false;
	private AudioSource audioSource;
    private Text textComponent;
	private CommandFunction commandFunction;

    // Use this for initialization
    void Start() {
        textComponent = transform.FindChild("Text").GetComponent<Text>();
		audioSource = GetComponent<AudioSource>();
		commandFunction = new CommandFunction(this);
    }

    // Update is called once per frame
    void Update() {

        if (isTextVisible) {
            time += Time.deltaTime;

			//テキストの順次表示処理
            if (time > textVisibleTime && i < text.Length) {
			
				//次の表示文字の抽出
				string nextString = getStringSingle(text,i++);

				//Unityリッチテキストでのファンクションを処理
				while (nextString == "<") {
                    command = "<";
                    while(nextString != ">") {
						nextString = getStringSingle(text, i++);
						command += nextString;
					}
					Debug.Log("Command:"+command);
					commandFunction.checkFunctionStartEnd(command);

					//再度文字の抽出
					nextString = getStringSingle(text, i++);

				}

				//ファンクション中の場合一文字に対してファンクションを付加
				//そして抽出した文字を表示させる。
				if (commandFunction.isCommand() == true) textComponent.text += commandFunction.addCommands(nextString);
                else textComponent.text += nextString;

				if(nextString != " " && nextString != "　" && nextString != "\n")audioSource.PlayOneShot(audioSource.clip);

				//次の文字へ・時間を検知時間分シフト
                time -= textVisibleTime;
            }
        } else {
            i = 0;
            time = 0.0f;
        }

    } 

	private string getStringSingle(string target,int index) {
		return (target.Substring(index,1));
	}

	private char getCharacterSingle(string target,int index) {
		return (target[index]);
	}

	public void wait(float second) {
		time -= second;
	}


	

	//文中に登場させることのできるコマンド（<b>など）に関するクラス
	class CommandFunction {
		private BoldCommand boldCommand;
		private ItalicCommand italicCommand;
		private SizeCommand sizeCommand;
		private ColorCommand colorCommand;
		private WaitCommand waitCommand;

		private Command[] commands;
		private List<Command> applyCommands = new List<Command>();

		private Writer writer;

		public CommandFunction(Writer writer) {
			boldCommand = new BoldCommand();
			italicCommand = new ItalicCommand();
			sizeCommand = new SizeCommand();
			colorCommand = new ColorCommand();
			waitCommand = new WaitCommand(writer);
			Command[] commands = { boldCommand, italicCommand, sizeCommand, colorCommand, waitCommand };
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
	}

}
