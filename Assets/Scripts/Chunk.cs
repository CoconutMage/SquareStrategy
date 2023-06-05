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

		//mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		mesh.name = "Chunk";
		mesh.Clear();

		vertices = new Vector3[((xSize + 1) * (ySize + 1)) * 6];
		Vector2[] uv = new Vector2[vertices.Length];
		int[] triangles = new int[(xSize * ySize * 6) * 8];
		float height = 1;
		float y = 0;

		for (int i = 0, ti = 0, index = 0; y < ySize; y++)
		{
			for (int x = 0; x < xSize; x++, i += 6, ti += 12, index++)
			{
				if (x % 2 == 0)
                {
					vertices[i] = new Vector3(x, y);
					vertices[i + 1] = new Vector3(x - (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad), y + (0.5f * height));
					vertices[i + 2] = new Vector3(x, y + height);

					vertices[i + 3] = new Vector3(x + (height / Mathf.Tan(60 * Mathf.Deg2Rad)), y + height);
					vertices[i + 4] = new Vector3(x + (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad) + (height / Mathf.Tan(60 * Mathf.Deg2Rad)), y + (0.5f * height));
					vertices[i + 5] = new Vector3(x + (height / Mathf.Tan(60 * Mathf.Deg2Rad)), y);

					triangles[ti] = i;
					triangles[ti + 1] = i + 1;
					triangles[ti + 2] = i + 2;

					triangles[ti + 3] = i;
					triangles[ti + 4] = i + 2;
					triangles[ti + 5] = i + 5;

					triangles[ti + 6] = i + 2;
					triangles[ti + 7] = i + 4;
					triangles[ti + 8] = i + 5;

					triangles[ti + 9] = i + 2;
					triangles[ti + 10] = i + 3;
					triangles[ti + 11] = i + 4;

					uv[i] = new Vector2(0.066f, 0);
					uv[i + 1] = new Vector2(0, 0.133f);
					uv[i + 2] = new Vector2(0.066f, 0.266f);
					uv[i + 3] = new Vector2(0.215f, 0.266f);
					uv[i + 4] = new Vector2(0.230f, 0.133f);
					uv[i + 5] = new Vector2(0.215f, 0);
				}
				else
                {
					y += (0.5f * height);
					vertices[i] = new Vector3(x, y);
					vertices[i + 1] = new Vector3(x - (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad), y + (0.5f * height));
					vertices[i + 2] = new Vector3(x, y + height);

					vertices[i + 3] = new Vector3(x + (height / Mathf.Tan(60 * Mathf.Deg2Rad)), y + height);
					vertices[i + 4] = new Vector3(x + (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad) + (height / Mathf.Tan(60 * Mathf.Deg2Rad)), y + (0.5f * height));
					vertices[i + 5] = new Vector3(x + (height / Mathf.Tan(60 * Mathf.Deg2Rad)), y);

					triangles[ti] = i;
					triangles[ti + 1] = i + 1;
					triangles[ti + 2] = i + 2;

					triangles[ti + 3] = i;
					triangles[ti + 4] = i + 2;
					triangles[ti + 5] = i + 5;

					triangles[ti + 6] = i + 2;
					triangles[ti + 7] = i + 4;
					triangles[ti + 8] = i + 5;

					triangles[ti + 9] = i + 2;
					triangles[ti + 10] = i + 3;
					triangles[ti + 11] = i + 4;

					uv[i] = new Vector2(0.066f, 0);
					uv[i + 1] = new Vector2(0, 0.133f);
					uv[i + 2] = new Vector2(0.066f, 0.266f);
					uv[i + 3] = new Vector2(0.215f, 0.266f);
					uv[i + 4] = new Vector2(0.230f, 0.133f);
					uv[i + 5] = new Vector2(0.215f, 0);
					y -= (0.5f * height);
				}
				//y++;

				//PERLIN NOISE CODE USED TO GO HERE

				/*for (int zz = 0; zz < 6; zz++)
				{
					uv[0 + zz] = data.TileMap["Stone"][zz];
				}*/

				data.PopulateChunkTileData(chunkIndex, index, new Vector2(0, 0), "Stone");
			}
			//y++;
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