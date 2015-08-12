
using UnityEngine;
using System.Collections;

public class BuyItem : MonoBehaviour
{
	public string itemName;
	public int index;
	public int price;
	public GameObject blockerPanel;
	public GameObject yesPanel;
	public GameObject pricePanel;

	void Start(){
		yesPanel = transform.GetChild(6).gameObject;
		pricePanel = transform.GetChild(1).gameObject;
		UpdateStat();
	}

	// Use this for initialization
	public void UpdateStat ()
	{
		yesPanel.SetActive(false);
		pricePanel.SetActive(true);
		pricePanel.GetComponent<UILabel>().text = price.ToString();

		if (index <200){
			if(PlayerStatistic.instance.availableHatIndex.Contains(index)){
				blockerPanel.SetActive(false);
				yesPanel.SetActive(true);
				pricePanel.SetActive(false);
			}
		}else
		if (index <300 && index >=200){
			if(PlayerStatistic.instance.availableGlassIndex.Contains(index)){
				blockerPanel.SetActive(false);
				yesPanel.SetActive(true);
				pricePanel.SetActive(false);
			}
		}else
		if (index <400 && index >=300){
			if(PlayerStatistic.instance.availableClothesIndex.Contains(index)){
				blockerPanel.SetActive(false);
				yesPanel.SetActive(true);
				pricePanel.SetActive(false);
			}
		}else
		if (index >400){
			if(PlayerStatistic.instance.availableShoesIndex.Contains(index)){
				blockerPanel.SetActive(false);
				yesPanel.SetActive(true);
				pricePanel.SetActive(false);
			}
		}
		transform.GetChild(0).GetComponent<UILabel>().text = itemName;
	}
	
	public void Buy(){
		//ShopManager.instance.BuyItem(index,price);
		ShopManager.instance.currentItem = this;
		ShopManager.instance.ShowPopUp();
	}
}

