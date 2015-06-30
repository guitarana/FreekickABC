using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour {

	public static ShopManager instance;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void BuyItem(int index,int currentPrice){
		if(PlayerStatistic.instance.credit>= currentPrice){

			if (index <200){
				PlayerStatistic.instance.availableHatIndex.Add(index);
			}else
			if (index <300 && index >=200){
				PlayerStatistic.instance.availableGlassIndex.Add(index);
			}else
			if (index <400 && index >=300){
				PlayerStatistic.instance.availableClothesIndex.Add(index);
			}else
			if (index >400){
				PlayerStatistic.instance.availableShoesIndex.Add(index);
			}

		}
	}
}
