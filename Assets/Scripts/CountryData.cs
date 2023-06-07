using System.Collections.Generic;
using UnityEngine;
using static MapData;

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

		Countries = new Dictionary<int, Country>();
	}

	/*
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
	*/

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

		public int armyUnitAmount;
		public int airForceUnitAmount;
		public int navalUnitAmount;

		public Dictionary<string, int> resources;
		public Dictionary<string, int> army;
		public Dictionary<string, int> airForce;
		public Dictionary<string, int> Navy;

		public Dictionary<string, int> tileBuildings;

		public static Country nullCountry = new Country(-1, "Null", -1, nullLeader);
		public static Leader nullLeader = new Leader("Null", "Null", 0, "Null");

		public static Country USA = new Country(0, "The United States of America", 333287557, new Leader("John Moses Browning", "God of Guns", 99999, "More Gun"));

		public Country(int aa, string ab, long ac, Leader ad)
		{
			countryID = aa;
			name = ab;
			population = ac;

			leader = ad;

			armyUnitAmount = 0;
			airForceUnitAmount = 0;
			//Tonnage?
			navalUnitAmount = 0;

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
			foreach (var unit in army) armyUnitAmount += unit.Value;
			foreach (var unit in airForce) airForceUnitAmount += unit.Value;
			foreach (var unit in Navy) navalUnitAmount += unit.Value;
		}
	}

	public struct City
	{
		public string name;

		public CountryData.Country parentCountry;

		public int population;
		public Dictionary<string, int> cityBuildings;

		public City(string aa, Country c, int ab, int r1 = 0, int r2 = 1, int r3 = 2, int r4 = 3)
		{
			name = aa;

			parentCountry = c;

			population = ab;

			cityBuildings = new Dictionary<string, int>
			{
				{ "Civilian Factories", r1 },
				{ "Military Factories", r2 },
				{ "Dockyards", r3 },
				{ "Infrastructure", r4 }
			};
		}
	}

	public static City WashingtonDC = new City("Washington D.C.", Country.USA, 689545);

	public static City nullCity = new City("Null", Country.nullCountry, -1);

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

	public struct Government
	{
		public string name;

		public Government(string aa)
		{
			name = aa;
		}
	}
}