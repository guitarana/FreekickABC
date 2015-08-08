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

				}
				if (_instance == null) {
					#if UNITY_EDITOR
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
	public int credit = 1000000;

	//Player Game Stat
	public int uniqueid = 242342354;
	public string username = "Player";
	public int globalLevel;
	public int xpGain;
	public int xpRemaining = 20;
	public int globalXPGain;

	//Cosmetic
	public int hatIndex = 100;
	public int glassIndex = 200;
	public int shoesIndex = 400;
	public int clothesIndex = 300;
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

		availableHatIndex.Add (100);
		availableGlassIndex.Add (200);
		availableShoesIndex.Add (400);
		availableClothesIndex.Add (300);
		if(availableAllCostume){
			for(int i=101;i<=105;i++){
				availableHatIndex.Add(i);
			}
			for(int i=201;i<=203;i++){
				availableGlassIndex.Add(i);
			}
			for(int i=401;i<=404;i++){
				availableShoesIndex.Add(i);
			}
			for(int i=301;i<=302;i++){
				availableClothesIndex.Add(i);
			}
		}
		//LoadGame();
		CloudDataController.instance.UpdateStat();
	}
	
	// Update is called once per frame
	void Update ()
	{
//		if(InGameUIManager.instance.inGameState == InGameUIManager.InGameState.PauseGame) return;

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

		string file_new = file + ".new";
		string file_old = file + ".old";
		
		if (File.Exists(file_new)) {
			//			Debug.Log("SAVE: ERROR: " + file_new + " still exists, removing ...");
			try { File.Delete(file_new); } catch {}
		}
		
		using (MemoryStream ms = new MemoryStream()) {
			StreamWriter sw = new StreamWriter(ms);
			
			//			Debug.Log("SAVE: Saving game ...");
			
			sw.WriteLine("$actor playerstat {");
			sw.WriteLine("username " + username + ";");
			sw.WriteLine("globallevel " + globalLevel + ";");
			sw.WriteLine("xpgain " + xpGain + ";");
			sw.WriteLine("xpremaining " + xpRemaining + ";");
		
			foreach(int i in availableHatIndex){
				sw.WriteLine("availableHatIndex " + i + ";"); 
			}

			foreach(int i in availableGlassIndex){
				sw.WriteLine("availableGlassIndex " + i + ";"); 
			}

			foreach(int i in availableClothesIndex){
				sw.WriteLine("availableClothesIndex " + i + ";"); 
			}

			foreach(int i in availableShoesIndex){
				sw.WriteLine("availableShoesIndex " + i + ";"); 
			}

			sw.WriteLine("}");
			
			sw.Flush();
			
			//ms.Position = 0;
			byte[] data = ms.ToArray();
			//byte[] atad = Crypt(data);
			
			//xxx
			using (FileStream fs = new FileStream(file_new, FileMode.Create, FileAccess.Write)) {
				//fs.Write(data, 0, data.Length);
				fs.Write(data, 0, data.Length);
			}
		}
		
		if (!File.Exists(file_new)) {
			// can't write save game
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

	public void Load(string file) {
		

		// check if it is exist first
		if (!File.Exists(file)) {
			// check older save
				return;
		}
		Function obj = null;
		SaveScript playerStat = null;
		playerStat = new SaveScript(file);
		foreach(string key in playerStat.objects.Keys){
			if(key.Contains("playerstat")){
				obj = playerStat.objects[key];
				if(obj.statements != null){
					foreach(Statement s in obj.statements){
						LoadData(s.command);
					}
				}
			}
		}
	}

	public void LoadGame() {

		string path = Path.GetFullPath(Application.persistentDataPath + "/save.dat");
		if(File.Exists(path))
			Load(path);
	}

	public void LoadData(string[] s){
		if(s.Length <2)
			return;
		switch(s[0]){
		case "username":
			username = s[1];
			break;
		case "globallevel":
			globalLevel = int.Parse(s[1]);
			break;
		case "xpgain":
			xpGain = int.Parse(s[1]);
			break;
		case "xpremaining":
			xpRemaining = int.Parse(s[1]);
			break;
		case "availableHatIndex":
			availableHatIndex.Add(int.Parse(s[1]));
			break;
		case "availableGlassIndex":
			availableGlassIndex.Add(int.Parse(s[1]));
			break;
		case "availableClothesIndex":
			availableClothesIndex.Add(int.Parse(s[1]));
			break;
		case "availableShoesIndex":
			availableShoesIndex.Add(int.Parse(s[1]));
			break;
		}
	}




}

