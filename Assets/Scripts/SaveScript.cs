using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


public class SaveScript {

	private Script script = null;
	private bool _error = false;

	public Dictionary<string,Function> objects = null; // table of savegame objects indexed by name

	public bool error {get{return _error;}}

	public SaveScript(string scriptFile)
	{
		//--// Debug.Log("SaveScript::Load: " + scriptFile);
		script = new Script(scriptFile, true);
		if (script == null) {
			_error = true;
			return;
		} else if (script.error) {
			_error = true;
			script.Free();
			return;
		}

		objects = new Dictionary<string,Function>();
		if (objects == null) {
			_error = true;
			script.Free();
			return;
		}

		Script.TokenType tokentType = Script.TokenType.Null;
		List<string> line = new List<string>();
		string[] a = null;
		string token = null;
		int bc = 0, oc = 0, sc = 0;
		Function obj = null;
		bool scriptError = false;

		do {
			tokentType = script.ReadToken();

			switch (tokentType) {

			case Script.TokenType.Null:
				scriptError = true;
				break;

			case Script.TokenType.Empty:
				line.Add("null");
				break;

			case Script.TokenType.String:
				token = script.GetToken();
				line.Add(token);
				break;

			case Script.TokenType.SemiColon:
				a = line.ToArray(); line.Clear();
				Statement s = new Statement(null, a);
				if (obj != null) {
					if (s != null)
						obj.statements.Add(s);
				}
				if (bc == 0) {
					//--// Debug.Log("SaveScript::ERROR: Statement out of scope at line " + script.line);
					scriptError = true;
				}
				sc++;
				break;

			case Script.TokenType.OpenBrace:
				bc++;
				if (bc > 1) {
					scriptError = true;
				}
				a = line.ToArray(); line.Clear();
				if (a[0] == "$actor") {
					obj = new Function(null, a[1]);
					if (obj != null) {
						obj.id = objects.Count;
						if(!objects.ContainsKey(a[1]))
							objects.Add(a[1], obj);
					}
					oc++;
				}

				break;

			case Script.TokenType.CloseBrace:
				bc--;
				if (bc < 0) {
					scriptError = true;
				}
				break;

			case Script.TokenType.EOF:
				break;

			}

			if (scriptError)
				break;

		} while (!script.eof);

		script.Free();
		line.Clear();

		if (scriptError)
			_error = true;
	}

}

