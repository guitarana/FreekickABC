using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAccessoriesList : MonoBehaviour
{

	public enum Type{
		Hat,
		Glass,
		Clothes,
		Shoes
	}

	[System.Serializable]
	public class Accessories{
		public string name;
		public GameObject go;
		public bool available;
		public Type type;
		public int index;
	}


	public List<Accessories> accessories= new List<Accessories>(1);

	public Accessories[] hats ;
	public Accessories[] glass;
	public Accessories[] shoes;
	public Accessories[] clothes;

	public Accessories currentHat;
	public Accessories currentGlass;
	public Accessories currentShoes;
	public Accessories currentClothes;

	public int hatIndex = 100;
	public int glassIndex = 200;
	public int shoesIndex = 400;
	public int clothesIndex = 300;

	private int totalHat;
	private int totalGlasses;
	private int totalShoes;
	private int totalClothes;
	// Use this for initialization
	void Start ()
	{
		AddHat();
		AddGlass();
		AddShoes();
		AddClothes();
		//Deactive(hats);
		//Deactive(glass);
		//Deactive(shoes);
		Deactive(accessories);
		hatIndex = PlayerStatistic.instance.hatIndex;
		glassIndex = PlayerStatistic.instance.glassIndex;
		shoesIndex = PlayerStatistic.instance.shoesIndex;
		clothesIndex = PlayerStatistic.instance.clothesIndex;

		currentHat = FindAccessories(hatIndex);
		currentHat.go.SetActive(true);

		currentGlass = FindAccessories(glassIndex);
		currentGlass.go.SetActive(true);

		currentShoes = FindAccessories(shoesIndex);
		if(currentShoes.go.GetComponent<SkinnedMeshRenderer>())
			currentShoes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
		else
			currentShoes.go.SetActive(true);

		currentClothes = FindAccessories(clothesIndex);
		if(currentClothes.go.GetComponent<SkinnedMeshRenderer>())
			currentClothes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
		else
			currentClothes.go.SetActive(true);

	}

	void SaveStat(){
		PlayerStatistic.instance.hatIndex   = hatIndex;
		PlayerStatistic.instance.glassIndex = glassIndex;
		PlayerStatistic.instance.shoesIndex = shoesIndex;
		PlayerStatistic.instance.clothesIndex = clothesIndex;
	}

	// Update is called once per frame
	void Update ()
	{
	
	}

	void Deactive(List<Accessories> acc){
		foreach(Accessories ac in acc){
			if(ac.go.GetComponent<SkinnedMeshRenderer>())
				ac.go.GetComponent<SkinnedMeshRenderer>().enabled = false;
			else
				ac.go.SetActive(false);
		}

	}

	void Deactive(List<Accessories> acc,Type type){
	
		foreach(Accessories ac in acc){
			if(type == ac.type){
				if(ac.go.GetComponent<SkinnedMeshRenderer>())
					ac.go.GetComponent<SkinnedMeshRenderer>().enabled = false;
				else
					ac.go.SetActive(false);
			}
		}

		
	}

	void ActiveAcc(){

		SaveStat();
	}

	public void SelectHatNext(){


		Deactive(accessories,Type.Hat);
		hatIndex += 1;

		if(hatIndex >= 100 + totalHat)
			hatIndex = 100;

		currentHat = FindAccessories(hatIndex);
		if(currentHat.available){
			currentHat.go.SetActive(true);
			ActiveAcc();
		}else{
			hatIndex += 1;
			SelectHatNext();
		}
	}

	public void SelectHatPrev(){


		Deactive(accessories,Type.Hat);
		hatIndex -= 1;

		if(hatIndex <100)
			hatIndex = 100 + totalHat-1;
		
		currentHat = FindAccessories(hatIndex);
		if(currentHat.available){
			currentHat.go.SetActive(true);
			ActiveAcc();
		}else{
			hatIndex -= 1;
			SelectHatPrev();
		}
		//}
	}

	public void SelectGlassNext(){
		
			
		Deactive(accessories,Type.Glass);
		glassIndex += 1;
		
		if(glassIndex >= 200 + totalGlasses)
			glassIndex = 200;
		
		currentGlass = FindAccessories(glassIndex);
		if(currentGlass.available){
			currentGlass.go.SetActive(true);
			ActiveAcc();
		}else{
			glassIndex += 1;
			SelectGlassNext();
		}
	}
	
	public void SelectGlassPrev(){
		

		Deactive(accessories,Type.Glass);
		glassIndex -= 1;
		
		if(glassIndex <200)
			glassIndex = 200 + totalGlasses-1;
			
		currentGlass = FindAccessories(glassIndex);
		if(currentGlass.available){
			currentGlass.go.SetActive(true);
			ActiveAcc();
		}else{
			glassIndex -= 1;
			SelectGlassPrev();
		}
	}

	public void SelectShoesNext(){
		
		Deactive(accessories,Type.Shoes);
		shoesIndex += 1;
		
		if(shoesIndex >= 400 + totalShoes)
			shoesIndex = 400;
		
		currentShoes = FindAccessories(shoesIndex);
		if(currentShoes.available){
			if(currentShoes.go.GetComponent<SkinnedMeshRenderer>())
				currentShoes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
			else
				currentShoes.go.SetActive(true);

			ActiveAcc();
		}else{
			shoesIndex += 1;
			SelectShoesNext();
		}
	}
	
	public void SelectShoesPrev(){
		

		Deactive(accessories,Type.Shoes);
		shoesIndex -= 1;
		
		if(shoesIndex <400)
			shoesIndex = 400 + totalShoes-1;
			
		currentShoes = FindAccessories(shoesIndex);
		if(currentShoes.available){
			if(currentShoes.go.GetComponent<SkinnedMeshRenderer>())
				currentShoes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
			else
				currentShoes.go.SetActive(true);
			
			ActiveAcc();
		}else{
			shoesIndex -= 1;
			SelectShoesPrev();
		}
	}

	public void SelectClothesNext(){

		Deactive(accessories,Type.Clothes);
		clothesIndex += 1;
		
		if(clothesIndex >= 300 +totalClothes)
			clothesIndex = 300;
		
		currentClothes = FindAccessories(clothesIndex);
		if(currentClothes.available){
			if(currentClothes.go.GetComponent<SkinnedMeshRenderer>())
				currentClothes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
			else
				currentClothes.go.SetActive(true);
			
			ActiveAcc();
		}else{
			clothesIndex += 1;
			SelectClothesNext();
		}
	}
	
	public void SelectClothesPrev(){
		
		Deactive(accessories,Type.Clothes);
		clothesIndex -= 1;
		
		if(clothesIndex <300)
			clothesIndex = 300 +totalClothes-1;
		
		currentClothes = FindAccessories(clothesIndex);
		if(currentClothes.available){
			if(currentClothes.go.GetComponent<SkinnedMeshRenderer>())
				currentClothes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
			else
				currentClothes.go.SetActive(true);
			
			ActiveAcc();
		}else{
			clothesIndex -= 1;
			SelectClothesPrev();
		}
	}



	int counter;
	void AddHat(){
		counter = 100;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Hat){
				totalHat +=1;
				ac.available = false;
				FindAccessories(100).available = true;
				if(PlayerStatistic.instance.availableHatIndex.Contains(counter)){
					FindAccessories(counter).available = true;
				}
				counter +=1;
			}
		}

	}

	void AddGlass(){
		counter = 200;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Glass){
				totalGlasses +=1;
				ac.available = false;
				FindAccessories(200).available = true;
				if(PlayerStatistic.instance.availableGlassIndex.Contains(counter)){
					FindAccessories(counter).available = true;
				}
				counter +=1;
			}
		}
	}

	void AddShoes(){
		counter = 400;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Shoes){
				totalShoes +=1;
				ac.available = false;
				FindAccessories(400).available = true;
				if(PlayerStatistic.instance.availableShoesIndex.Contains(counter)){
					FindAccessories(counter).available = true;
				}
				counter +=1;
			}
		}
	}

	void AddClothes(){
		counter = 300;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Clothes){
				totalClothes +=1;
				ac.available = false;
				FindAccessories(300).available = true;
				if(PlayerStatistic.instance.availableClothesIndex.Contains(counter)){
					FindAccessories(counter).available = true;
				}
				counter +=1;
			}
		}
	}

	public Accessories FindAccessories(int index)
	{
		List<Accessories> list = accessories;
		Accessories acc = list.Find(
			delegate(Accessories ac)
			{
			return ac.index == index;
		}
		);
		if (acc != null)
		{
			return acc;
		}
		else
		{
			return null;
		}
		return null;
	}
}

