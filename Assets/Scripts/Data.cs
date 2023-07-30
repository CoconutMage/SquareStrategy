using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Data : MonoBehaviour
{
	//This data script is an absolute fucking mess, partly because its a work and progress, and partly because
	//I don't know what the fuck I'm doing and shouldn't be allowed anywhere near a serious data structure
	public static Data Instance { get; private set; }

	//these are static because otherwise it gets angry, if they can be made so they can not be static, that might be better but this works for now
	public static Player nullPlayer = new Player(-1, "Null", -1, nullCountry);
	//
	public static Country nullCountry = new Country(-1, "Null", "Null", nullLeader, new Dictionary<int, Tile>());
	public static Leader nullLeader = new Leader("Null", null, "Null");
	public static City nullCity = new City(-1, "Null", false, nullCountry, -1);

	public Tile nullTile = new(-1, -1, new Vector2(0, 0), "Null", nullCountry, nullCity, -1, -1, -1, -1, -1);

	public Dictionary<int, Chunk> map;

	public Dictionary<string, Country> countries;
	public Dictionary<string, City> cities;
	public Dictionary<string, Leader> leaders;

	public Dictionary<string, Sprite> leaderImages;

	public bool mapEditorScene;

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

		//Temp Data
		leaderImages["Stalin"] = Resources.Load<Sprite>("Pictures/Stalin");
		leaderImages["Wojtek"] = Resources.Load<Sprite>("Pictures/Wojtek");

		leaders["Dwight D. Eisenhower"] = new Leader("Dwight D. Eisenhower", leaderImages["Wojtek"], "Not Communist");
		leaders["Josef Stalin"] = new Leader("Josef Stalin", leaderImages["Stalin"], "Communist");


		Dictionary<int, Tile> americaTile = new Dictionary<int, Tile>();

		countries["USA"] = new Country(0, "USA", "The United States of America", leaders["Dwight D. Eisenhower"], americaTile);
	


		countries["USSR"] = new Country(1, "USSR", "The Union of Soviet Socialist Republics", leaders["Josef Stalin"], new Dictionary<int, Tile>());

		cities["WashingtonDC"] = new City(0, "Washington D.C.", true, countries["USA"], 689545);
		cities["New York"] = new City(1, "New York", false, countries["USA"], 689545);
		cities["Los Angeles"] = new City(1, "Los Angeles", false, countries["USA"], 689545);

		cities["Moscow"] = new City(2, "Moscow", true, countries["USSR"], 0);


		americaTile[0] = new Tile(0, 0, new Vector2(0, 0), "grass", countries["USA"], cities["WashingtonDC"], 0, 0, 0, 0, 0);
	}

	public void CreatePlayer(string playerName, Country playerCountry)
	{
		players[playerCreationIndex] = new Player(playerCreationIndex, playerName, 0, playerCountry);
		playerCreationIndex++;
	}

	public Player[] players = new Player[8];
	int playerCreationIndex;
	public struct Player
	{
		public int id;
		public string name;

		public int politicalPower;

		public Country country;

		public Player(int i, string aa, int pp, Country ab)
		{
			id = i;
			name = aa;

			politicalPower = pp;

			country = ab;
		}
	}

	//This struct also needs some method of storing the tiles a country owns as well as what cities it has and what tiles they're in
	//(if tiles store cities then a dict should suffice)
	//This struct has also been reworked to be much simpler and have less things that we might need in the future in favor of things we are currently using,
	//old version can be found if required for reference at the bottom of this file
	public struct Country
	{
		public int countryID;
		public string countryTag;
		public string countryName;

		public Leader countryLeader;

		public Dictionary<int, Tile> countryTiles;

		public Country(int id, string tag, string name, Leader ad, Dictionary<int, Tile> tiles)
		{
			countryID = id;
			countryTag = tag;
			countryName = name;

			countryLeader = ad;

			countryTiles = tiles;
		}
	}

	public struct City
	{
		public int cityId;
		public string cityName;

		public bool isCapital;

		public Country parentCountry;

		public int population;
		public Dictionary<string, int> cityBuildings;

		public City(int id, string name, bool cap, Country c, int ab, int r1 = 0, int r2 = 1, int r3 = 2, int r4 = 3)
		{
			cityId = id;
			cityName = name;

			isCapital = cap;

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

		public string politicalParty;
		public int termsServed;

		public Leader(string aa, Sprite image, string ac)
		{
			name = aa;
			leaderImage = image;

			politicalParty = ac;
			termsServed = 0;
		}
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

		public Country parentCountry;
		public City city;
		public Dictionary<string, int> tileResources;


		public Dictionary<string, int> tileBuildings;

		public Tile(int aa, int ab, Vector2 ac, string ad, Country pc, City ae, int r1, int r2, int r3, int r4, int r5)
		{
			//Tile Info
			chunkIndex = aa;
			index = ab;
			coordinates = ac;
			tileType = ad;

			//Game Info
			parentCountry = pc;
			city = ae;

			tileResources = new Dictionary<string, int> { { "Iron", r1 }, { "Uranium", r2 }, { "Oil", r3 }, { "Corn", r4 }, { "Wheat", r5 } };


			//Silos are tile based but can only have like 5 per chunk (perhaps you can only build 1 and you can upgrade it)
			//Airports have levels and store planes
			//Carriers work like airports
			//Planes have ranges from the tile if carrier/city or tile? they are stationed in
			//Since the map is hex-based, planes can have a radius where they can automatically operate, which makes airzones and ocean zones unnecessary
			//maybe factories are also tile buildings? I want some city buildings but im not sure exactly what
			tileBuildings = new Dictionary<string, int> { { "Missile Silos", r1 }, { "Forts", r2 }, { "Railroads", r3 }, { "Airport", r3 } };
			//you wouldnt build a missile silo in a city, cities would be targeted by missiles
			//forts need to be able to be built anywhere, along with railroads and airports

		}
	}
}










