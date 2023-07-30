using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class MapEditorChunk : MonoBehaviour
{
	Data data;
	MapEditorGeneration map;

	int xSize, ySize;
	public int chunkIndex;
	public Vector2 chunkPosition;

	public enum HexDirection { NE, S, SE, SW, W, NW }

	private void Awake()
	{
		data = Data.Instance;
		map = MapEditorGeneration.Instance;

		chunkIndex = map.chunkIndex;
		
	}
	void Start()
	{
		xSize = map.chunkXSize;
		ySize = map.chunkYSize;
		RenderMap();
	}

	private List<Vector3> vertices;
	private Mesh mesh;
	public GameObject labelPrefab;

	private void RenderMap()
	{
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		GetComponent<MeshCollider>().sharedMesh = mesh;

		mesh.name = "Chunk";
		mesh.Clear();

		vertices = new List<Vector3>();
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

		float blendRegion = 0.1f;

		Color grassColor = Color.green;
		Color hillColor = new Color(0.3f, 0.3f, 0.3f);
		Color mountainColor = Color.gray;
		Color waterColor = Color.blue;

		Dictionary<string, Color> colorDict = new Dictionary<string, Color>();
		colorDict["water"] = Color.blue;
		colorDict["coast"] = new Color32(51, 196, 255, 255);
		colorDict["snow"] = new Color32(185, 235, 255, 255);
		colorDict["tundra"] = new Color32(180, 184, 205, 255);
		colorDict["desert"] = new Color32(223, 197, 76, 255);
		colorDict["grass"] = Color.green;
		colorDict["forest"] = new Color32(73, 156, 42, 255);
		colorDict["mountain"] = Color.gray;

		for (int i = 0, ti = 0, index = 0; y < ySize; y++)
		{
			colNum = 0;
			for (x = 0; colNum < xSize; x += offsetSide + offsetEdge, i += 25, ti += 12, index++)
			{
				if (colNum % 2 != 0) y += (0.5f * height);

				Data.Chunk chunk = data.map[chunkIndex];
				string tileType = chunk.tiles[index].tileType;

				float distBetween;
				float inwardDist;
				float xDist;
				float yDist;
				Vector3 loc;

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

				Test(1,7, false, true);
				Test(2,8, true, false);

				//15
				Test(2,8, true, true);
				Test(3,9, false, false);

				//17
				loc = new Vector3(vertices[i + 3].x, vertices[i + 9].y);
				vertices.Add(loc);

				loc = new Vector3(vertices[i + 4].x, vertices[i + 10].y);
				vertices.Add(loc);

				//19
				Test(4,10, true, false);
				Test(5,11, false, true);

				//21
				Test(5,11,false, false);
				Test(6,12,true, true);

				void Test(int a, int b, bool c, bool d)
				{
					distBetween = Vector3.Distance(vertices[i + a], vertices[i + b]);
					inwardDist = Mathf.Cos(60 * Mathf.Deg2Rad) * distBetween;
					xDist = Mathf.Cos(60 * Mathf.Deg2Rad) * inwardDist;
					yDist = Mathf.Sin(60 * Mathf.Deg2Rad) * inwardDist;
					if (c)
					{
						if (d) loc = new Vector3(vertices[i + b].x + xDist, vertices[i + b].y + yDist);
						else loc = new Vector3(vertices[i + b].x + xDist, vertices[i + b].y - yDist);
					}
					else
					{
						if (d) loc = new Vector3(vertices[i + b].x - xDist, vertices[i + b].y + yDist);
						else loc = new Vector3(vertices[i + b].x - xDist, vertices[i + b].y - yDist);
					}
					vertices.Add(loc);
				}

				//23
				loc = new Vector3(vertices[i + 6].x, vertices[i + 12].y);
				vertices.Add(loc);

				loc = new Vector3(vertices[i + 1].x, vertices[i + 7].y);
				vertices.Add(loc);

				triangles.AddRange(new[] 
				{
					i, i + 1, i + 2,
					i, i + 2, i + 3,
					i, i + 3, i + 4,
					i, i + 4, i + 5,
					i, i + 5, i + 6,
					i, i + 6, i + 1,
					i + 1, i + 14, i + 2,
					i + 13, i + 14, i + 1,
					i + 1, i + 7, i + 13,
					i + 14, i + 8, i + 2,
					i + 2, i + 15, i + 16,
					i + 2, i + 16, i + 3,
					i + 2, i + 8, i + 15,
					i + 3, i + 16, i + 9,
					i + 3, i + 17, i + 18,
					i + 3, i + 18, i + 4,
					i + 3, i + 9, i + 17,
					i + 4, i + 18, i + 10,
					i + 4, i + 19, i + 20,
					i + 4, i + 20, i + 5,
					i + 4, i + 10, i + 19,
					i + 5, i + 20, i + 11,
					i + 5, i + 21, i + 22,
					i + 5, i + 22, i + 6,
					i + 5, i + 11, i + 21,
					i + 6, i + 22, i + 12,
					i + 6, i + 23, i + 24,
					i + 6, i + 24, i + 1,
					i + 6, i + 12, i + 23,
					i + 1, i + 24, i + 7
				});
				#endregion

				GameObject label = Instantiate(labelPrefab);
				label.transform.parent = transform;
				label.transform.localPosition = new Vector3(x, y);
				label.GetComponent<TextMeshPro>().text = index.ToString();

				if (colNum % 2 != 0) y -= (0.5f * height);

				for (int r = 0; r < 7; r++)
				{
					colors.Add(colorDict[tileType]);
				}

				if ((chunkIndex % map.xSize == 0 && index % xSize == 0) || (chunkIndex % map.xSize == map.xSize - 1 && index % xSize == xSize - 1) || (chunkIndex / ((map.ySize - 1) * map.xSize) >= 1 && index / xSize >= ySize - 1) || (chunkIndex < map.xSize && index < xSize))
				{
					for (int yellow = 0; yellow < 18; yellow++)
					{
						colors.Add(Color.yellow);
					}
				}
				else
				{
					if (index % xSize == xSize - 1 || index / xSize >= ySize - 1)
					{
						for (int yellow = 0; yellow < 18; yellow++)
						{
							colors.Add(Color.yellow);
						}
					}
					else if (index < xSize)
					{
						if (index == 0)
						{
							colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - map.xSize - 1].tiles[index - 1 + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - map.xSize - 1].tiles[index - 1 + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType] + colorDict[chunk.tiles[index + xSize].tileType]) / 3);

							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1].tileType] + colorDict[chunk.tiles[index + xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + 1 + (xSize * ySize)].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + 1 + (xSize * ySize)].tileType]) / 3);

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize - 1].tiles[index - 1 + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize - 1].tiles[index - 1 + (xSize * ySize)].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index + 1 - xSize + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index + 1 - xSize + (xSize * ySize)].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
						}
						else if (index % 2 == 0)
						{
							colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize - 1 + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - 1].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - 1 - xSize + (xSize * ySize)].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - 1].tileType] + colorDict[chunk.tiles[index + xSize].tileType]) / 3);

							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1].tileType] + colorDict[chunk.tiles[index + xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + 1 + (xSize * ySize)].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + 1 + (xSize * ySize)].tileType]) / 3);

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize - 1 + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize - 1 + (xSize * ySize)].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index + 1 - xSize + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index + 1 - xSize + (xSize * ySize)].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
						}
						else
						{
							colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType] + colorDict[chunk.tiles[index - 1].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - 1].tileType] + colorDict[chunk.tiles[index - 1 + xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + xSize].tileType] + colorDict[chunk.tiles[index - 1 + xSize].tileType]) / 3);

							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1 + xSize].tileType] + colorDict[chunk.tiles[index + xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1].tileType] + colorDict[chunk.tiles[index + 1 + xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType] + colorDict[chunk.tiles[index + 1].tileType]) / 3);

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1 + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1 + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - map.xSize].tiles[index - xSize + (xSize * ySize)].tileType], 0.5f));
						}
					}
					else if (index % xSize == 0)
					{
						colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1].tileType] + colorDict[chunk.tiles[index - xSize].tileType]) / 3);
						colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1].tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType]) / 3);
						colors.Add((colorDict[tileType] + colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType] + colorDict[chunk.tiles[index + xSize].tileType]) / 3);

						colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1].tileType] + colorDict[chunk.tiles[index + xSize].tileType]) / 3);
						colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1].tileType] + colorDict[chunk.tiles[index - xSize + 1].tileType]) / 3);
						colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - xSize].tileType] + colorDict[chunk.tiles[index - xSize + 1].tileType]) / 3);

						colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1].tileType], 0.5f));

						colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[tileType], colorDict[data.map[chunkIndex - 1].tiles[index - 1 + xSize].tileType], 0.5f));

						colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));

						colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));

						colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1 - xSize].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1 - xSize].tileType], 0.5f));

						colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - xSize].tileType], 0.5f));
						colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - xSize].tileType], 0.5f));
					}
					else
					{
						if (index % 2 == 0)
						{
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - xSize - 1].tileType] + colorDict[chunk.tiles[index - xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - 1].tileType] + colorDict[chunk.tiles[index - 1 - xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - 1].tileType] + colorDict[chunk.tiles[index + xSize].tileType]) / 3);

							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1].tileType] + colorDict[chunk.tiles[index + xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1].tileType] + colorDict[chunk.tiles[index - xSize + 1].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - xSize].tileType] + colorDict[chunk.tiles[index - xSize + 1].tileType]) / 3);

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - xSize - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - xSize - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1 - xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1 - xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - xSize].tileType], 0.5f));
						}
						else
						{
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - xSize].tileType] + colorDict[chunk.tiles[index - 1].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - 1].tileType] + colorDict[chunk.tiles[index - 1 + xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + xSize].tileType] + colorDict[chunk.tiles[index - 1 + xSize].tileType]) / 3);

							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1 + xSize].tileType] + colorDict[chunk.tiles[index + xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index + 1].tileType] + colorDict[chunk.tiles[index + 1 + xSize].tileType]) / 3);
							colors.Add((colorDict[tileType] + colorDict[chunk.tiles[index - xSize].tileType] + colorDict[chunk.tiles[index + 1].tileType]) / 3);

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize - 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize - 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1 + xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1 + xSize].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index + 1].tileType], 0.5f));

							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - xSize].tileType], 0.5f));
							colors.Add(Color.Lerp(colorDict[tileType], colorDict[chunk.tiles[index - xSize].tileType], 0.5f));
						}
					}
				}
				colNum++;
			}
		}

		mesh.vertices = vertices.ToArray();
		mesh.colors = colors.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
	}
}