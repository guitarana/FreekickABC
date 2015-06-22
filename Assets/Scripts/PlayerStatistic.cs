using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

public class PlayerStatistic : MonoBehaviour
{
	public static PlayerStatistic _instance;
	
	public static PlayerStatistic instance {
		get {
			if (_instance == null) {
				GameObject go = FindObjectOfType(typeof(PlayerStatistic)) as GameObject;
				if (go == null)
					go = new GameObject("@PLAYERSTAT", typeof(PlayerStatistic));
				if (go != null) {
					DontDestroyOnLoad(go);
					_instance = go.GetComponent<PlayerStatistic>();
					Debug.Log("GET PLAYERSTAT");
				}
				if (_instance == null) {
					#if UNITY_EDITOR
					Debug.Log("/*UIMGR*/ ERROR: Can't create PLAYERSTAT object");
					Debug.Break();
					#else
					// = fatal error in release build
					Debug.LogError("/*UIMGR*/ ERROR: Can't create LevelManager object");
					Application.Quit();
					#endif
				}
			}
			return _instance;
		}
	}

	public void Create()
	{
		/* SHOULD BE EMPTY */
	}
	
	public void Init()
	{
		/* Initialization */
	}

	//Global Stat
	public int highScoreArcade;
	public int highScoreTimeAttack;
	public int highScoreOneBall;

	//Player Game Stat
	public string username = "Player";
	public int globalLevel;
	public int xpGain;
	public int xpRemaining = 15;
	public int globalXPGain;

	//Cosmetiic
	public int hatIndex = 5;
	public int glassIndex = 3;
	public int shoesIndex = 3;
	public int clothesIndex = 2;
	public List<int> availableHatIndex = new List<int>();
	public List<int> availableGlassIndex = new List<int>();
	public List<int> availableShoesIndex = new List<int>();
	public List<int> availableClothesIndex = new List<int>();


	//Local GameMode Stat
	public int targetScore;
	public int score;
	public int shots;
	public int goals;
	public float successRatio;
	public int bonus;
	public int goalKeeperBlock;
	public int chart;
	public bool availableAllCostume = true;

	// Use this for initialization
	void Start ()
	{
		availableHatIndex.Add (5);
		availableGlassIndex.Add (3);
		availableShoesIndex.Add (4);
		availableClothesIndex.Add (2);
		if(availableAllCostume){
			for(int i=0;i<5;i++){
				availableHatIndex.Add(i);
			}
			for(int i=0;i<3;i++){
				availableGlassIndex.Add(i);
			}
			for(int i=0;i<4;i++){
				availableShoesIndex.Add(i);
			}
			for(int i=0;i<2;i++){
				availableClothesIndex.Add(i);
			}
		}
		LoadGame();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(InGameUIManager.instance.inGameState == InGameUIManager.InGameState.PauseGame) return;

		if(xpGain==xpRemaining)
			UpgradeXP();
	}

	void UpgradeXP(){
		xpRemaining = xpRemaining + (int)((globalLevel/100*xpRemaining));
		globalLevel +=1;
	}

	public void SaveGame(){
		string file = Path.GetFullPath(Application.persistentDataPath + "/save.dat");
		Save(file);
	}
		
	private static byte[] key = new byte[8] {0x44,0x72,0x65,0x61,0x64,0x4F,0x75,0x74}; 
	
	public static byte[] Crypt(byte[] data)
	{
		SymmetricAlgorithm algorithm = DES.Create();
		ICryptoTransform transform = algorithm.CreateEncryptor(key, key);
		return transform.TransformFinalBlock(data, 0, data.Length);
	}
	
	public static byte[] Decrypt(byte[] data)
	{
		SymmetricAlgorithm algorithm = DES.Create();
		ICryptoTransform transform = algorithm.CreateDecryptor(key, key);
		return transform.TransformFinalBlock(data, 0, data.Length);
	}