/*	Not in use at the moment / for future use
	public void CreatePlayerCountry(string name) //Leader leader)
	{
		countries[name] = new Country(0, name, name, nullLeader);
		Country c = countries[name];
		Debug.Log("NEW COUNTRY: ID: " + c.countryTag + ", NAME: " + c.countryName + ", LEADER NAME: " + c.leader.name);
	}
*/

/*	Code Graveyard

	//Used to be in the tile struct but should be unnecessary since units can most likely store their own positions
	public Dictionary<int, UnitData.ArmoredUnit> landArmoredUnits;
	public Dictionary<string, int> landUnits;
	public Dictionary<string, int> airUnits;

	landArmoredUnits = new Dictionary<int, UnitData.ArmoredUnit> { };
	landUnits = new Dictionary<string, int> { { "M1 Abrams Main Battle Tank", r1 }, { "Infantry", r2 }, { "Howitzer", r3 }, { "Railway Gun", r4 } };
	airUnits = new Dictionary<string, int> { { "Enemy AC130 Above!", r1 }, { "Fairchild Republic A-10 Thunderbolt II", r2 }, { "F-22", r3 }, { "F-35", r4 } };


	//Used to be in the water tile struct but should be unecessary for the reason above, which also mostly removes the need for a water tile in the first place,
	//as well as the water tile struct would have most likely been removed anyway
	{ "Zumwalt class guided missile destroyer", r1 },
	{ "Arleigh Burke class guided-missile destroyer", r2 },
	{ "Gerald R. Ford Class Aircraft Carrier: John F. Kennedy", r3 },
	{ "Iowa Class Battleship", r4 }


	//Simplified for ease of use, kept here for reference
	public struct Country
	{
		public int countryID;
		public string countryTag;
		public string name;
		//public MapData.City capitalCity;

		public long population;

		public Leader leader;

		public int armyUnitAmount;
		public int airForceUnitAmount;
		public int navalUnitAmount;

		//These will almost likely get reworked into structs as well
		public Dictionary<string, int> resources;
		public Dictionary<string, int> army;
		public Dictionary<string, int> airForce;
		public Dictionary<string, int> Navy;

		public Dictionary<string, int> tileBuildings;

		public Country(int ii, string aa, string ab, long ac, Leader ad)
		{
			countryID = ii;
			countryTag = aa;
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
*/