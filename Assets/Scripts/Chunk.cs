using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
	Data data;
	Map map;

	int xSize, ySize;
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
		xSize = map.chunkXSize;
		ySize = map.chunkYSize;
		RenderMap();
	}

	//private Vector3[] vertices;
	private List<Vector3> vertices;
	private Mesh mesh;
	public GameObject labelPrefab;

	private void RenderMap()
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
		   //The length of one flat side
		float offsetSide = (height / Mathf.Tan(60 * Mathf.Deg2Rad));
		   //The x length of the angled side on the left and right edges
		float offsetEdge = (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad);

		float blendRegion = 0.20f;

		Color grassColor = Color.green;
		Color hillColor = new Color(0.3f, 0.3f, 0.3f);
		Color mountainColor = Color.gray;
		Color waterColor = Color.blue;

		Dictionary<string, Color> colorDict = new Dictionary<string, Color>();
		colorDict["water"] = Color.blue;
		colorDict["grass"] = Color.green;
		colorDict["hill"] = new Color(0.3f, 0.3f, 0.3f);
		colorDict["mountain"] = Color.gray;

		for (int i = 0, ti = 0, index = 0; y < ySize; y++)
		{
			colNum = 0;
			for (x = 0; colNum < xSize; x += offsetSide + offsetEdge, i += 25, ti += 12, index++)
			{
				if (colNum % 2 != 0) y += (0.5f * height);
				
                #region forbiddenLand
                vertices.Add(new Vector3(x, y));

				vertices.Add(new Vector3(x - (offsetSide / 2) * (1 - blendRegion), y - ((height / 2) * (1 - blendRegion))));
				vertices.Add(new Vector3(x - ((offsetEdge + (offsetSide / 2)) * (1 - blendRegion)), y));
				vertices.Add(new Vector3(x - (offsetSide / 2) * (1 - blendRegion), y + ((height / 2) * (1 - blendRegion))));

				vertices.Add(new Vector3(x + (offsetSide / 2) * (1 - blendRegion), y + ((height / 2) * (1 - blendRegion))));
				vertices.Add(new Vector3(x + ((offsetEdge + (offsetSide / 2)) * (1 - blendRegion)), y));
				vertices.Add(new Vector3(x + (offsetSide / 2) * (1 - blendRegion), y - ((height / 2) * (1 - blendRegion))));

				vertices.Add(new Vector3(x - (offsetSide / 2), y - (height / 2)));
				vertices.Add(new Vector3(x - offsetEdge - (offsetSide / 2), y));
				vertices.Add(new Vector3(x - (offsetSide / 2), y + (height / 2)));

				vertices.Add(new Vector3(x + (offsetSide / 2), y + (height / 2)));
				vertices.Add(new Vector3(x + offsetEdge + (offsetSide / 2), y));
				vertices.Add(new Vector3(x + (offsetSide / 2), y - (height / 2)));

				//vertices.Add((((vertices[i + 1] + vertices[i + 2]) / 2) / (1 - blendRegion)) - ((vertices[i + 1] + vertices[i + 2]) / 2));
				//vertices.Add((((vertices[i + 1] + vertices[i + 2]) / 2) / (1 - blendRegion)) + ((vertices[i + 1] + vertices[i + 2]) / 2));

				float distBetween = Vector3.Distance(vertices[i + 1], vertices[i + 7]);
				float inwardDist = Mathf.Cos(60 * Mathf.Deg2Rad) * distBetween;
				float xDist = Mathf.Cos(60 * Mathf.Deg2Rad) * inwardDist;
				float yDist = Mathf.Sin(60 * Mathf.Deg2Rad) * inwardDist;
				Vector3 loc = new Vector3(vertices[i + 7].x - xDist, vertices[i + 7].y + yDist);
				vertices.Add(loc);

				distBetween = Vector3.Distance(vertices[i + 2], vertices[i + 8]);
				inwardDist = Mathf.Cos(60 * Mathf.Deg2Rad) * distBetween;
				xDist = Mathf.Cos(60 * Mathf.Deg2Rad) * inwardDist;
				yDist = Mathf.Sin(60 * Mathf.Deg2Rad) * inwardDist;
				loc = new Vector3(vertices[i + 8].x + xDist, vertices[i + 8].y - yDist);
				vertices.Add(loc);

				//15
				distBetween = Vector3.Distance(vertices[i + 2], vertices[i + 8]);
				inwardDist = Mathf.Cos(60 * Mathf.Deg2Rad) * distBetween;
				xDist = Mathf.Cos(60 * Mathf.Deg2Rad) * inwardDist;
				yDist = Mathf.Sin(60 * Mathf.Deg2Rad) * inwardDist;
				loc = new Vector3(vertices[i + 8].x + xDist, vertices[i + 8].y + yDist);
				vertices.Add(loc);

				distBetween = Vector3.Distance(vertices[i + 3], vertices[i + 9]);
				inwardDist = Mathf.Cos(60 * Mathf.Deg2Rad) * distBetween;
				xDist = Mathf.Cos(60 * Mathf.Deg2Rad) * inwardDist;
				yDist = Mathf.Sin(60 * Mathf.Deg2Rad) * inwardDist;
				loc = new Vector3(vertices[i + 9].x - xDist, vertices[i + 9].y - yDist);
				vertices.Add(loc);

				//17
				loc = new Vector3(vertices[i + 3].x, vertices[i + 9].y);
				vertices.Add(loc);

				loc = new Vector3(vertices[i + 4].x, vertices[i + 10].y);
				vertices.Add(loc);

				//19
				distBetween = Vector3.Distance(vertices[i + 4], vertices[i + 10]);
				inwardDist = Mathf.Cos(60 * Mathf.Deg2Rad) * distBetween;
				xDist = Mathf.Cos(60 * Mathf.Deg2Rad) * inwardDist;
				yDist = Mathf.Sin(60 * Mathf.Deg2Rad) * inwardDist;
				loc = new Vector3(vertices[i + 10].x + xDist, vertices[i + 10].y - yDist);
				vertices.Add(loc);

				distBetween = Vector3.Distance(vertices[i + 5], vertices[i + 11]);
				inwardDist = Mathf.Cos(60 * Mathf.Deg2Rad) * distBetween;
				xDist = Mathf.Cos(60 * Mathf.Deg2Rad) * inwardDist;
				yDist = Mathf.Sin(60 * Mathf.Deg2Rad) * inwardDist;
				loc = new Vector3(vertices[i + 11].x - xDist, vertices[i + 11].y + yDist);
				vertices.Add(loc);

				//21
				distBetween = Vector3.Distance(vertices[i + 5], vertices[i + 11]);
				inwardDist = Mathf.Cos(60 * Mathf.Deg2Rad) * distBetween;
				xDist = Mathf.Cos(60 * Mathf.Deg2Rad) * inwardDist;
				yDist = Mathf.Sin(60 * Mathf.Deg2Rad) * inwardDist;
				loc = new Vector3(vertices[i + 11].x - xDist, vertices[i + 11].y - yDist);
				vertices.Add(loc);

				distBetween = Vector3.Distance(vertices[i + 6], vertices[i + 12]);
				inwardDist = Mathf.Cos(60 * Mathf.Deg2Rad) * distBetween;
				xDist = Mathf.Cos(60 * Mathf.Deg2Rad) * inwardDist;
				yDist = Mathf.Sin(60 * Mathf.Deg2Rad) * inwardDist;
				loc = new Vector3(vertices[i + 12].x + xDist, vertices[i + 12].y + yDist);
				vertices.Add(loc);

				//23
				loc = new Vector3(vertices[i + 6].x, vertices[i + 12].y);
				vertices.Add(loc);

				loc = new Vector3(vertices[i + 1].x, vertices[i + 7].y);
				vertices.Add(loc);

				/*vertices.Add(new Vector3(x + ((offsetEdge + (offsetSide / 2)) * (1 - blendRegion)), y));
				vertices.Add(new Vector3(x + (offsetSide / 2) * (1 - blendRegion), y - ((height / 2) * (1 - blendRegion))));

				vertices.Add(new Vector3(x + ((offsetEdge + (offsetSide / 2)) * (1 - blendRegion)), y));
				vertices.Add(new Vector3(x + (offsetSide / 2) * (1 - blendRegion), y - ((height / 2) * (1 - blendRegion))));

				vertices.Add(new Vector3(x + ((offsetEdge + (offsetSide / 2)) * (1 - blendRegion)), y));
				vertices.Add(new Vector3(x + (offsetSide / 2) * (1 - blendRegion), y - ((height / 2) * (1 - blendRegion))));

				vertices.Add(new Vector3(x + ((offsetEdge + (offsetSide / 2)) * (1 - blendRegion)), y));
				vertices.Add(new Vector3(x + (offsetSide / 2) * (1 - blendRegion), y - ((height / 2) * (1 - blendRegion))));

				vertices.Add(new Vector3(x + ((offsetEdge + (offsetSide / 2)) * (1 - blendRegion)), y));
				vertices.Add(new Vector3(x + (offsetSide / 2) * (1 - blendRegion), y - ((height / 2) * (1 - blendRegion))));*/

				triangles.Add(i);
				triangles.Add(i + 1);
				triangles.Add(i + 2);

				triangles.Add(i);
				triangles.Add(i + 2);
				triangles.Add(i + 3);

				triangles.Add(i);
				triangles.Add(i + 3);
				triangles.Add(i + 4);

				triangles.Add(i);
				triangles.Add(i + 4);
				triangles.Add(i + 5);

				triangles.Add(i);
				triangles.Add(i + 5);
				triangles.Add(i + 6);

				triangles.Add(i);
				triangles.Add(i + 6);
				triangles.Add(i + 1);

				triangles.Add(i + 1);
				triangles.Add(i + 14);
				triangles.Add(i + 2);

				triangles.Add(i + 13);
				triangles.Add(i + 14);
				triangles.Add(i + 1);

				triangles.Add(i + 1);
				triangles.Add(i + 7);
				triangles.Add(i + 13);

				triangles.Add(i + 14);
				triangles.Add(i + 8);
				triangles.Add(i + 2);

				triangles.Add(i + 2);
				triangles.Add(i + 15);
				triangles.Add(i + 16);

				triangles.Add(i + 2);
				triangles.Add(i + 16);
				triangles.Add(i + 3);

				triangles.Add(i + 2);
				triangles.Add(i + 8);
				triangles.Add(i + 15);

				triangles.Add(i + 3);
				triangles.Add(i + 16);
				triangles.Add(i + 9);

				triangles.Add(i + 3);
				triangles.Add(i + 17);
				triangles.Add(i + 18);

				triangles.Add(i + 3);
				triangles.Add(i + 18);
				triangles.Add(i + 4);

				triangles.Add(i + 3);
				triangles.Add(i + 9);
				triangles.Add(i + 17);

				triangles.Add(i + 4);
				triangles.Add(i + 18);
				triangles.Add(i + 10);

				triangles.Add(i + 4);
				triangles.Add(i + 19);
				triangles.Add(i + 20);

				triangles.Add(i + 4);
				triangles.Add(i + 20);
				triangles.Add(i + 5);

				triangles.Add(i + 4);
				triangles.Add(i + 10);
				triangles.Add(i + 19);

				triangles.Add(i + 5);
				triangles.Add(i + 20);
				triangles.Add(i + 11);

				triangles.Add(i + 5);
				triangles.Add(i + 21);
				triangles.Add(i + 22);

				triangles.Add(i + 5);
				triangles.Add(i + 22);
				triangles.Add(i + 6);

				triangles.Add(i + 5);
				triangles.Add(i + 11);
				triangles.Add(i + 21);

				triangles.Add(i + 6);
				triangles.Add(i + 22);
				triangles.Add(i + 12);

				triangles.Add(i + 6);
				triangles.Add(i + 23);
				triangles.Add(i + 24);

				triangles.Add(i + 6);
				triangles.Add(i + 24);
				triangles.Add(i + 1);

				triangles.Add(i + 6);
				triangles.Add(i + 12);
				triangles.Add(i + 23);

				triangles.Add(i + 1);
				triangles.Add(i + 24);
				triangles.Add(i + 7);

				#endregion

				GameObject label = Instantiate(labelPrefab);
				label.transform.parent = transform;
				label.transform.localPosition = new Vector3(x, y);
				label.GetComponent<TextMeshPro>().text = index.ToString();

				if (colNum % 2 != 0) y -= (0.5f * height);

				int r = Random.Range(0,101);

				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

				if ((chunkIndex % map.xSize == 0 && index % xSize == 0) || (chunkIndex % map.xSize == map.xSize - 1 && index % xSize == xSize - 1) || (chunkIndex / ((map.ySize - 1) * map.xSize) >= 1 && index / xSize >= ySize - 1) || (chunkIndex < map.xSize && index < xSize))
                {

					/*if (index % 2 == 0 && index >= 10)
                    {
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1 - xSize].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1 - xSize].tileType]) * 0.5f);

						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
					}
					else if (index <= 90 && index <= 90)
                    {
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);

						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1 + xSize].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1 + xSize].tileType]) * 0.5f);
					}
					else
                    {
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					}*/


					/*colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);*/

					colors.Add(Color.yellow);
					colors.Add(Color.yellow);
					colors.Add(Color.yellow);

					colors.Add(Color.yellow);
					colors.Add(Color.yellow);
					colors.Add(Color.yellow);

					colors.Add(Color.yellow);
					colors.Add(Color.yellow);

					colors.Add(Color.yellow);
					colors.Add(Color.yellow);

					colors.Add(Color.yellow);
					colors.Add(Color.yellow);

					colors.Add(Color.yellow);
					colors.Add(Color.yellow);

					colors.Add(Color.yellow);
					colors.Add(Color.yellow);

					colors.Add(Color.yellow);
					colors.Add(Color.yellow);
				}
				else
                {
					if (index % xSize == xSize - 1 || index / xSize >= ySize - 1)
                    {
						colors.Add(Color.yellow);
						colors.Add(Color.yellow);
						colors.Add(Color.yellow);

						colors.Add(Color.yellow);
						colors.Add(Color.yellow);
						colors.Add(Color.yellow);

						colors.Add(Color.yellow);
						colors.Add(Color.yellow);

						colors.Add(Color.yellow);
						colors.Add(Color.yellow);

						colors.Add(Color.yellow);
						colors.Add(Color.yellow);

						colors.Add(Color.yellow);
						colors.Add(Color.yellow);

						colors.Add(Color.yellow);
						colors.Add(Color.yellow);

						colors.Add(Color.yellow);
						colors.Add(Color.yellow);
					}
					else if (index < xSize)
                    {
						if (index == 0)
						{
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - map.xSize - 1].tiles[index - 1 + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - map.xSize - 1].tiles[index - 1 + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) / 3);

							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + 1 + (xSize * ySize)].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + 1 + (xSize * ySize)].tileType]) / 3);

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize - 1].tiles[index - 1 + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize - 1].tiles[index - 1 + (xSize * ySize)].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index + 1 - xSize + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index + 1 - xSize + (xSize * ySize)].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
						}
						else if (index % 2 == 0)
						{
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize - 1 + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - 1 - xSize + (xSize * ySize)].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) / 3);

							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + 1 + (xSize * ySize)].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + 1 + (xSize * ySize)].tileType]) / 3);

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize - 1 + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize - 1 + (xSize * ySize)].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index + 1 - xSize + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index + 1 - xSize + (xSize * ySize)].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
						}
						else
						{
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1 + xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1 + xSize].tileType]) / 3);

							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1 + xSize].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1 + xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType]) / 3);

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1 + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1 + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
						}
					}
					else if (index % xSize == 0)
                    {
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize].tileType]) / 3);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1].tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType]) / 3);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) / 3);

						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) / 3);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize + 1].tileType]) / 3);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize + 1].tileType]) / 3);

						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1].tileType], 0.5f));

						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType], 0.5f));

						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));

						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));

						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1 - xSize].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1 - xSize].tileType], 0.5f));

						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - xSize].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - xSize].tileType], 0.5f));
					}
					else
                    {
						if (index % 2 == 0)
						{
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize - 1].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1 - xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) / 3);

							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize + 1].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize + 1].tileType]) / 3);

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - xSize - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - xSize - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1 - xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1 - xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - xSize].tileType], 0.5f));
						}
						else
						{
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1 + xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1 + xSize].tileType]) / 3);

							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1 + xSize].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1 + xSize].tileType]) / 3);
							colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize].tileType] + colorDict[data.map[chunkIndex].tiles[index + 1].tileType]) / 3);

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + xSize].tileType], 0.5f));
							
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1 + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1 + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index + 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[data.map[chunkIndex].tiles[index].tileType], colorDict[data.map[chunkIndex].tiles[index - xSize].tileType], 0.5f));
						}
					}
					/*colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);*/
				}
				/*if ((colNum % 2 == 0 && (index / 10) % 2 == 0) || (colNum % 2 != 0 && (index / 10) % 2 != 0))
				{
					//top left and top right are same row
					if (index % xSize != 0 && index >= 10)
					{
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize - 1].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - xSize - 1].tileType]) * 0.5f);

						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
					}
					else if (index % xSize == 0)
                    {
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					}
					else
                    {
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
					}
                }
				else if ((colNum % 2 != 0 && (index / 10) % 2 == 0) || (colNum % 2 == 0 && (index / 10) % 2 != 0))
				{
					//bottom left and bottom right are same row
					if (index % xSize == 0)
					{
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					}
					else if (index / 10 != ySize - 1)
					{
						Debug.Log("INdex: " + index );
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);

						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
					}
					else if (index / 10 = map.ySize)
					{
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);

						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					}
					else
					{
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
						colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
						colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
					}
				}*/


				/*if (index % xSize != 0 && index / xSize >= 1 && index / xSize != map.xSize - 1 && y % 2 == 0)
				{
					colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
					colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);

					colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) * 0.5f);
					colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) * 0.5f);
				}
				else if (index % xSize != 0 && index / xSize >= 1 && index / xSize != map.xSize - 1 && y % 2 == 0)
				{
					colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) * 0.5f);
					colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index + xSize].tileType]) * 0.5f);

					colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
					colors.Add((colorDict[data.map[chunkIndex].tiles[index].tileType] + colorDict[data.map[chunkIndex].tiles[index - 1].tileType]) * 0.5f);
				}
				else
				{
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
					colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
				}*/

				/*colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);

				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);
				colors.Add(colorDict[data.map[chunkIndex].tiles[index].tileType]);*/

				colNum++;
			}
		}

		//mesh.subMeshCount = 2;
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