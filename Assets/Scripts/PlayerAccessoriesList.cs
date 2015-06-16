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

	public int hatIndex = 5;
	public int glassIndex = 3;
	public int shoesIndex = 3;
	public int clothesIndex = 2;
	// Use this for initialization
	void Start ()
	{
		AddHat();
		AddGlass();
		AddShoes();
		AddClothes();
		Deactive(hats);
		Deactive(glass);
		Deactive(shoes);
		Deactive(clothes);
		hatIndex = PlayerStatistic.instance.hatIndex;
		glassIndex = PlayerStatistic.instance.glassIndex;
		shoesIndex = PlayerStatistic.instance.shoesIndex;
		clothesIndex = PlayerStatistic.instance.clothesIndex;

		currentHat = hats[hatIndex];
		currentHat.go.SetActive(true);

		currentGlass = glass[glassIndex];
		currentGlass.go.SetActive(true);

		currentShoes = shoes[shoesIndex];
		if(currentShoes.go.GetComponent<SkinnedMeshRenderer>())
			currentShoes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
		else
			currentShoes.go.SetActive(true);

		currentClothes = clothes[clothesIndex];
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

	void Deactive(PlayerAccessoriesList.Accessories[] acc){
		if(acc.Length == 0) return;
		Debug.Log("I : "+ acc.Length);
		for(int i=0;i<acc.Length;i++){
			if(acc[i].go.GetComponent<SkinnedMeshRenderer>())
				acc[i].go.GetComponent<SkinnedMeshRenderer>().enabled = false;
			else
				acc[i].go.SetActive(false);
		}

	}

	void ActiveAcc(){

		SaveStat();
	}

	public void SelectHatNext(){

		if(hats.Length ==0) return;

		Deactive(hats);
		hatIndex += 1;

		//if(hatIndex != 0){
		if(hatIndex >= hats.Length)
			hatIndex = 0;

		currentHat = hats[hatIndex];
		if(currentHat.available){
			currentHat.go.SetActive(true);
			ActiveAcc();
		}else{
			hatIndex += 1;
		}
		//}
	}

	public void SelectHatPrev(){

		if(hats.Length ==0) return;

		Deactive(hats);
		hatIndex -= 1;

		//if(hatIndex != 0){
		if(hatIndex <0)
			hatIndex = hats.Length-1;
		
		currentHat = hats[hatIndex];
		if(currentHat.available){
			currentHat.go.SetActive(true);
			ActiveAcc();
		}else{
			hatIndex -= 1;
		}
		//}
	}

	public void SelectGlassNext(){
		
		if(glass.Length ==0) return;
		
		Deactive(glass);
		glassIndex += 1;
		
		//if(glassIndex != 0){
		if(glassIndex >= glass.Length)
			glassIndex = 0;
		
		currentGlass = glass[glassIndex];
		if(currentGlass.available){
			currentGlass.go.SetActive(true);
			ActiveAcc();
		}else{
			glassIndex += 1;
		}
		//}
	}
	
	public void SelectGlassPrev(){
		
		if(glass.Length ==0) return;
		
		Deactive(glass);
		glassIndex -= 1;
		
		//if(glassIndex != 0){
		if(glassIndex <0)
			glassIndex = glass.Length-1;
			
		currentGlass = glass[glassIndex];
		if(currentGlass.available){
			currentGlass.go.SetActive(true);
			ActiveAcc();
		}else{
			glassIndex -= 1;
		}
		//}
	}

	public void SelectShoesNext(){
		
		if(shoes.Length ==0) return;
		
		Deactive(shoes);
		shoesIndex += 1;
		
		//if(shoesIndex != 0){
		if(shoesIndex >= shoes.Length)
			shoesIndex = 0;
		
		currentShoes = shoes[shoesIndex];
		if(currentShoes.available){
			if(currentShoes.go.GetComponent<SkinnedMeshRenderer>())
				currentShoes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
			else
				currentShoes.go.SetActive(true);

			ActiveAcc();
		}else{
			shoesIndex += 1;
		}
		//}
	}
	
	public void SelectShoesPrev(){
		
		if(shoes.Length ==0) return;
		
		Deactive(shoes);
		shoesIndex -= 1;
		
		//if(shoesIndex != 0){
		if(shoesIndex <0)
			shoesIndex = shoes.Length-1;
			
		currentShoes = shoes[shoesIndex];
		if(currentShoes.available){
			if(currentShoes.go.GetComponent<SkinnedMeshRenderer>())
				currentShoes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
			else
				currentShoes.go.SetActive(true);
			
			ActiveAcc();
		}else{
			shoesIndex -= 1;
		}
		//}
	}

	public void SelectClothesNext(){
		
		if(clothes.Length ==0) return;
		
		Deactive(clothes);
		clothesIndex += 1;
		
		//if(shoesIndex != 0){
		if(clothesIndex >= clothes.Length)
			clothesIndex = 0;
		
		currentClothes = clothes[clothesIndex];
		if(currentClothes.available){
			if(currentClothes.go.GetComponent<SkinnedMeshRenderer>())
				currentClothes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
			else
				currentClothes.go.SetActive(true);
			
			ActiveAcc();
		}else{
			clothesIndex += 1;
		}
		//}
	}
	
	public void SelectClothesPrev(){
		
		if(clothes.Length ==0) return;
		
		Deactive(clothes);
		clothesIndex -= 1;
		
		//if(shoesIndex != 0){
		if(clothesIndex <0)
			clothesIndex = clothes.Length-1;
		
		currentClothes = clothes[clothesIndex];
		if(currentClothes.available){
			if(currentClothes.go.GetComponent<SkinnedMeshRenderer>())
				currentClothes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
			else
				currentClothes.go.SetActive(true);
			
			ActiveAcc();
		}else{
			clothesIndex -= 1;
		}
		//}
	}



	int counter;
	void AddHat(){
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Hat){
				//if(ac.available){
					counter +=1;
				//}
			}
		}
		hats = new Accessories[counter];
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Hat){
				//if(ac.available){
				hats[counter] = ac;
				hats[counter].available = false;
				if(PlayerStatistic.instance.availableHatIndex.Contains(counter)){
					hats[counter].available = true;
				}
				counter +=1;
				//}
			}
		}

	}

	void AddGlass(){
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Glass){
				//if(ac.available){
					counter +=1;
				//}
			}
		}
		glass = new Accessories[counter];
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Glass){
				//if(ac.available){
				glass[counter] = ac;
				glass[counter].available = false;
				if(PlayerStatistic.instance.availableGlassIndex.Contains(counter)){
					glass[counter].available = true;
				}
				counter +=1;
				//}
			}
		}
	}

	void AddShoes(){
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Shoes){
				//if(ac.available){
					counter +=1;
				//}
			}
		}
		shoes = new Accessories[counter];
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Shoes){
				//if(ac.available){
				shoes[counter] = ac;
				shoes[counter].available = false;
				if(PlayerStatistic.instance.availableShoesIndex.Contains(counter)){
					shoes[counter].available = true;
				}
				counter +=1;
				//}
			}
		}
	}

	void AddClothes(){
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Clothes){
				//if(ac.available){
				counter +=1;
				//}
			}
		}
		clothes = new Accessories[counter];
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Clothes){
				//if(ac.available){
				clothes[counter] = ac;
				clothes[counter].available = false;
				if(PlayerStatistic.instance.availableClothesIndex.Contains(counter)){
					clothes[counter].available = true;
				}
				counter +=1;
				//}
			}
		}
	}
}

