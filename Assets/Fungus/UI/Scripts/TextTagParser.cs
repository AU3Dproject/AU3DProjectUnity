using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fungus
{
	
	public class TextTagParser
	{
		public enum TokenType
		{
			Invalid,
			Words,					// A string of words
			BoldStart,				// b
			BoldEnd,				// /b
			ItalicStart,			// i
			ItalicEnd,				// /i
			ColorStart,				// color=red
			ColorEnd,				// /color
			Wait, 					// w, w=0.5
			WaitForInputNoClear, 	// wi
			WaitForInputAndClear, 	// wc
			WaitOnPunctuationStart, // wp, wp=0.5
			WaitOnPunctuationEnd,   // /wp
			Clear, 					// c
			SpeedStart, 			// s, s=60
			SpeedEnd, 				// /s
			Exit, 					// x
			Message,				// m=MessageName
			VerticalPunch,       	// {vpunch=0.5}
			HorizontalPunch,        // {hpunch=0.5}
			Punch,					// {shake=0.5}
			Flash,					// {flash=0.5}
			Audio,					// {audio=Sound}
			AudioLoop,				// {audioloop=Sound}
			AudioPause,				// {audiopause=Sound}
			AudioStop				// {audiostop=Sound}
		}
		
		public class Token
		{
			public TokenType type = TokenType.Invalid;
			public string param = "";
		}

		public static string GetTagHelp()
		{
			return "" +
				"\t{b} Bold Text {/b}\n" + 
				"\t{i} Italic Text {/i}\n" +
				"\t{color=red} Color Text (color){/color}\n" +
				"\n" +
				"\t{s}, {s=60} Writing speed (chars per sec){/s}\n" +
				"\t{w}, {w=0.5} Wait (seconds)\n" +
				"\t{wi} Wait for input\n" +
				"\t{wc} Wait for input and clear\n" +
				"\t{wp}, {wp=0.5} Wait on punctuation (seconds){/wp}\n" +
				"\t{c} Clear\n" +
				"\t{x} Exit, advance to the next command without waiting for input\n" +
				"\n" +
				"\t{vpunch=0.5} Vertically punch screen (intensity)\n" +
				"\t{hpunch=0.5} Horizontally punch screen (intensity)\n" +
				"\t{shake=1} Shake screen (intensity)\n" +
				"\t{flash=0.5} Flash screen (duration)\n" +
				"\n" +
				"\t{audio=AudioObjectName} Play Audio Once\n" +
				"\t{audioloop=AudioObjectName} Play Audio Loop\n" +
				"\t{audiopause=AudioObjectName} Pause Audio\n" +
				"\t{audiostop=AudioObjectName} Stop Audio\n" +
				"\n" +
				"\t{m=MessageName} Broadcast message\n" +
				"\t{$VarName} Substitute variable";
		}

		public virtual List<Token> Tokenize(string storyText)
		{
			List<Token> tokens = new List<Token>();
			
			string pattern = @"\{.*?\}";
			Regex myRegex = new Regex(pattern);
			
			Match m = myRegex.Match(storyText);   // m is the first match
			
			int position = 0;
			while (m.Success)
			{
				// Get bit leading up to tag
				string preText = storyText.Substring(position, m.Index - position);
				string tagText = m.Value;
				
				if (preText != "")
				{
					AddWordsToken(tokens, preText);
				}
				AddTagToken(tokens, tagText);
				
				position = m.Index + tagText.Length;
				m = m.NextMatch();
			}
			
			if (position < storyText.Length)
			{
				string postText = storyText.Substring(position, storyText.Length - position);
				if (postText.Length > 0)
				{
					AddWordsToken(tokens, postText);
				}
			}
			
			// Remove all leading whitespace & newlines after a {c} or {wc} tag
			// These characters are usually added for legibility when editing, but are not 
			// desireable when viewing the text in game.
			bool trimLeading = false;
			foreach (Token token in tokens)
			{
				if (trimLeading &&
				    token.type == TokenType.Words)
				{
					token.param = token.param.TrimStart(' ', '\t', '\r', '\n');
				}
				
				if (token.type == TokenType.Clear || 
				    token.type == TokenType.WaitForInputAndClear)
				{
					trimLeading = true;
				}
				else
				{
					trimLeading = false;
				}
			}
			
			return tokens;
		}
		
		protected virtual void AddWordsToken(List<Token> tokenList, string words)
		{
			Token token = new Token();
			token.type = TokenType.Words;
			token.param = words;
			tokenList.Add(token);
		}
		
		protected virtual void AddTagToken(List<Token> tokenList, string tagText)
		{
			if (tagText.Length < 3 ||
			    tagText.Substring(0,1) != "{" ||
			    tagText.Substring(tagText.Length - 1,1) != "}")
			{
				return;
			}
			
			string tag = tagText.Substring(1, tagText.Length - 2);
			
			TokenType type = TokenType.Invalid;
			string paramText = ExtractParameter(tag);
			
			if (tag == "b")
			{
				type = TokenType.BoldStart;
			}
			else if (tag == "/b")
			{
				type = TokenType.BoldEnd;
			}
			else if (tag == "i")
			{
				type = TokenType.ItalicStart;
			}
			else if (tag == "/i")
			{
				type = TokenType.ItalicEnd;
			}
			else if (tag.StartsWith("color="))
			{
				type = TokenType.ColorStart;
			}
			else if (tag == "/color")
			{
				type = TokenType.ColorEnd;
			}
			else if (tag == "wi")
			{
				type = TokenType.WaitForInputNoClear;
			}
			if (tag == "wc")
			{
				type = TokenType.WaitForInputAndClear;
			}
			else if (tag.StartsWith("wp="))
			{
				type = TokenType.WaitOnPunctuationStart;
			}
			else if (tag == "wp")
			{
				type = TokenType.WaitOnPunctuationStart;
			}
			else if (tag == "/wp")
			{
				type = TokenType.WaitOnPunctuationEnd;
			}
			else if (tag.StartsWith("w="))
			{
				type = TokenType.Wait;
			}
			else if (tag == "w")
			{
				type = TokenType.Wait;
			}
			else if (tag == "c")
			{
				type = TokenType.Clear;
			}
			else if (tag.StartsWith("s="))
			{
				type = TokenType.SpeedStart;
			}
			else if (tag == "s")
			{
				type = TokenType.SpeedStart;
			}
			else if (tag == "/s")
			{
				type = TokenType.SpeedEnd;
			}
			else if (tag == "x")
			{
				type = TokenType.Exit;
			}
			else if (tag.StartsWith("m="))
			{
				type = TokenType.Message;
			}
			else if (tag.StartsWith("vpunch") ||
			         tag.StartsWith("vpunch="))
			{
				type = TokenType.VerticalPunch;
			}
			else if (tag.StartsWith("hpunch") ||
			         tag.StartsWith("hpunch="))
			{
				type = TokenType.HorizontalPunch;
			}
			else if (tag.StartsWith("punch") ||
			         tag.StartsWith("punch="))
			{
				type = TokenType.Punch;
			}
			else if (tag.StartsWith("flash") ||
			         tag.StartsWith("flash="))
			{
				type = TokenType.Flash;
			}
			else if (tag.StartsWith("audio="))
			{
				type = TokenType.Audio;
			}
			else if (tag.StartsWith("audioloop="))
			{
				type = TokenType.AudioLoop;
			}
			else if (tag.StartsWith("audiopause="))
			{
				type = TokenType.AudioPause;
			}
			else if (tag.StartsWith("audiostop="))
			{
				type = TokenType.AudioStop;
			}
			
			if (type != TokenType.Invalid)
			{
				Token token = new Token();
				token.type = type;
				token.param = paramText.Trim();			
				tokenList.Add(token);
			}
			else
			{
				Debug.LogWarning("Invalid text tag " + tag);
			}
		}
		
		protected virtual string ExtractParameter(string input)
		{
			int index = input.IndexOf('=');
			if (index == -1)
			{
				return "";
			}
			
			return input.Substring(index +1);
		}
	}
	
}