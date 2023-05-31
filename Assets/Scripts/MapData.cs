using System;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
	public static MapData Instance { get; private set; }
	public Dictionary<int, Chunk> Map;

	private void Awake()
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
		Map[chunkIndex].tiles[i] = new Tile(chunkIndex, i, pos, material, i);
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
		public Vector2 location;

		public string material;

		public int units;

		public Tile(int aa, int ab, Vector2 ac, string ad, int ae)
		{
			chunkIndex = aa;
			index = ab;
			location = ac;

			material = ad;

			units = ae;
		}
	}

	public Tile nullTile = new(-1, -1, new Vector2(0,0), "Null", -1);

	public Dictionary<string, Vector2[]> TileMap = new()
	{
		{ "Grass",			new Vector2[] { new Vector2(0.5f,  0.75f), new Vector2(0.5f,  1		), new Vector2(0.75f, 1f), new Vector2(0.75f,	0.75f) } },
		{ "Stone",			new Vector2[] { new Vector2(0.75f, 0.75f), new Vector2(0.75f, 1		), new Vector2(1, 1		), new Vector2(1,		0.75f) } },
		{ "Selected_Tile",	new Vector2[] { new Vector2(0.75f, 0	), new Vector2(0.75f, 0.25f	), new Vector2(1, 0.25f	), new Vector2(1,		0	 ) } },

		{ "Gold",			new Vector2[] { new Vector2(0,	   0.75f), new Vector2(0,	  0.50f ), new Vector2(0.25f, 0.75f ), new Vector2(0.25f,   0.50f) } },
		{ "Blue",			new Vector2[] { new Vector2(0.25f,     0.75f    ), new Vector2(0,     0.75f ), new Vector2(0.25f, 1 ), new Vector2(0.25f,   0.75f) } },
		{ "Orange",         new Vector2[] { new Vector2(0,     1    ), new Vector2(0,     0.75f ), new Vector2(0.25f, 1 ), new Vector2(0.25f,   0.75f) } },
		{ "Red",			new Vector2[] { new Vector2(0,     1    ), new Vector2(0,     0.75f ), new Vector2(0.25f, 1 ), new Vector2(0.25f,   0.75f) } },

		{ "Debug",          new Vector2[] { new Vector2(0,     1    ), new Vector2(0,     0.75f ), new Vector2(0.25f, 1 ), new Vector2(0.25f,   0.75f) } },
	};
}