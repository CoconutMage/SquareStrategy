using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
	Data data;
	Map map;

	public int xSize, ySize;
	public int chunkIndex;
	public Vector2 chunkPosition;

	public enum HexDirection { NE, S, SE, SW, W, NW }

	private void Awake()
	{
		data = Data.Instance;
		map = Map.Instance;

		chunkIndex = map.chunkIndex;
	}
	void Start()
	{
		Generate();
	}

	//private Vector3[] vertices;
	private List<Vector3> vertices;
	private Mesh mesh;

	private void Generate()
	{
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		GetComponent<MeshCollider>().sharedMesh = mesh;

		//mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		mesh.name = "Chunk";
		mesh.Clear();

		//vertices = new Vector3[((xSize + 1) * (ySize + 1)) * 6];
		vertices = new List<Vector3>();
		//Vector2[] uv = new Vector2[vertices.Length];
		//Vector2[] uv2 = new Vector2[vertices.Length];
		List<int> triangles = new List<int>();
		List<Color> colors = new List<Color>();

		float height = 1;
		float y = 0;
		float x;
		float colNum = 0;
		float offsetSide = (height / Mathf.Tan(60 * Mathf.Deg2Rad));
		float offsetEdge = (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad);

		Color grassColor = Color.green;
		Color hillColor = new Color(0.3f, 0.3f, 0.3f);
		Color mountainColor = Color.gray;
		Color waterColor = Color.blue;

		for (int i = 0, ti = 0, index = 0; y < ySize; y++)
		{
			colNum = 0;
			for (x = 0; colNum < xSize; x += offsetSide + offsetEdge, i += 6, ti += 12, index++)
			{
				if (colNum % 2 != 0) y += (0.5f * height);

				vertices.Add(new Vector3(x, y));
				vertices.Add(new Vector3(x - offsetEdge, y + (0.5f * height)));
				vertices.Add(new Vector3(x, y + height));

				vertices.Add(new Vector3(x + offsetSide, y + height));
				vertices.Add(new Vector3(x + offsetEdge + offsetSide, y + (0.5f * height)));
				vertices.Add(new Vector3(x + offsetSide, y));

				triangles.Add(i);
				triangles.Add(i + 1);
				triangles.Add(i + 2);

				triangles.Add(i);
				triangles.Add(i + 2);
				triangles.Add(i + 5);

				triangles.Add(i + 2);
				triangles.Add(i + 4);
				triangles.Add(i + 5);

				triangles.Add(i + 2);
				triangles.Add(i + 3);
				triangles.Add(i + 4);

				int r = Random.Range(0,101);
				int tileType = 0;

				//Debug.Log("Perlin: " + Mathf.PerlinNoise(x / (float)xSize, y / (float)ySize));
				//Debug.Log("Index: " + chunkIndex + " : " + x + " : " + (xSize * (chunkIndex % map.xSize)) * (offsetSide + offsetEdge) + " : " + (float)(xSize * map.xSize * (offsetSide + offsetEdge)));

				//These numbers I pulled out of my ass, so edit for your pleasure. Except for coords and map size, dont edit those
				float xCord = x + (xSize * (chunkIndex % map.xSize) * (offsetSide + offsetEdge)), yCord = y + (ySize * (chunkIndex / map.xSize) * height);
				float mapSizeX = (float)(xSize * map.xSize * (offsetSide + offsetEdge)), mapSizeY = (float)(ySize * map.ySize * height);
				float offsetX = map.noiseOffsetX, offsetY = map.noiseOffsetY;
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

				if (perlinVal <= 3.5f)
				{
					tileType = 0;
					colors.Add(waterColor);
					colors.Add(waterColor);
					colors.Add(waterColor);

					colors.Add(waterColor);
					colors.Add(waterColor);
					colors.Add(waterColor);
				}
				else if (perlinVal > 3.5f && perlinVal <= 7f)
				{
					tileType = 1;
					colors.Add(grassColor);
					colors.Add(grassColor);
					colors.Add(grassColor);

					colors.Add(grassColor);
					colors.Add(grassColor);
					colors.Add(grassColor);
				}
				else if (perlinVal > 7f && perlinVal <= 8.75f)
				{
					tileType = 2;
					colors.Add(hillColor);
					colors.Add(hillColor);
					colors.Add(hillColor);

					colors.Add(hillColor);
					colors.Add(hillColor);
					colors.Add(hillColor);
				}
				else
				{
					tileType = 3;
					colors.Add(mountainColor);
					colors.Add(mountainColor);
					colors.Add(mountainColor);

					colors.Add(mountainColor);
					colors.Add(mountainColor);
					colors.Add(mountainColor);
				}

				if (colNum % 2 != 0) y -= (0.5f * height);

				//y++;

				//PERLIN NOISE CODE USED TO GO HERE

				/*for (int zz = 0; zz < 6; zz++)
				{
					uv[0 + zz] = data.TileMap["Stone"][zz];
				}*/

				data.PopulateChunkTileData(tileType, chunkIndex, index, new Vector2(0, 0), "Stone");
				//mapData.PopulateChunkTileData(r, chunkIndex, index, new Vector2(0, 0), "Stone");
				colNum++;
			}
			//y++;
		}

		mesh.subMeshCount = 2;
		mesh.vertices = vertices.ToArray();
		mesh.colors = colors.ToArray();
		mesh.triangles = triangles.ToArray();
		//mesh.SetTriangles(triangles, 0);
		//mesh.SetTriangles(triangles, 1);
		mesh.RecalculateNormals();
		//mesh.uv = uv;

	}

	/* PERLIN NOISE CODE
		float sample = Mathf.PerlinNoise((float)((float)x / xSize), (float)((float)y / ySize));
		Debug.Log(sample);
		if (sample < 0.45f)
		{
			uv[i] = new Vector2(0.5f, 0.75f);
			uv[i + 1] = new Vector2(0.5f, 1);
			uv[i + 2] = new Vector2(0.75f, 1f);
			uv[i + 3] = new Vector2(0.75f, 0.75f);
		}
		else
		{
			uv[i] = new Vector2(0.75f, 0.75f);
			uv[i + 1] = new Vector2(0.75f, 1);
			uv[i + 2] = new Vector2(1f, 1f);
			uv[i + 3] = new Vector2(1f, 0.75f);
		}
	*/
}