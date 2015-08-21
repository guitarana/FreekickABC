using UnityEngine;
using System.Collections;

public class PlayerAccessories : MonoBehaviour
{

	public int hatIndex;
	public int glassIndex;
	public int shoesIndex;
	public int clothesIndex;
	public string hatName;
	public string glassName;
	public string shoesName;
	public string clothesName;
	public PlayerAccessoriesList pl;

	// Use this for initialization
	void Start ()
	{
		pl=GetComponent<PlayerAccessoriesList>();
	}

	// Update is called once per frame
	void Update ()
	{
		hatIndex = PlayerStatistic.instance.hatIndex;
		glassIndex = PlayerStatistic.instance.glassIndex;
		shoesIndex = PlayerStatistic.instance.shoesIndex;
		clothesIndex = PlayerStatistic.instance.clothesIndex;
		if(pl.currentHat!=null)
			hatName = pl.currentHat.Value.name.ToString();
		if(pl.currentGlass!=null)
			glassName = pl.currentGlass.Value.name.ToString();
		if(pl.currentShoes!=null)
			shoesName = pl.currentShoes.Value.name.ToString();
		if(pl.currentShoes!=null)
			clothesName = pl.currentClothes.Value.name.ToString();
	}
	
}

