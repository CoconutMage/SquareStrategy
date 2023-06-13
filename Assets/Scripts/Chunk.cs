using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
	Data data;
	Map map;

	public int xSize, ySize;
	public int chunkIndex;
	public Vector2 chunkPosition;

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
		Vector2[] uv2 = new Vector2[vertices.Length];
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

				/*float oneX = 18f, twoX = 1f, threeX = 18f, fourX = 53f, fiveX = 70.5f, sixX = 53f;
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
				uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);*/

				int r = Random.Range(0,101);
				int tileType = 0;

				//Sacred numbers given by the Hexagods, do not touch
				float oneX = 18f, twoX = 1f, threeX = 18f, fourX = 53f, fiveX = 70.5f, sixX = 53f;
				float oneY = 2f, twoY = 32f, threeY = 62f, fourY = 62f, fiveY = 32f, sixY = 2f;
				float sizeX = 288, sizeY = 256;
				float modifierX = -0.5f, modifierY = 0.5f;
				int textureIndexX = 0, textureIndexY = 0;
				//-----------------------------------------------------------------------------

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

				if (perlinVal <= 3f)
                {
					tileType = 0;

					textureIndexX = 0;
					textureIndexY = 1;
					modifierX -= (72 * textureIndexX);
					modifierY -= (64 * textureIndexY);

					uv[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
					uv[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
					uv[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
					uv[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
					uv[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
					uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
				}
				else if (perlinVal > 3f && perlinVal <= 6f)
				{
					if (r <= 90)
					{
						tileType = 0;

						textureIndexX = 1;
						textureIndexY = 0;
						modifierX -= (72 * textureIndexX);
						modifierY -= (64 * textureIndexY);

						uv[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
						uv[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
						uv[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
						uv[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
						uv[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
						uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
					}
					else
                    {
						tileType = 1;

						textureIndexX = 2;
						textureIndexY = 0;
						modifierX -= (72 * textureIndexX);
						modifierY -= (64 * textureIndexY);

						uv[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
						uv[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
						uv[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
						uv[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
						uv[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
						uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
					}
				}
				else if (perlinVal > 6f && perlinVal <= 8f)
				{
					tileType = 0;

					textureIndexX = 3;
					textureIndexY = 0;
					modifierX -= (72 * textureIndexX);
					modifierY -= (64 * textureIndexY);

					uv[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
					uv[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
					uv[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
					uv[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
					uv[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
					uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
				}
				else if (perlinVal > 8f)
				{
					tileType = 0;

					textureIndexX = 0;
					textureIndexY = 0;
					modifierX -= (72 * textureIndexX);
					modifierY -= (64 * textureIndexY);

					uv[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
					uv[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
					uv[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
					uv[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
					uv[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
					uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
				}
				//if (r <= 98)
				/*if (r <= 98)
				{
					tileType = 0;
					if (r <= 20)
                    {
						textureIndexX = 3;
						textureIndexY = 0;
						modifierX -= (72 * textureIndexX);
						modifierY -= (64 * textureIndexY);

						uv[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
						uv[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
						uv[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
						uv[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
						uv[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
						uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
					}
					else if (r <= 30)
					{
						textureIndexX = 0;
						textureIndexY = 0;
						modifierX -= (72 * textureIndexX);
						modifierY -= (64 * textureIndexY);

						uv[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
						uv[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
						uv[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
						uv[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
						uv[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
						uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
					}
					else
					{
						textureIndexX = 1;
						textureIndexY = 0;
						modifierX -= (72 * textureIndexX);
						modifierY -= (64 * textureIndexY);

						uv[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
						uv[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
						uv[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
						uv[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
						uv[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
						uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
					}
				}
				else
				{
					tileType = 1;
					textureIndexX = 2;
					textureIndexY = 0;
					modifierX -= (72 * textureIndexX);
					modifierY -= (64 * textureIndexY);

					uv[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
					uv[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
					uv[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
					uv[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
					uv[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
					uv[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
				}*/

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

				textureIndexX = 0;
				textureIndexY = 0;
				modifierX -= (72 * textureIndexX);
				modifierY -= (64 * textureIndexY);

				uv2[i] = new Vector2((oneX - modifierX) / sizeX, (oneY - modifierY) / sizeY);
				uv2[i + 1] = new Vector2((twoX - modifierX) / sizeX, (twoY - modifierY) / sizeY);
				uv2[i + 2] = new Vector2((threeX - modifierX) / sizeX, (threeY - modifierY) / sizeY);
				uv2[i + 3] = new Vector2((fourX - modifierX) / sizeX, (fourY - modifierY) / sizeY);
				uv2[i + 4] = new Vector2((fiveX - modifierX) / sizeX, (fiveY - modifierY) / sizeY);
				uv2[i + 5] = new Vector2((sixX - modifierX) / sizeX, (sixY - modifierY) / sizeY);
			}
			//y++;
		}

		mesh.subMeshCount = 2;
		mesh.vertices = vertices;
		mesh.SetTriangles(triangles, 0);
		//mesh.SetTriangles(triangles, 1);
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