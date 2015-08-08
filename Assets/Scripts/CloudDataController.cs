using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CloudDataController : MonoBehaviour
{
	public static CloudDataController instance;

    private string secretKey = "freeKickABC"; // Edit this value and make sure it's the same as the one stored on the server
    public string addScoreURL = "http://localhost/TestServerSide/addscore.php?"; //be sure to add a ? to your url
    public string highscoreURL = "http://localhost/TestServerSide/display.php";
	public string addStatsURL = "http://localhost/TestServerSide/addstats.php?";
	public string getStatsURL = "http://localhost/TestServerSide/getstats.php?";
	public string getDataURL = "http://localhost/TestServerSide/getdata.php?";

	private string _uniqueid = "0";
	private string _credit = "1000000";
	private string _username = "Player";
	private string _globalLevel;
	private string _xpGain;
	private string _xpRemaining = "20";
	private string _availableHat; 
	private string _availableGlass;
	private string _availableClothes;
	private string _availableShoes;

	private bool userExist;
	private bool dataDownloaded;
	private bool errorDownload;

    void Start()
    {
		instance = this;
		DontDestroyOnLoad(this);
       // StartCoroutine(GetScores());
    }

	private int dataCount;
	private bool dataLoaded;
	public void UpdateStat(){
		StartCoroutine(CheckExistingUser(PlayerStatistic.instance.uniqueid));
		//if(userExist){
			StartCoroutine(GetUserData(PlayerStatistic.instance.uniqueid,"GetName"));
			StartCoroutine(GetUserData(PlayerStatistic.instance.uniqueid,"GetLevel"));
			StartCoroutine(GetUserData(PlayerStatistic.instance.uniqueid,"GetXPGain"));
			StartCoroutine(GetUserData(PlayerStatistic.instance.uniqueid,"GetXPRemaining"));
			StartCoroutine(GetUserData(PlayerStatistic.instance.uniqueid,"GetHat"));
			StartCoroutine(GetUserData(PlayerStatistic.instance.uniqueid,"GetGlass"));
			StartCoroutine(GetUserData(PlayerStatistic.instance.uniqueid,"GetClothes"));
			StartCoroutine(GetUserData(PlayerStatistic.instance.uniqueid,"GetShoes"));
			StartCoroutine(GetUserData(PlayerStatistic.instance.uniqueid,"GetCredits"));

		//}
	}

	public void SetStat(){
		UpdateStat();
		StartCoroutine(PostStats());
	}

 
    // remember to use StartCoroutine when calling this function!
    public IEnumerator PostScores(string name, int score)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
		string hash = MD5S.Md5Sum(name + score + secretKey);
 
        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
 
        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done
 
        if (hs_post.error != null)
        {
            print("There was an error posting the high score: " + hs_post.error);
        }
    }
 
    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    IEnumerator GetScores()
    {
        gameObject.GetComponent<UILabel>().text = "Loading Scores";
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;
 
        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
			gameObject.GetComponent<UILabel>().text = hs_get.text; // this is a GUIText that will display the scores in game.
        }
    }

	// remember to use StartCoroutine when calling this function!
	public IEnumerator PostStats()
	{
		if(errorDownload){
			Debug.Log("Check Your Connectivity");
			yield break;
		}
		yield return (dataDownloaded == true);

		if(!userExist){

			_uniqueid = PlayerStatistic.instance.uniqueid.ToString();
			_credit = PlayerStatistic.instance.credit.ToString();
			_username = PlayerStatistic.instance.username;
			_globalLevel = PlayerStatistic.instance.globalLevel.ToString();
			_xpGain = PlayerStatistic.instance.xpGain.ToString();
			_xpRemaining = PlayerStatistic.instance.xpRemaining.ToString();



			_availableHat = "";
			_availableGlass = "";
			_availableClothes = "";
			_availableShoes = "";

			foreach(int li in PlayerStatistic.instance.availableHatIndex){
				_availableHat = _availableHat + li.ToString();
			}
			foreach(int li in PlayerStatistic.instance.availableGlassIndex){
				_availableGlass = _availableGlass + li.ToString();
			}
			foreach(int li in PlayerStatistic.instance.availableClothesIndex){
				_availableClothes = _availableClothes + li.ToString();
			}
			foreach(int li in PlayerStatistic.instance.availableShoesIndex){
				_availableShoes = _availableShoes + li.ToString();
			}

			//This connects to a server side php script that will add the name and score to a MySQL DB.
			// Supply it with a string representing the players name and the players score.
			string hash = MD5S.Md5Sum(_uniqueid + _username + _globalLevel + _xpGain + _xpRemaining
			                          + _availableHat + _availableGlass + _availableClothes + _availableShoes   + _credit + secretKey);

			string post_url = addStatsURL 
				+ "uniqueid=" +  _uniqueid
				+ "&username=" + WWW.EscapeURL(_username) 
				+ "&globallevel=" +  _globalLevel
				+ "&xpGain=" +  _xpGain
				+ "&xpremaining=" +  _xpRemaining
				+ "&availableHatIndex=" + WWW.EscapeURL(_availableHat)
				+ "&availableGlassIndex=" + WWW.EscapeURL(_availableGlass)
				+ "&availableClothesIndex=" + WWW.EscapeURL(_availableClothes)
				+ "&availableShoesIndex=" + WWW.EscapeURL(_availableShoes)
				+ "&credits=" + _credit
				+ "&hash=" + hash;

			Debug.Log (post_url);
			// Post the URL to the site and create a download object to get the result.
			WWW hs_post = new WWW(post_url);
			yield return hs_post; // Wait until the download is done
			Debug.Log ("PostSucceed");
			if (hs_post.error != null)
			{
				print("There was an error posting the high score: " + hs_post.error);
			}
		}else{

		}
		dataDownloaded = false;
	}


	public IEnumerator CheckExistingUser(int userid){
		WWW user_get = new WWW(getStatsURL + "uniqueid=" +  userid);
		yield return user_get;
		errorDownload = false;
		Debug.Log(getStatsURL +"uniquid=" + userid);
		if (user_get.error != null)
		{
			errorDownload = true;
			print("There was an error getting userdata " + user_get.error);
		}
		else
		{
			if(user_get.text == "Exist"){
				userExist = true;
			}else{
				userExist = false;
			}
		}

		dataDownloaded = true;
	}


	public IEnumerator GetUserData(int userid,string func){
		WWW user_get = new WWW(getDataURL + "uniqueid=" +  userid + "&f=" + func);
		yield return user_get;
		errorDownload = false;
		Debug.Log(getDataURL + "uniqueid=" +  userid + "&f=" + func);

		if (user_get.error != null)
		{
			errorDownload = true;
			print("There was an error getting the high score: " + user_get.error);
		}
		else
		{
			dataCount +=1;
			if(func == "GetName")
				PlayerStatistic.instance.username = user_get.text;
			if(func == "GetLevel")
				PlayerStatistic.instance.globalLevel =int.Parse(user_get.text);
			if(func == "GetXPGain")
				PlayerStatistic.instance.xpGain = int.Parse(user_get.text);
			if(func == "GetXPRemaining")
				PlayerStatistic.instance.xpRemaining = int.Parse(user_get.text);
			if(func == "GetHat")
			//	PlayerStatistic.instance.availableHatIndex = user_get.text;
			if(func == "GetGlass")
			//	PlayerStatistic.instance.availableGlassIndex = user_get.text;
			if(func == "GetClothes")
			//PlayerStatistic.instance.availableClothesIndex = user_get.text;
			if(func == "GetShoes")
			//	PlayerStatistic.instance.availableShoesIndex = user_get.text;
			if(func == "GetCredits")
				PlayerStatistic.instance.credit = int.Parse(user_get.text);
		}
		
		dataDownloaded = true;
		if(dataCount>=9){
			dataLoaded = true;
		}
	}





 
}