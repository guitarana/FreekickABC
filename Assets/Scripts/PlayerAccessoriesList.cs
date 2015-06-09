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

	public Accessories currentHat;
	public Accessories currentGlass;
	public Accessories currentShoes;

	public int hatIndex = 5;
	public int glassIndex = 3;
	public int shoesIndex = 3;

	// Use this for initialization
	void Start ()
	{
		AddHat();
		AddGlass();
		AddShoes();
		Deactive(hats);
		Deactive(glass);
		Deactive(shoes);
		hatIndex = PlayerStatistic.instance.hatIndex;
		glassIndex = PlayerStatistic.instance.glassIndex;
		shoesIndex = PlayerStatistic.instance.shoesIndex;
		currentHat = hats[hatIndex];
		currentHat.go.SetActive(true);
		currentGlass = glass[glassIndex];
		currentGlass.go.SetActive(true);
		currentShoes = shoes[shoesIndex];
		if(currentShoes.go.GetComponent<SkinnedMeshRenderer>())
			currentShoes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
		else
			currentShoes.go.SetActive(true);
	}

	void SaveStat(){
		PlayerStatistic.instance.hatIndex   = hatIndex;
		PlayerStatistic.instance.glassIndex = glassIndex;
		PlayerStatistic.instance.shoesIndex = shoesIndex;
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
			currentHat.go.SetActive(true);
			ActiveAcc();
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
			currentHat.go.SetActive(true);
			ActiveAcc();
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
			currentGlass.go.SetActive(true);
			ActiveAcc();
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
			currentGlass.go.SetActive(true);
			ActiveAcc();
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
			if(currentShoes.go.GetComponent<SkinnedMeshRenderer>())
				currentShoes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
			else
				currentShoes.go.SetActive(true);

			ActiveAcc();
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
			if(currentShoes.go.GetComponent<SkinnedMeshRenderer>())
				currentShoes.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
			else
				currentShoes.go.SetActive(true);
			ActiveAcc();
		//}
	}



	int counter;
	void AddHat(){
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Hat){
				if(ac.available){
					counter +=1;
				}
			}
		}
		hats = new Accessories[counter];
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Hat){
				if(ac.available){
					hats[counter] = ac;
					counter +=1;
				}
			}
		}

	}

	void AddGlass(){
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Glass){
				if(ac.available){
					counter +=1;
				}
			}
		}
		glass = new Accessories[counter];
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Glass){
				if(ac.available){
					glass[counter] = ac;
					counter +=1;
				}
			}
		}
	}

	void AddShoes(){
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Shoes){
				if(ac.available){
					counter +=1;
				}
			}
		}
		shoes = new Accessories[counter];
		counter = 0;
		foreach(Accessories ac in accessories){
			if(ac.type == Type.Shoes){
				if(ac.available){
					shoes[counter] = ac;
					counter +=1;
				}
			}
		}
	}
}

