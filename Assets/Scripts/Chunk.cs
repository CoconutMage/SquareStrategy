using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
	MapData data;
	Map map;

	public int xSize, ySize;
	public int chunkIndex;
	public Vector2 chunkPosition;

	private void Awake()
	{
		data = MapData.Instance;
		map = Map.Instance;

		chunkIndex = map.chunkIndex;
	}
	void Start()
	{
	
		Generate();
	}

	private Vector3[] vertices;
	private Mesh mesh;

	private void Generate()
	{
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		GetComponent<MeshCollider>().sharedMesh = mesh;

		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		mesh.name = "Chunk";
		mesh.Clear();


		vertices = new Vector3[((xSize + 1) * (ySize + 1)) * 4];
		Vector2[] uv = new Vector2[vertices.Length];
		int[] triangles = new int[(xSize * ySize * 6) * 4];

		for (int i = 0, y = 0, ti = 0, index = 0; y < ySize; y++)
		{
			for (int x = 0; x < xSize; x++, i += 4, ti += 6, index++)
			{
				vertices[i] = new Vector3(x, y);
				vertices[i + 1] = new Vector3(x, y + 1);
				vertices[i + 2] = new Vector3(x + 1, y + 1);
				vertices[i + 3] = new Vector3(x + 1, y);

				triangles[ti] = i;
				triangles[ti + 1] = i + 1;
				triangles[ti + 2] = i + 2;
				triangles[ti + 3] = i;
				triangles[ti + 4] = i + 2;
				triangles[ti + 5] = i + 3;

				//PERLIN NOISE CODE USED TO GO HERE

				for (int zz = 0; zz < 4; zz++)
				{
					uv[i + zz] = data.TileMap["Stone"][zz];
				}

				data.PopulateChunkTileData(chunkIndex, index, new Vector2(x, y), "Stone");
			}
		}

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.uv = uv;

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