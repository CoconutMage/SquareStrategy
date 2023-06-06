using System.Collections.Generic;
using UnityEngine;

public class CountryData : MonoBehaviour
{
	public static CountryData Instance { get; private set; }
	public Dictionary<int, Country> Countries;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;
	}

	void Start()
	{
		CreateCountry
		(
			0,
			"The United States of America", 
			333287557,
			new Leader("John Moses Browning", "God of Guns", 99999, "More Gun")
		);
	}

	void CreateCountry(int id, string name, long pop, Leader lead)
	{
		Countries[id] = new Country
		(
			id,
			name,
			pop,
			lead
		);
	}

	public struct Country
	{
		public int countryID;
		public string name;
		//public MapData.City capitalCity;

		public long population;

		public Leader leader;

		public int landUnitAmount;
		public int airUnitAmount;
		public int waterUnitAmount;

		public Dictionary<string, int> resources;
		public Dictionary<string, int> army;
		public Dictionary<string, int> airForce;
		public Dictionary<string, int> Navy;

		public Dictionary<string, int> tileBuildings;

		public Country(int aa, string ab, long ac, Leader ad)
		{
			countryID = aa;
			name = ab;
			population = ac;

			leader = ad;

			landUnitAmount = 0;
			airUnitAmount = 0;
			//Tonnage?
			waterUnitAmount = 0;

			resources = new Dictionary<string, int> { { "Iron", 9 }, { "Uranium", 9 }, { "Oil", 9 }, { "Corn", 9 }, { "Wheat", 9 } };
			army = new Dictionary<string, int> { { "M1 Abrams Main Battle Tank", 9 }, { "Infantry", 9 }, { "Howitzer", 9 }, { "Railway Gun", 9 } };
			airForce = new Dictionary<string, int> { { "Enemy AC130 Above!", 9 }, { "Fairchild Republic A-10 Thunderbolt II", 9 }, { "F-22", 9 }, { "F-35", 9 } };
			Navy = new Dictionary<string, int>{ { "Zumwalt class guided missile destroyer", 9 }, { "Arleigh Burke class guided-missile destroyer", 9 }, 
				{ "Gerald R. Ford Class Aircraft Carrier: John F. Kennedy", 9 }, { "Iowa Class Battleship", 9 } };

			tileBuildings = new Dictionary<string, int> { { "Missile Silos", 9 }, { "Forts", 9 }, { "Railroads", 9 }, { "Airport", 9 } };

			UnitMath();
		}

		void UnitMath()
		{
			foreach (var unit in army) landUnitAmount += unit.Value;
			foreach (var unit in airForce) airUnitAmount += unit.Value;
			foreach (var unit in Navy) waterUnitAmount += unit.Value;
		}
	}

	public struct Leader
	{
		public string name;
		public string title;

		public int politicalPower;

		public string politicalParty;
		public int termsServed;

		public Leader(string aa, string ab, int pp, string ac)
		{
			name = aa;
			title = ab;

			politicalPower = pp;

			politicalParty = ac;
			termsServed = 0;
		}
	}
}