using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


public class GameScript {

	private Script script = null;
	private bool _error = false;
	
	public GameObject executor = null;

	public Dictionary<string,GameObject> variables = null; // table of gameobjects indexed by name
	public Dictionary<string,Function> functions = null; // table of functions indexed by name

	public bool error {get{return _error;}}

	public GameScript(string scriptFile)
	{
		//--// Debug.Log("GameScript: Load " + scriptFile);
		script = new Script(scriptFile, true);
		if (script == null) {
			_error = true;
			return;
		} else if (script.error) {
			_error = true;
			script.Free();
			return;
		}

		variables = new Dictionary<string,GameObject>();
		functions = new Dictionary<string,Function>();
		if (variables == null || functions == null) {
			_error = true;
			script.Free();
			return;
		}

		Script.TokenType tokentType = Script.TokenType.Null;
		List<string> line = new List<string>();
		string[] a = null;
		string token = null;
		int bc = 0, fc = 0, vc = 0, sc = 0;
		Function function = null;
		bool force_nowait = false;
		bool force_wait = false;
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
				if (token == "wait")
					force_wait = true;
				else if (token == "nowait")
					force_nowait = true;
				else
					line.Add(token);
				break;

			case Script.TokenType.SemiColon:
				a = line.ToArray(); line.Clear();
				if (a[0] == "var") {
					variables.Add(a[1], null);
					vc++;
				} else {
					// statement
					Statement statement = new Statement(this, a);
					if (function != null) {
						if (statement != null) {
							if (force_wait)
								statement.wait = true;
							if (force_nowait)
								statement.wait = false;
							if (a[0] == "delay")
								statement.wait = true;
							function.statements.Add(statement);
						}
					}
					if (bc == 0) {
						//--// Debug.Log("GameScript::ERROR: Statement out of scope at line " + script.line + " (statement must be inside a function)");
						scriptError = true;
					}
					sc++;
				}
				force_wait = false;
				force_nowait = false;
				break;

			case Script.TokenType.OpenBrace:
				bc++;
				if (bc > 1) {
					//--// Debug.Log("GameScript::ERROR: Excessive { at line " + script.line + " (might be missing } somewhere before)");
					scriptError = true;
				}
				a = line.ToArray(); line.Clear();
				if (a[0] == "function") {
					function = new Function(this, a[1]);
					if (function != null) {
						function.id = functions.Count;
						functions.Add(a[1], function);
					}
					fc++;
				}
				break;

			case Script.TokenType.CloseBrace:
				bc--;
				if (bc < 0) {
					//--// Debug.Log("GameScript::ERROR: Excessive } at line " + script.line + " (might be missing { somewhere before)");
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

