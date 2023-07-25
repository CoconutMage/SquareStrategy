using UnityEngine;

public class Map : MonoBehaviour
{
	public static Map Instance { get; private set; }

	public int xSize, ySize, chunkXSize, chunkYSize;
	public int chunkIndex;

	public GameObject chunkPrefab;

	Data data;
	Chunk chunkScript;
	EarthMapValues emv;
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
		emv = EarthMapValues.Instance;

		noiseOffsetX = Random.Range(0, 9999);
		noiseOffsetY = Random.Range(0, 9999);

		//PerlinGenerate();
		//Generate();
		ReadMap();
	}

	void Generate()
	{
		float height = 1;
		//The length of one flat side
		float offsetSide = (height / Mathf.Tan(60 * Mathf.Deg2Rad));
		//The x length of the angled side on the left and right edges
		float offsetEdge = (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad);

		for (int w = 0; w < 16/*64*/; w++)
		{
			for (int z = 0; z < 32/*104*/; z++, chunkIndex++)
			{
				Vector2 chunkPos = new Vector2(z * chunkXSize * (offsetSide + offsetEdge), w * chunkYSize * height);
				data.PopulateChunkData(chunkIndex, chunkPos);

				float y = 0;
				float x;
				float colNum = 0;

				for (int i = 0, ti = 0, index = 0; y < chunkYSize; y++)
				{
					colNum = 0;
					for (x = 0; colNum < chunkXSize; x += offsetSide + offsetEdge, i += 25, ti += 12, index++)
					{
						if (colNum % 2 != 0) y += (0.5f * height);
						string tileType = "";

						if (emv.tiles[z, w] == 16)
						{
							tileType = "water";
						}
						if (emv.tiles[z, w] == 15)
						{
							tileType = "mountain";
						}
						if (emv.tiles[z, w] == 14)
						{
							tileType = "hill";
						}
						else
						{
							int r = Random.Range(0,4);
							switch (r)
							{
								case 0:
									tileType = "grass";
									break;
								case 1:
									tileType = "water";
									break;
								case 2:
									tileType = "mountain";
									break;
								case 3:
									tileType = "hill";
									break;
							}
						}

						if (colNum % 2 != 0) y -= (0.5f * height);

						data.PopulateChunkTileData(0, chunkIndex, index, new Vector2(0, 0), tileType);
						colNum++;
					}
				}

				//Physical Characteristics
				GameObject chunk = Instantiate(chunkPrefab);
				chunk.transform.parent = transform;
				chunk.transform.localPosition = chunkPos;

				//Data Management
				chunkScript = chunk.GetComponent<Chunk>();
				chunkScript.chunkIndex = chunkIndex;
				chunkScript.chunkPosition = chunkPos;
			}
		}
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
				data.PopulateChunkData(chunkIndex, chunkPos);

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

						//Debug.Log("Perlin: " + Mathf.PerlinNoise(x / (float)xSize, y / (float)ySize));
						//Debug.Log("Index: " + chunkIndex + " : " + x + " : " + (xSize * (chunkIndex % map.xSize)) * (offsetSide + offsetEdge) + " : " + (float)(xSize * map.xSize * (offsetSide + offsetEdge)));

						//--------------------------------------------------------------------------------------------------------------------------- I think these cords are wrong

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

						//-----------------------------------------------------------------------------

						for (int k = 0; k < octave; k++)
						{
							perlinVal += (Mathf.PerlinNoise((offsetX + xCord) / mapSizeX * frequency * (Mathf.Pow(lacunarity, k)), (offsetY + yCord) / mapSizeY * frequency * (Mathf.Pow(lacunarity, k)))) * Mathf.Pow(persistance, k);
						}

						perlinVal *= amplitude;
						perlinVal += offsetZ;
						//Debug.Log("Index: " + chunkIndex + " : " + x + " : " + perlinVal);

						if (perlinVal <= 3.5f) tileType = "water";
						else if (perlinVal > 3.5f && perlinVal <= 7f) tileType = "grass";
						else if (perlinVal > 7f && perlinVal <= 8.75f) tileType = "hill";
						else tileType = "mountain";

						if (colNum % 2 != 0) y -= (0.5f * height);

						data.PopulateChunkTileData(0, chunkIndex, index, new Vector2(0, 0), tileType);
						//mapData.PopulateChunkTileData(r, chunkIndex, index, new Vector2(0, 0), "Stone");
						colNum++;
					}
				}

				//Physical Characteristics
				GameObject chunk = Instantiate(chunkPrefab);
				chunk.transform.parent = transform;
				chunk.transform.localPosition = chunkPos;

				//Data Management
				chunkScript = chunk.GetComponent<Chunk>();
				chunkScript.chunkIndex = chunkIndex;
				chunkScript.chunkPosition = chunkPos;
			}
		}
	}
	void ReadMap()
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
				data.PopulateChunkData(chunkIndex, chunkPos);

				float y = 0;
				float x;
				float colNum = 0;

				for (int i = 0, ti = 0, index = 0; y < chunkYSize; y++)
				{
					colNum = 0;
					for (x = 0; colNum < chunkXSize; x += /*offsetSide + offsetEdge*/ 1, i += 25, ti += 12, index++)
					{
						//if (colNum % 2 != 0) y += (0.5f * height);

						int r = Random.Range(0, 101);
						string tileType = "";

						//Debug.Log("Perlin: " + Mathf.PerlinNoise(x / (float)xSize, y / (float)ySize));
						//Debug.Log("Index: " + chunkIndex + " : " + x + " : " + (xSize * (chunkIndex % map.xSize)) * (offsetSide + offsetEdge) + " : " + (float)(xSize * map.xSize * (offsetSide + offsetEdge)));

						//These numbers I pulled out of my ass, so edit for your pleasure. Except for coords and map size, dont edit those
						float xCord = x + (chunkXSize * (chunkIndex % xSize))/* * (offsetSide + offsetEdge))*/, yCord = y + (chunkYSize * (chunkIndex / xSize)/* * height*/);

						Debug.Log("XCord: " + (x + (chunkXSize * (chunkIndex % xSize))) + " : " + (y + (chunkYSize * (chunkIndex / xSize))));

						if (emv.tiles[(int)xCord, (int)yCord] == 16) tileType = "water";
						else if (emv.tiles[(int)xCord, (int)yCord] == 15) tileType = "coast";
						else if (emv.tiles[(int)xCord, (int)yCord] == 14) tileType = "snow";
						else if (emv.tiles[(int)xCord, (int)yCord] == 13) tileType = "snow";
						else if (emv.tiles[(int)xCord, (int)yCord] == 12) tileType = "snow";
						else if (emv.tiles[(int)xCord, (int)yCord] == 11) tileType = "tundra";
						else if (emv.tiles[(int)xCord, (int)yCord] == 10) tileType = "tundra";
						else if (emv.tiles[(int)xCord, (int)yCord] == 9) tileType = "tundra";
						else if (emv.tiles[(int)xCord, (int)yCord] == 8) tileType = "desert";
						else if (emv.tiles[(int)xCord, (int)yCord] == 7) tileType = "desert";
						else if (emv.tiles[(int)xCord, (int)yCord] == 6) tileType = "desert";
						else if (emv.tiles[(int)xCord, (int)yCord] == 5) tileType = "forest";
						else if (emv.tiles[(int)xCord, (int)yCord] == 4) tileType = "forest";
						else if (emv.tiles[(int)xCord, (int)yCord] == 3) tileType = "forest";
						else tileType = "grass";

						//if (colNum % 2 != 0) y -= (0.5f * height);

						data.PopulateChunkTileData(0, chunkIndex, index, new Vector2(0, 0), tileType);
						//mapData.PopulateChunkTileData(r, chunkIndex, index, new Vector2(0, 0), "Stone");
						colNum++;
					}
				}

				//Physical Characteristics
				GameObject chunk = Instantiate(chunkPrefab);
				chunk.transform.parent = transform;
				chunk.transform.localPosition = chunkPos;

				//Data Management
				chunkScript = chunk.GetComponent<Chunk>();
				chunkScript.chunkIndex = chunkIndex;
				chunkScript.chunkPosition = chunkPos;
			}
		}
	}
}
