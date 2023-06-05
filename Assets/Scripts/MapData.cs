using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
	public static MapData Instance { get; private set; }
	public Dictionary<int, Chunk> Map;

	private void Start()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		Map = new Dictionary<int, Chunk>();
	}



	public void PopulateChunkData(int chunkIndex, Vector2 location)
	{
		Map[chunkIndex] = new Chunk
		(
			chunkIndex,
			location,
			new Dictionary<int, Tile>()
		);
	}

	public void PopulateChunkTileData(int chunkIndex, int i, Vector2 pos, string material)
	{
		Map[chunkIndex].tiles[i] = new Tile(
			chunkIndex, 
			i, 
			pos, 
			material,
			false,

			Random.Range(0,10), Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10)
		);
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
		public string material;

		public bool city;
		public Dictionary<string, int> resources;

		public Dictionary<string, int> landUnits;
		public Dictionary<string, int> airUnits;

		public Dictionary<string, int> tileBuildings;

		public Tile(int aa, int ab, Vector2 ac, string ad, bool ae, int r1, int r2, int r3, int r4, int r5)
		{
			#region Immutable Data
			//Tile Info
			chunkIndex = aa;
			index = ab;
			coordinates = ac;
			material = ad;

			//Game Info
			city = ae;

			resources = new Dictionary<string, int> { { "Iron", r1 }, { "Uranium", r2 }, { "Oil", r3 }, { "Corn" , r4 }, { "Wheat", r5 } };

			#endregion

			#region Mutable
			landUnits = new Dictionary<string, int> { { "M1 Abrams Main Battle Tank", r1 }, { "Infantry", r2 }, { "Howitzer", r3 }, { "Railway Gun", r4 } };
			airUnits = new Dictionary<string, int> { { "Enemy AC130 Above!", r1 }, { "Fairchild Republic A-10 Thunderbolt II", r2 }, { "F-22", r3 }, { "F-35", r4 } };

			//Silos are tile based but can only have like 5 per chunk
			tileBuildings = new Dictionary<string, int> { { "Missile Silos", r1 }, { "Forts", r2 }, { "Railroads", r3 } };

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

		public Dictionary<string, int> units;

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
			units = new Dictionary<string, int> 
			{ 
				{ "Zumwalt class guided missile destroyer", r1 }, 
				{ "Arleigh Burke class guided-missile destroyer", r2 }, 
				{ "Gerald R. Ford Class Aircraft Carrier: John F. Kennedy", r3 }, 
				{ "Iowa Class Battleship", r4 } 
			};

			#endregion
		}
	}

	public struct City
	{
		public int population;
		public Dictionary<string, int> cityBuildings;

		public City(int aa, int r1 = 0, int r2 = 1, int r3 = 2, int r4 = 3)
		{
			population = 37468000;

			cityBuildings = new Dictionary<string, int>
			{
				{ "Civilian Factories", r1 },
				{ "Military Factories", r2 },
				{ "Dockyards", r3 },
				{ "Infrastructure", r4 }
			};
		}
	}

	public Tile nullTile = new(-1, -1, new Vector2(0,0), "Null", false, -1, -1, -1, -1, -1);
	public Tile nullWaterTile = new(-1, -1, new Vector2(0, 0), "Null", false, -1, -1, -1, -1, -1);

	public Dictionary<string, Vector2[]> TileMap = new()
	{
		{ "Grass",			new Vector2[] { new Vector2(0.5f,  0.75f), new Vector2(0.5f,  1	   ), new Vector2(0.75f, 1f	  ), new Vector2(0.75f,	0.75f) } },
		{ "Stone",			new Vector2[] { new Vector2(0.75f, 0.75f), new Vector2(0.75f, 1	   ), new Vector2(1,	 1	  ), new Vector2(1,		0.75f) } },
		{ "Selected_Tile",	new Vector2[] { new Vector2(0.75f, 0	), new Vector2(0.75f, 0.25f), new Vector2(1,	 0.25f), new Vector2(1,		0	 ) } },

		{ "Gold",			new Vector2[] { new Vector2(0,	   0.75f), new Vector2(0,	  0.50f), new Vector2(0.25f, 0.75f), new Vector2(0.25f, 0.50f) } },
		{ "Blue",			new Vector2[] { new Vector2(0.25f, 0.50f), new Vector2(0.25f, 0.75f), new Vector2(0.50f, 0.75f), new Vector2(0.50f, 0.50f) } },
		{ "Orange",         new Vector2[] { new Vector2(0.50f, 0.50f), new Vector2(0.50f, 0.75f), new Vector2(0.75f, 0.75f), new Vector2(0.75f, 0.50f) } },
		{ "Red",			new Vector2[] { new Vector2(0.75f, 0.50f), new Vector2(0.75f, 0.75f), new Vector2(1,     0.75f), new Vector2(1,     0.50f) } },

		{ "Debug",          new Vector2[] { new Vector2(0,     1    ), new Vector2(0,     0.75f), new Vector2(0.25f, 1    ), new Vector2(0.25f, 0.75f) } },
	};
}