	public void Save(string file) {
		
		Debug.Log("PlayerStat.cs: #SAVE#");
		
		string file_new = file + ".new";
		string file_old = file + ".old";
		
		if (File.Exists(file_new)) {
			//			Debug.Log("SAVE: ERROR: " + file_new + " still exists, removing ...");
			try { File.Delete(file_new); } catch {}
		}
		
		using (MemoryStream ms = new MemoryStream()) {
			StreamWriter sw = new StreamWriter(ms);
			
			//			Debug.Log("SAVE: Saving game ...");
			
			sw.WriteLine("$playerstat {");
			sw.WriteLine("username \"" + username + "\";");
			sw.WriteLine("globallevel \"" + globalLevel + "\";");
			sw.WriteLine("xpgain \"" + xpGain + "\";");
			sw.WriteLine("xpremaining \"" + xpRemaining + "\";");

			sw.WriteLine("}");
			
			sw.Flush();
			
			//ms.Position = 0;
			byte[] data = ms.ToArray();
			byte[] atad = Crypt(data);
			
			//xxx
			using (FileStream fs = new FileStream(file_new, FileMode.Create, FileAccess.Write)) {
				//fs.Write(data, 0, data.Length);
				fs.Write(atad, 0, atad.Length);
			}
		}
		
		if (!File.Exists(file_new)) {
			// can't write save game
			Debug.Log("SAVE: ERROR: Can't write save game! " + file_new);
		} else {
			// remove the older save
			if (File.Exists(file_old)) {
				// old file exists, delete it
				try { File.Delete(file_old); } catch {}
				//				Debug.Log("SAVE: removing " + file_old);
			}
			// make current save as older save
			if (File.Exists(file)) {
				try { File.Move(file, file_old); } catch {}
				//				Debug.Log("SAVE: rename " + file + " to " + file_old);
			}
			// make the new save as current save
			try { File.Move(file_new, file); } catch {}
			//			Debug.Log("SAVE: rename " + file_new + " to " + file);
			if (File.Exists(file_new)) {
				//				Debug.Log("SAVE: ERROR: " + file_new + " still exists, removing ...");
				try { File.Delete(file_new); } catch {}
			}
		}
		
	}

	public void LoadGame() {
		string path = Path.GetFullPath(Application.persistentDataPath + "/save.dat");
		if(File.Exists(path))
			parser (path);
	}

	void parser(string dataTemp){
		Script script = new Script(dataTemp, true);
		
		Script.TokenType tokentType = Script.TokenType.Null;
		List<string> line = new List<string>();
		string[] a = null;
		string token = null;
		int bc = 0, ac = 0, sc = 0;
		
		do {
			tokentType = script.ReadToken();
			
			switch (tokentType) {
				
			case Script.TokenType.Null:
				//return;
				break;
				
			case Script.TokenType.Empty:
				line.Add("null");
				break;
				
			case Script.TokenType.String:
				token = script.GetToken();
				line.Add(token);
				break;
				
			case Script.TokenType.SemiColon:


				sc++;
				
				break;
				
			case Script.TokenType.OpenBrace:
				bc++;
				a = line.ToArray(); line.Clear();
				if (a[0] == "username") {
					username = a[1];
					//--// Debug.Log (enviAreas);
					ac++;
				}

				if (a[0] == "globallevel") {
					globalLevel = int.Parse(a[1]);
					//--// Debug.Log (enviAreas);
					ac++;
				}

				if (a[0] == "xpgain") {
					xpGain = int.Parse(a[1]);
					//--// Debug.Log (enviAreas);
					ac++;
				}

				if (a[0] == "xpremaining") {
					xpRemaining = int.Parse(a[1]);
					//--// Debug.Log (enviAreas);
					ac++;
				}

				break;
				
			case Script.TokenType.CloseBrace:
				
				break;
				
			case Script.TokenType.EOF:
				break;
				
			}
			//			enviObject.add(eo);
		} while (!script.eof);
		
	}



}

