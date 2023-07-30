using System.Collections.Generic;
using UnityEngine;

public class MapEditorGeneration : MonoBehaviour
{
	public static MapEditorGeneration Instance { get; private set; }

	public int xSize, ySize, chunkXSize, chunkYSize;
	public int chunkIndex;

	public GameObject chunkPrefab;

	Data data;
	CameraController cam;
	MapEditorChunk chunkScript;
	public float noiseOffsetX, noiseOffsetY;



	private void Start()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		data = Data.Instance;
		cam = CameraController.Instance;

		data.mapEditorScene = true;

		noiseOffsetX = Random.Range(0, 9999);
		noiseOffsetY = Random.Range(0, 9999);

		PerlinGenerate();
	}

	void PerlinGenerate()
	{
		float height = 1;
		//The length of one flat side
		float offsetSide = (height / Mathf.Tan(60 * Mathf.Deg2Rad));
		//The x length of the angled side on the left and right edges
		float offsetEdge = (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad);

		for (int w = 0; w < ySize; w++)
		{
			for (int z = 0; z < xSize; z++, chunkIndex++)
			{
				Vector2 chunkPos = new Vector2(z * chunkXSize * (offsetSide + offsetEdge), w * chunkYSize * height);
				PopulateChunkData(chunkIndex, chunkPos);

				float y = 0;
				float x;
				float colNum = 0;

				for (int i = 0, ti = 0, index = 0; y < chunkYSize; y++)
				{
					colNum = 0;
					for (x = 0; colNum < chunkXSize; x += offsetSide + offsetEdge, i += 25, ti += 12, index++)
					{
						if (colNum % 2 != 0) y += (0.5f * height);

						int r = Random.Range(0, 101);
						string tileType = "";

						//These numbers I pulled out of my ass, so edit for your pleasure. Except for coords and map size, dont edit those
						float xCord = x + (chunkXSize * (chunkIndex % xSize) * (offsetSide + offsetEdge)), yCord = y + (chunkYSize * (chunkIndex / xSize) * height);
						float mapSizeX = (float)(chunkXSize * xSize * (offsetSide + offsetEdge)), mapSizeY = (float)(chunkYSize * ySize * height);
						float offsetX = noiseOffsetX, offsetY = noiseOffsetY;
						//How many iterations of less impactfull noise functions are layered on
						int octave = 3;
						//How much detail is added for each octave. Basically increases the frequency for each successive octave
						int lacunarity = 2;
						//How bunched together the hills are. Think frequency of a sound wave
						float frequency = 3.75f;
						//How high and low the peaks and valleys are
						float amplitude = 10;
						//How much less impactful each successive octave is. Basically reduces the amplitutude of each octave
						float persistance = 0.5f;
						float offsetZ = -2.5f;
						float perlinVal = 0;


						for (int k = 0; k < octave; k++)
						{
							perlinVal += (Mathf.PerlinNoise((offsetX + xCord) / mapSizeX * frequency * 
								(Mathf.Pow(lacunarity, k)), (offsetY + yCord) / mapSizeY * frequency * 
								(Mathf.Pow(lacunarity, k)))) * Mathf.Pow(persistance, k);
						}

						perlinVal *= amplitude;
						perlinVal += offsetZ;
						//Debug.Log("Index: " + chunkIndex + " : " + x + " : " + perlinVal);

						if (perlinVal <= 3.5f) tileType = "water";
						else if (perlinVal > 3.5f && perlinVal <= 7f) tileType = "grass";
						else if (perlinVal > 7f && perlinVal <= 8.75f) tileType = "forest";
						else tileType = "desert";

						if (colNum % 2 != 0) y -= (0.5f * height);

						PopulateChunkTileData(Random.Range(0, 10), chunkIndex, index, new Vector2(0, 0), tileType);
						//mapData.PopulateChunkTileData(r, chunkIndex, index, new Vector2(0, 0), "Stone");
						colNum++;
					}
				}

				//Physical Characteristics
				GameObject chunk = Instantiate(chunkPrefab);
				chunk.transform.parent = transform;
				chunk.transform.localPosition = chunkPos;

				//Data Management
				chunkScript = chunk.GetComponent<MapEditorChunk>();
				chunkScript.chunkIndex = chunkIndex;
				chunkScript.chunkPosition = chunkPos;
			}
		}
	}

	public void PopulateChunkData(int chunkIndex, Vector2 location)
	{
		data.map[chunkIndex] = new Data.Chunk
		(
			chunkIndex,
			location,
			new Dictionary<int, Data.Tile>()
		);
	}

	public void PopulateChunkTileData(int r, int chunkIndex, int index, Vector2 pos, string tileType)
	{
		if (chunkIndex == 0 && index != 0 && index != 1 && index != 8)
		{
			createTileData(data.countries["USA"], Data.nullCity);
		}
		else if (chunkIndex == 0 && index == 0)
		{
			createTileData(data.countries["USA"], data.cities["WashingtonDC"]);
		}
		else if (chunkIndex == 0 && index == 1)
		{
			createTileData(data.countries["USA"], data.cities["New York"]);
		}
		else if (chunkIndex == 0 && index == 8)
		{
			createTileData(data.countries["USA"], data.cities["Los Angeles"]);
		}

		else if (chunkIndex == 3 && index != 63)
		{
			createTileData(data.countries["USSR"], Data.nullCity);
		}
		else if (chunkIndex == 3 && index == 63)
		{
			createTileData(data.countries["USSR"], data.cities["Moscow"]);
		}

		else
		{
			createTileData(Data.nullCountry, Data.nullCity);
		}

		void createTileData(Data.Country country, Data.City city)
		{
			data.map[chunkIndex].tiles[index] = new Data.Tile(chunkIndex, index, pos, tileType, country, city, r, r, r, r, r);
		}
	}
}