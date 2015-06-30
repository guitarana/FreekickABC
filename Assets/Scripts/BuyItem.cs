
using UnityEngine;
using System.Collections;

public class BuyItem : MonoBehaviour
{

	public int index;
	public int price;
	public GameObject blockerPanel;

	// Use this for initialization
	void Start ()
	{
		if (index <200){
			if(PlayerStatistic.instance.availableHatIndex.Contains(index)){
				blockerPanel.SetActive(false);
			}
		}else
		if (index <300 && index >=200){
			if(PlayerStatistic.instance.availableGlassIndex.Contains(index)){
				blockerPanel.SetActive(false);
			}
		}else
		if (index <400 && index >=300){
			if(PlayerStatistic.instance.availableClothesIndex.Contains(index)){
				blockerPanel.SetActive(false);
			}
		}else
		if (index >400){
			if(PlayerStatistic.instance.availableShoesIndex.Contains(index)){
				blockerPanel.SetActive(false);
			}
		}
	}
	
	public void Buy(){
		ShopManager.instance.BuyItem(index,price);
	}
}

