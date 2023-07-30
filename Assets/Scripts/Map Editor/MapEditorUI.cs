using System.IO;
using TMPro;
using UnityEngine;
using static Data;

public class MapEditorUI : MonoBehaviour
{
	Data data;
	CameraController cam;

	MapEditorGeneration mapEditorMap;

	public static MapEditorUI Instance { get; private set; }

	string mapFilePath = "Assets/Resources/Maps/TestMap/Map.txt";
	string countryFilePath = "Assets/Resources/Maps/TestMap/Countries.txt";
	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;
	}

	void Start()
	{
		data = Data.Instance;
		cam = CameraController.Instance;
		mapEditorMap = MapEditorGeneration.Instance;
	}

	public void ExportMapToFile()
	{
		File.WriteAllText(mapFilePath, "");
		File.WriteAllText(countryFilePath, "");

		string mapText = "";
		string countryText = "";

		//Map Saving
		mapText += mapEditorMap.xSize + "x" + mapEditorMap.ySize + "\n";

		float y = 0;
		float x;
		float colNum = 0;
		int[,] mapArray = new int[MapEditorGeneration.Instance.chunkXSize * MapEditorGeneration.Instance.xSize, MapEditorGeneration.Instance.chunkYSize * MapEditorGeneration.Instance.ySize];

		for (int chunkIndex = 0; chunkIndex < data.map.Count; chunkIndex++)
		{
			for (int i = 0, ti = 0, index = 0; y < MapEditorGeneration.Instance.chunkYSize; y++)
			{
				colNum = 0;
				for (x = 0; colNum < MapEditorGeneration.Instance.chunkXSize; x += 1, i += 25, ti += 12, index++)
				{
					float xCord = x + (MapEditorGeneration.Instance.chunkXSize * (chunkIndex % MapEditorGeneration.Instance.xSize)), 
						yCord = y + (MapEditorGeneration.Instance.chunkYSize * (chunkIndex / MapEditorGeneration.Instance.xSize));

					//mapArray[(int)xCord, (int)yCord] = data.map[chunkIndex].tiles[index].tileTypeID;

					if (data.map[chunkIndex].tiles[index].tileType == "water") mapArray[(int)xCord, (int)yCord] = 16;
					else if (data.map[chunkIndex].tiles[index].tileType == "coast") mapArray[(int)xCord, (int)yCord] = 15;
					else if (data.map[chunkIndex].tiles[index].tileType == "snow") mapArray[(int)xCord, (int)yCord] = 14;
					else if (data.map[chunkIndex].tiles[index].tileType == "snow") mapArray[(int)xCord, (int)yCord] = 13;
					else if (data.map[chunkIndex].tiles[index].tileType == "snow") mapArray[(int)xCord, (int)yCord] = 12;
					else if (data.map[chunkIndex].tiles[index].tileType == "tundra") mapArray[(int)xCord, (int)yCord] = 11;
					else if (data.map[chunkIndex].tiles[index].tileType == "tundra") mapArray[(int)xCord, (int)yCord] = 10;
					else if (data.map[chunkIndex].tiles[index].tileType == "tundra") mapArray[(int)xCord, (int)yCord] = 9;
					else if (data.map[chunkIndex].tiles[index].tileType == "desert") mapArray[(int)xCord, (int)yCord] = 8;
					else if (data.map[chunkIndex].tiles[index].tileType == "desert") mapArray[(int)xCord, (int)yCord] = 7;
					else if (data.map[chunkIndex].tiles[index].tileType == "desert") mapArray[(int)xCord, (int)yCord] = 6;
					else if (data.map[chunkIndex].tiles[index].tileType == "forest") mapArray[(int)xCord, (int)yCord] = 5;
					else if (data.map[chunkIndex].tiles[index].tileType == "forest") mapArray[(int)xCord, (int)yCord] = 4;
					else if (data.map[chunkIndex].tiles[index].tileType == "forest") mapArray[(int)xCord, (int)yCord] = 3;
					else mapArray[(int)xCord, (int)yCord] = 1;

					colNum++;
				}
			}
		}

		for (int yy = 0; yy < mapArray.GetLength(0); yy++)
		{
			for (int xx = 0; xx < mapArray.GetLength(1); xx++)
			{
				mapText += mapArray[xx, yy] + ", ";
			}
			mapText += "\n";
		}

		// Write to disk
		StreamWriter mapWriter = new(mapFilePath, false);
		mapWriter.Write(mapText);
		mapWriter.Close();


		string tab = "\t";
		//Country Saving
		foreach (Country country in data.countries.Values)
		{
			countryText += "{\n\t";
			countryText += country.countryID + ", ";
			countryText += country.countryTag + ", ";
			countryText += country.countryName + ", ";

			countryText += country.countryLeader.name + "\n\t{";

			foreach (Tile tile in data.countries[country.countryTag].countryTiles.Values)
			{
				countryText += tab + tab + tile.chunkIndex + ", ";
				countryText += tab + tab + tile.index + ", ";
				//coordinates should go here but they might end up being depreciated
				//tile type shouldnt be needed
				//neither should parent country
				countryText += tab + tab + tile.city;
				//public Dictionary<string, int> tileBuildings;
			}
			countryText += "\n\t\tTILE DATA HERE";

			countryText += "\n\t}\n";
			if(country.countryID != data.countries.Count - 1) countryText += "},\n";
			else countryText += "}";
		}

		// Write to disk
		StreamWriter countryWriter = new(countryFilePath, false);
		countryWriter.Write(countryText);
		countryWriter.Close();
	}

	public void LoadMapFromFile()
	{
		/*
		int lineInFile = 0;
		using (StreamReader r = new(mapFilePath))
		{
			while (r.ReadLine() != null) lineInFile++;
		}

		StreamReader reader = new(mapFilePath);

		string[] temp = new string[5];
		for (int i = 0; i < lineInFile; i++)
		{
			string currentLine = reader.ReadLine();
			string[] lineSplit = currentLine.Split(",");

			temp = lineSplit;
		}

		int ii = 0;
		foreach (var i in temp)
		{
			Debug.Log(temp[ii]);
			ii++;
		}
		*/
	}
}

/*
public void ExportMapToFile()
{
	File.WriteAllText(mapFilePath, "");

	string mapText = "";
	foreach (Data.Chunk c in data.map.Values)
	{
		foreach (Data.Tile t in data.map[c.index].tiles.Values)
		{
			//mapText += "{";
			mapText += t.chunkIndex + ", " + t.index + ", ";
			//mapText += t.coordinates.x + ", " + t.coordinates.y + ", ";
			mapText += t.tileType + ", ";

			mapText += t.parentCountry.countryID + ", ";
			mapText += t.parentCountry.countryTag + ", ";
			mapText += t.city.cityName; //+ ",";

			if (t.index == data.map[c.index].tiles.Values.Count - 1 && c.index == data.map.Count - 1)
			{
				//mapText += "}";
			}
			else
			{
				//mapText += "},\n";
				mapText += "\n";
			}
		}
	}


	// Write to disk
	StreamWriter writer = new StreamWriter(mapFilePath, false);
	writer.Write(mapText);
	writer.Close();
}
*/