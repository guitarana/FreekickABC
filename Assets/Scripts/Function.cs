/** @file Function.cs
	Script's function data structure
*/
using UnityEngine;
using System.Collections.Generic;

public class Function {
	public string name;
	public List<Statement> statements;
	public int id; // function id

	public GameScript gs;

	public Function(GameScript gs, string name) {
		this.name = name;
		this.statements = new List<Statement>();
		this.id = 0;
		this.gs = gs;
	}
}
