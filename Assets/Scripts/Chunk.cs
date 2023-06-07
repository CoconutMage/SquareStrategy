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
		float x;
		float colNum = 0;
		float offsetSide = (height / Mathf.Tan(60 * Mathf.Deg2Rad));
		float offsetEdge = (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad);

		for (int i = 0, ti = 0, index = 0; y < ySize; y++)
		{
			colNum = 0;
			for (x = 0; colNum < xSize; x += offsetSide + offsetEdge, i += 6, ti += 12, index++)
			{
				if (colNum % 2 != 0) y += (0.5f * height);

				vertices[i] = new Vector3(x, y);
				vertices[i + 1] = new Vector3(x - offsetEdge, y + (0.5f * height));
				vertices[i + 2] = new Vector3(x, y + height);

				vertices[i + 3] = new Vector3(x + offsetSide, y + height);
				vertices[i + 4] = new Vector3(x + offsetEdge + offsetSide, y + (0.5f * height));
				vertices[i + 5] = new Vector3(x + offsetSide, y);

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

<<<<<<< Updated upstream
				int r = Random.Range(0,2);
				if (r == 0)
				{
					float oneX = 18f, twoX = 1f, threeX = 18f, fourX = 53f, fiveX = 70.5f, sixX = 53f;
					float oneY = 2f, twoY = 32f, threeY = 62f, fourY = 62f, fiveY = 32f, sixY = 2f;
					//float sizeX = 72, sizeY = 63;
					float sizeX = 288, sizeY = 256;
					//float modifierX = -0.5f, modifierY = 0.5f;
					float modifierX = -0.5f, modifierY = 0.5f;

					uv[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
					uv[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
					uv[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
					uv[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
					uv[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
					uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
				}
				else
				{
					float oneX = 18f, twoX = 1f, threeX = 18f, fourX = 53f, fiveX = 70.5f, sixX = 53f;
					float oneY = 2f, twoY = 32f, threeY = 62f, fourY = 62f, fiveY = 32f, sixY = 2f;
					//float sizeX = 72, sizeY = 63;
					float sizeX = 288, sizeY = 256;
					//float modifierX = -0.5f, modifierY = 0.5f;
					float modifierX = -0.5f, modifierY = 0.5f;
					int textureIndexX = 1, textureIndexY = 1;
					modifierX -= (72 * 2);

					uv[i] = new Vector2(((oneX - modifierX) / sizeX) * textureIndexX, ((oneY - modifierY) / sizeY) * textureIndexY);
					uv[i + 1] = new Vector2(((twoX - modifierX) / sizeX) * textureIndexX, ((twoY - modifierY) / sizeY) * textureIndexY);
					uv[i + 2] = new Vector2(((threeX - modifierX) / sizeX) * textureIndexX, ((threeY - modifierY) / sizeY) * textureIndexY);
					uv[i + 3] = new Vector2(((fourX - modifierX) / sizeX) * textureIndexX, ((fourY - modifierY) / sizeY) * textureIndexY);
					uv[i + 4] = new Vector2(((fiveX - modifierX) / sizeX) * textureIndexX, ((fiveY - modifierY) / sizeY) * textureIndexY);
					uv[i + 5] = new Vector2(((sixX - modifierX) / sizeX) * textureIndexX, ((sixY - modifierY) / sizeY) * textureIndexY);
				}

				if (colNum % 2 != 0) y -= (0.5f * height);

				//y++;

				//PERLIN NOISE CODE USED TO GO HERE

				/*for (int zz = 0; zz < 6; zz++)
				{
					uv[0 + zz] = data.TileMap["Stone"][zz];
				}*/

				data.PopulateChunkTileData(r, chunkIndex, index, new Vector2(0, 0), "Stone");
				colNum++;
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