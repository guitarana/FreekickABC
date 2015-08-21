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

	public LinkedList<Accessories> linkedHats;
	public LinkedList<Accessories> linkedGlass;
	public LinkedList<Accessories> linkedShoes;
	public LinkedList<Accessories> linkedClothes;

	public Accessories[] hats ;
	public Accessories[] glass;
	public Accessories[] shoes;
	public Accessories[] clothes;

	public LinkedListNode<Accessories> currentHat;
	public LinkedListNode<Accessories> currentGlass;
	public LinkedListNode<Accessories> currentShoes;
	public LinkedListNode<Accessories> currentClothes;

	public int hatIndex = 100;
	public int glassIndex = 200;
	public int shoesIndex = 400;
	public int clothesIndex = 300;

	private int totalHat;
	private int totalGlasses;
	private int totalShoes;
	private int totalClothes;

	private AudioSource aud;
	public AudioClip sfx;
	// Use this for initialization
	void Start ()
	{

		linkedHats = new LinkedList<Accessories>();
		currentHat = new LinkedListNode<Accessories>(FindAccessories(100));
		linkedHats.AddFirst(currentHat);

		linkedGlass = new LinkedList<Accessories>();
		currentGlass = new LinkedListNode<Accessories>(FindAccessories(200));
		linkedGlass.AddFirst(currentGlass);

		linkedShoes = new LinkedList<Accessories>();
		currentShoes = new LinkedListNode<Accessories>(FindAccessories(400));
		linkedShoes.AddFirst(currentShoes);

		linkedClothes = new LinkedList<Accessories>();
		currentClothes = new LinkedListNode<Accessories>(FindAccessories(300));
		linkedClothes.AddFirst(currentClothes);

		aud = GetComponent<AudioSource>();
		sfx = aud.clip;
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
	
		AddListToLinkedList(accessories);

		currentHat = FindLinkedListNode(linkedHats,hatIndex);
		ActivateItem(currentHat.Value);

		currentGlass = FindLinkedListNode(linkedGlass,glassIndex);
		ActivateItem(currentGlass.Value);

		currentShoes = FindLinkedListNode(linkedShoes,shoesIndex);
		ActivateItem(currentShoes.Value);

		currentClothes = FindLinkedListNode(linkedClothes,clothesIndex);
		ActivateItem(currentClothes.Value);

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

		aud.PlayOneShot(sfx);
		Deactive(accessories,Type.Hat);
		if(currentHat == linkedHats.Last)
			currentHat = linkedHats.First;
		else
			currentHat = currentHat.Next;
	
		ActivateItem(currentHat.Value);
		PlayerStatistic.instance.hatIndex = currentHat.Value.index;

	}

	public void SelectHatPrev(){

		aud.PlayOneShot(sfx);
		Deactive(accessories,Type.Hat);
		if(currentHat == linkedHats.First)
			currentHat = linkedHats.Last;
		else
			currentHat = currentHat.Previous;

		ActivateItem(currentHat.Value);
		PlayerStatistic.instance.hatIndex = currentHat.Value.index;

	}

	public void SelectGlassNext(){
		
		aud.PlayOneShot(sfx);
		Deactive(accessories,Type.Glass);
		if(currentGlass == linkedGlass.Last)
			currentGlass = linkedGlass.First;
		else
			currentGlass = currentGlass.Next;
		
		ActivateItem(currentGlass.Value);
		PlayerStatistic.instance.glassIndex = currentGlass.Value.index;

	}
	
	public void SelectGlassPrev(){
		
		aud.PlayOneShot(sfx);
		Deactive(accessories,Type.Glass);
		if(currentGlass == linkedGlass.First)
			currentGlass = linkedGlass.Last;
		else
			currentGlass = currentGlass.Previous;
		
		ActivateItem(currentGlass.Value);
		PlayerStatistic.instance.glassIndex = currentGlass.Value.index;
		
	}

	public void SelectShoesNext(){
		
		aud.PlayOneShot(sfx);
		Deactive(accessories,Type.Shoes);
		if(currentShoes == linkedShoes.Last)
			currentShoes = linkedShoes.First;
		else
			currentShoes = currentShoes.Next;
		
		ActivateItem(currentShoes.Value);
		PlayerStatistic.instance.shoesIndex = currentShoes.Value.index;
		
	}
	
	public void SelectShoesPrev(){
		
		aud.PlayOneShot(sfx);
		Deactive(accessories,Type.Shoes);
		if(currentShoes == linkedShoes.First)
			currentShoes = linkedShoes.Last;
		else
			currentShoes = currentShoes.Previous;
		
		ActivateItem(currentShoes.Value);
		PlayerStatistic.instance.shoesIndex = currentShoes.Value.index;
		
	}

	public void SelectClothesNext(){
		
		aud.PlayOneShot(sfx);
		Deactive(accessories,Type.Clothes);
		if(currentClothes == linkedClothes.Last)
			currentClothes = linkedClothes.First;
		else
			currentClothes = currentClothes.Next;
		
		ActivateItem(currentClothes.Value);
		PlayerStatistic.instance.clothesIndex = currentClothes.Value.index;
		
	}
	
	public void SelectClothesPrev(){
		
		aud.PlayOneShot(sfx);
		Deactive(accessories,Type.Clothes);
		if(currentClothes == linkedClothes.First)
			currentClothes = linkedClothes.Last;
		else
			currentClothes = currentClothes.Previous;
		
		ActivateItem(currentClothes.Value);
		PlayerStatistic.instance.clothesIndex = currentClothes.Value.index;
		
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

	public void AddListToLinkedList(List<Accessories> list){
		foreach(Accessories acc in list){


			if(acc.index.ToString().StartsWith("1") && !acc.index.ToString().EndsWith("0")){
				if(PlayerStatistic.instance.availableHatIndex.Contains(acc.index))
					linkedHats.AddLast(acc);
			}
			if(acc.index.ToString().StartsWith("2") && !acc.index.ToString().EndsWith("0")){
				if(PlayerStatistic.instance.availableGlassIndex.Contains(acc.index))
					linkedGlass.AddLast(acc);
			}
			if(acc.index.ToString().StartsWith("3") && !acc.index.ToString().EndsWith("0")){
				if(PlayerStatistic.instance.availableClothesIndex.Contains(acc.index))
					linkedClothes.AddLast(acc);
			}
			if(acc.index.ToString().StartsWith("4") && !acc.index.ToString().EndsWith("0")){
				if(PlayerStatistic.instance.availableShoesIndex.Contains(acc.index))
					linkedShoes.AddLast(acc);
			}
		}
	}

	public LinkedListNode<Accessories> FindLinkedListNode(LinkedList<Accessories> acc,int index){
		return acc.Find(FindAccessories(index));
	}

	public void ActivateItem(PlayerAccessoriesList.Accessories acc){
		if(acc.go.GetComponent<SkinnedMeshRenderer>())
			acc.go.GetComponent<SkinnedMeshRenderer>().enabled = true;
		else
			acc.go.SetActive(true);
	}
}

