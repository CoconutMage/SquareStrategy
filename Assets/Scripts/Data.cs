using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
	public static Data Instance { get; private set; }

	public static Country nullCountry = new Country(-1, "Null", -1, nullLeader);
	public static Leader nullLeader = new Leader("Null", null, "Null", -1, "Null");
	public static City nullCity = new City("Null", nullCountry, -1);

	public Tile nullTile = new(-1, -1, new Vector2(0, 0), "Null", nullCity, -1, -1, -1, -1, -1);
	public Tile nullWaterTile = new(-1, -1, new Vector2(0, 0), "Null", nullCity, -1, -1, -1, -1, -1);

	public Dictionary<int, Chunk> map;

	public Dictionary<string, Country> countries;
	public Dictionary<string, City> cities;
	public Dictionary<string, Leader> leaders;

	public Dictionary<string, Sprite> leaderImages;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		map = new Dictionary<int, Chunk>();


		countries = new Dictionary<string, Country>();
		leaders = new Dictionary<string, Leader>();
		cities = new Dictionary<string, City>();

		leaderImages = new Dictionary<string, Sprite>();
	}

	void Start()
	{
		leaderImages["Stalin"] = Resources.Load<Sprite>("Pictures/Stalin");
		leaderImages["Wojtek"] = Resources.Load<Sprite>("Pictures/Wojtek");

		leaders["John Moses Browning"] = new Leader("John Moses Browning", leaderImages["Wojtek"], "God of Guns", 99999, "More Gun");
		leaders["Stalin"] = new Leader("Stalin", leaderImages["Stalin"], "The King of Starvation", 0, "Communist");

		countries["USA"] = new Country(0, "The United States of America", 333287557, leaders["John Moses Browning"]);
		countries["USSR"] = new Country(0, "USSR", 0, leaders["Stalin"]);

		cities["Moscow"] = new City("Moscow", countries["USSR"], 0);
		cities["WashingtonDC"] = new City("Washington D.C.", countries["USA"], 689545);
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

		public Country parentCountry;

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


	public struct Leader
	{
		public string name;

		public Sprite leaderImage;

		public string title;

		public int politicalPower;

		public string politicalParty;
		public int termsServed;

		public Leader(string aa, Sprite image, string ab, int pp, string ac)
		{
			name = aa;
			leaderImage = image;
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



	public void PopulateChunkData(int chunkIndex, Vector2 location)
	{
		map[chunkIndex] = new Chunk
		(
			chunkIndex,
			location,
			new Dictionary<int, Tile>()
		);
	}

	public void PopulateChunkTileData(int r, int chunkIndex, int i, Vector2 pos, string material)
	{
		if (r == 0 || r == 2)
		{
			map[chunkIndex].tiles[i] = new Tile(chunkIndex, i, pos, material, cities["Moscow"],
				Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)
			);
		}
		else if (r == 1)
		{
			map[chunkIndex].tiles[i] = new Tile(chunkIndex, i, pos, material, cities["WashingtonDC"],
				Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)
			);

		}
		/*
		else if (r == 2)
		{
			map[chunkIndex].tiles[i] = new WaterTile(chunkIndex, i, pos, material,
				Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)
			);

		}
		*/
	}

	public struct Chunk
	{
		public Vector2 location;
		public int index;

		public Dictionary<int, Tile> tiles;

		public Chunk(int aa, Vector2 ab, Dictionary<int, Tile> ac)
		{
			index = aa;
			location = ab;

			tiles = ac;
		}
	}

	public struct Tile
	{
		public int chunkIndex;
		public int index;
		public Vector2 coordinates;
		public string tileType;

		public City city;
		public Dictionary<string, int> resources;

		public Dictionary<int, UnitData.ArmoredUnit> landArmoredUnits;
		public Dictionary<string, int> landUnits;
		public Dictionary<string, int> airUnits;

		public Dictionary<string, int> tileBuildings;

		public Tile(int aa, int ab, Vector2 ac, string ad, City ae, int r1, int r2, int r3, int r4, int r5)
		{
			#region Immutable Data
			//Tile Info
			chunkIndex = aa;
			index = ab;
			coordinates = ac;
			tileType = ad;

			//Game Info
			city = ae;

			resources = new Dictionary<string, int> { { "Iron", r1 }, { "Uranium", r2 }, { "Oil", r3 }, { "Corn" , r4 }, { "Wheat", r5 } };

			#endregion

			#region Mutable
			landArmoredUnits = new Dictionary<int, UnitData.ArmoredUnit> { };
			landUnits = new Dictionary<string, int> { { "M1 Abrams Main Battle Tank", r1 }, { "Infantry", r2 }, { "Howitzer", r3 }, { "Railway Gun", r4 } };
			airUnits = new Dictionary<string, int> { { "Enemy AC130 Above!", r1 }, { "Fairchild Republic A-10 Thunderbolt II", r2 }, { "F-22", r3 }, { "F-35", r4 } };

			//Silos are tile based but can only have like 5 per chunk
			//Airports have levels and store planes
			//Carriers work like airports
			//Planes have ranges from the tile if carrier/city or tile? they are stationed in
			//maybe airports are tile based?
			//maybe factories are also tile buildings? I want some city buildings but im not sure exactly what
			tileBuildings = new Dictionary<string, int> { { "Missile Silos", r1 }, { "Forts", r2 }, { "Railroads", r3 }, { "Airport", r3 } };

			#endregion
		}
	}

	public struct WaterTile
	{
		public int chunkIndex;
		public int index;
		public Vector2 coordinates;
		public string material;

		public Dictionary<string, int> resources;

		public Dictionary<string, int> navalUnits;

		public WaterTile(int aa, int ab, Vector2 ac, string ad, int r1, int r2, int r3, int r4, int r5)
		{
			#region Immutable Data
			//Tile Info
			chunkIndex = aa;
			index = ab;
			coordinates = ac;
			material = ad;

			//Game Info
			resources = new Dictionary<string, int> 
			{ 
				{ "Fish", r1 }, 
				{ "Oil", r2 }, 
				{ "Water Corn", r3 }
			};

			#endregion

			#region Mutable
			navalUnits = new Dictionary<string, int>
			{

				{ "Zumwalt class guided missile destroyer", r1 },
				{ "Arleigh Burke class guided-missile destroyer", r2 },
				{ "Gerald R. Ford Class Aircraft Carrier: John F. Kennedy", r3 },
				{ "Iowa Class Battleship", r4 }
			};

			#endregion
		}
	}
}