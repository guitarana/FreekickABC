using UnityEngine;
using System;
using System.IO;
using System.Text;

public class Script {

	public enum TokenType {
		Null = 0,
		Empty = 1,
		String = 2,
		SemiColon = 3,
		OpenBrace = 4,
		CloseBrace = 5,
		EOF = 6,
	}

	private string _token = null;
	private bool _error = false;
	private bool _eof = false;
	private char[] s = null;
	private char[] t = null;
	private int i = 0, j = 0, k = 0, n = 0;

	public bool error {get{return _error;}}
	public bool eof {get{return _eof;}}
	public int line {get{return n;}}
	public string token {get{return _token;}}

	public Script()
	{
		_error = false;
		_eof = true;
	}

	public Script(string scriptFile, bool encrypted = false, bool cloud = false)
	{
		_error = true;
		_eof = true;
		try {
			if (encrypted) {
//				byte[] atad = File.ReadAllBytes(scriptFile);
//				byte[] data = PlayerStatistic.instance.Decrypt(atad);
//				//--// Debug.Log(data.Length + " " + atad.Length);
//				//string file = Encoding.ASCII.GetString(data);
//				//s = file.ToCharArray();
//				//k = file.Length;
//				MemoryStream ms = new MemoryStream(data);
//				StreamReader sr = new StreamReader(ms);
//				string file = sr.ReadToEnd();
//				s = file.ToCharArray();
//				k = file.Length;
			} else {
			using(StreamReader sr = new StreamReader(scriptFile)) {
				string file = sr.ReadToEnd();
				s = file.ToCharArray();
				k = file.Length;
			}
			}

			t = new char[1024];
			if (t != null) {
				_error = false;
				_eof = false;
				n = 1;
			}
		} catch (Exception e) {
#if UNITY_EDITOR
			Debug.Log("Script::ERROR: " + e.ToString());
#else
			Debug.LogError("Script::ERROR: " + e.ToString());
			Debug.Log("Script::ERROR: " + e.ToString());
#endif
		}
	}

	public void Free()
	{
		_token = null;
		s = null;
		t = null;
	}

	public string GetToken()
	{
		return _token;
	}

	public TokenType ReadToken()
	{
		_token = null;
		if (_error)
			return TokenType.Null;

		// skip white space, including ',' ':' '=' and comments
		do {
			while (i < k && (s[i] <= ' ' || s[i] == ',' || s[i] == ':' || s[i] == '=')) {
				if (s[i] == '\n')
					n++;
				i++;
			}
			if (i >= k) // eof?
				break;
			if (s[i] == '/' && s[i+1] == '/') {
				// line comment
				while (i < k && s[i] != '\n')
					i++;
			} else if (s[i] == '/' && s[i+1] == '*') {
				// block comment
				i += 2;
				while (i < k && !(s[i] == '*' && s[i+1] == '/')) {
					if (s[i] == '\n')
						n++;
					i++;
				}
				i += 2;
			} else // got a token
				break;
		} while (i < k);

		// check for eof
		if (i >= k) {
			_eof = true;
			return TokenType.EOF;
		}

		// get the token
		j = 0;
		if (s[i] == '\"') { // quoted string
			i++;
			do {
				if (s[i] == '\"') {
					break;
				} else if (s[i] == '\r') {
					continue;
				} else if (s[i] == '\n') {
					n++;
					t[j++] = ' ';
					i++;
					continue;
				} else if (s[i] == '\\' && (i + 1) < k) {
					if (s[i+1] == '"' || s[i+1] == '\'')
						i++;
				}
				t[j++] = s[i++];
			} while (i < k && j < 1024 && s[i] != '\"');
			if (j >= 1024) {
				Debug.Log("Script::ERROR: Text too long at line " + n);
				_error = true;
				return TokenType.Null;
			}
			i++;
		} else if (s[i] == '\'') { // literal string
			i++;
			do {
				if (s[i] == '\'') {
					break;
				} else if (s[i] == '\r') {
					continue;
				} else if (s[i] == '\n') {
					n++;
					t[j++] = ' ';
					i++;
					continue;
				} else if (s[i] == '\\' && (i + 1) < k) {
					if (s[i+1] == '"' || s[i+1] == '\'')
						i++;
				}
				t[j++] = s[i++];
			} while (i < k && j < 1024 && s[i] != '\'');
			if (j >= 1024) {
				Debug.Log("Script::ERROR: Text too long at line " + n);
				_error = true;
				return TokenType.Null;
			}
			i++;
		} else if (s[i] == ';') {
			i++;
			return TokenType.SemiColon;
		} else if (s[i] == '{') {
			i++;
			return TokenType.OpenBrace;
		} else if (s[i] == '}') {
			i++;
			return TokenType.CloseBrace;
		} else { // regular token
			do {
				t[j++] = s[i++];
			} while (i < k && j < 1024 && s[i] > ' ' && s[i] != ',' && s[i] != ':' && s[i] != '=' && s[i] != ';' && s[i] != '{' && s[i] != '}');
			if (j >= 1024) {
				Debug.Log("Script::ERROR: Text too long at line " + n);
				_error = true;
				return TokenType.Null;
			}
		}

		if (j == 0) // empty token is not null
			return TokenType.Empty;

		_token = new string(t,0,j);

		return TokenType.String;
	}

	public static string Escape(string input)
	{
		if (input == null)
			return null;

		char[] x = input.ToCharArray();
		char[] y = new char[input.Length * 2 + 1];
		int a = 0, b = 0;

		do {
			if (x[a] == '\"' || x[a] == '\'')
				y[b++] = '\\';
			y[b++] = x[a++];
		} while (a < input.Length);

		if (b == 0)
			return input;

		return new string(y,0,b);
	}
}

// EOF
