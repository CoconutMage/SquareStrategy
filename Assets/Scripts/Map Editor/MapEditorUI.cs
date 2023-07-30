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
		for (int y = 0; y < data.map.Count; y++)
		{
			for (int x = 0; x < data.map[y].tiles.Count; x++)
			{
				/*
				if (x == 7) { y++; x = 0; }
				if (y == 3) { y = 0;x = 1; }
				*/
				Tile t = data.map[y].
					tiles[x];
				//mapText += "{";
				//mapText += t.chunkIndex + ", " + t.index + ", ";
				//mapText += "(" + t.coordinates.x + ", " + t.coordinates.y + ") ";
				mapText += t.tileType + ", ";

				//mapText += t.parentCountry.countryID + ", ";
				//mapText += t.parentCountry.countryTag + ", ";
				//mapText += t.city.cityName; //+ ",";
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