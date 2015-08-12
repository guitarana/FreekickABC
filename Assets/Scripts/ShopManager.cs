using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour {

	public static ShopManager instance;
	public GameObject table;
	public GameObject popUp;
	public GameObject popUpItemName;
	public GameObject popUpItemPict;
	public GameObject popUpAlert;
	public GameObject popUpYes;
	public GameObject popUpNo;
	public AudioClip sfx;
	public AudioClip sfx2;

	public BuyItem currentItem;


	// Use this for initialization
	void Start () {
		instance = this;
		popUp.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		table.SetActive(!popUp.activeSelf);
	}

	public void ShowPopUp(){
		gameObject.GetComponent<AudioSource>().PlayOneShot(sfx);
		if(PlayerStatistic.instance.availableClothesIndex.Contains(currentItem.index)) return;
		if(PlayerStatistic.instance.availableHatIndex.Contains(currentItem.index)) return;
		if(PlayerStatistic.instance.availableGlassIndex.Contains(currentItem.index)) return;
		if(PlayerStatistic.instance.availableShoesIndex.Contains(currentItem.index)) return;

		popUpItemName.GetComponent<UILabel>().text = currentItem.itemName;
		popUpItemPict.GetComponent<UISprite>().SetSprite(currentItem.transform.GetChild(2).GetChild(0).GetComponent<UISprite>().GetAtlasSprite());
		popUp.SetActive(true);
	}

	public void Buy(){
		gameObject.GetComponent<AudioSource>().PlayOneShot(sfx);
		BuyItem(currentItem.index,currentItem.price);
	}

	public void Cancel(){
		gameObject.GetComponent<AudioSource>().PlayOneShot(sfx);
		popUp.SetActive(false);
	}

	IEnumerator CloseAlert(){
		gameObject.GetComponent<AudioSource>().PlayOneShot(sfx);
		yield return new WaitForSeconds(2);
		popUpAlert.SetActive(false);
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
			currentItem.UpdateStat();
			popUp.SetActive(false);
			PlayerStatistic.instance.credit -= currentPrice;
			gameObject.GetComponent<AudioSource>().PlayOneShot(sfx2);
			CloudDataController.instance.SetStat();

		}else{
			popUpAlert.SetActive(true);
			StartCoroutine(CloseAlert());
		}
	}
}
