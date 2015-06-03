using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAccessories : MonoBehaviour
{
	public enum Hat{
		Koboi,
		Kupluk,
		Sunda,
		Gaul1,
		Gaul2
	}

	public enum Glass{
		Centil,
		Casual,
		Gaya
	}

	public enum Shoes{
		Boots,
		HighBoots
	}

	public class Accessories{
		string name;
		GameObject go;
		bool available;
	}

	public List<Accessories> accessories;

	// Use this for initialization
	void Start ()
	{
		accessories = new List<Accessories>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

