/** @Statement.cs
	Script's statement data structure
*/
using UnityEngine;

public class Statement {
	public GameScript gs = null;
	public string[] command = null;
	public bool wait = true;

	public Statement(GameScript gs, string[] command) {
		this.gs = gs;
		this.command = command;
		//this.wait = true;
	}
}
