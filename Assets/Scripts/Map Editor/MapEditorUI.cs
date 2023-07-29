using System.IO;
using TMPro;
using UnityEngine;

public class MapEditorUI : MonoBehaviour
{
	Data data;
	CameraController cam;

	public static MapEditorUI Instance { get; private set; }

	string mapFilePath = "Assets/Resources/Maps/Map.txt";
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
	}

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
				mapText += t.city.name; //+ ",";

				if(t.index == data.map[c.index].tiles.Values.Count - 1 && c.index == data.map.Count - 1)
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

	public void LoadMapFromFile()
	{
		int lineInFile = 0;
		using (StreamReader r = new StreamReader(mapFilePath))
		{
			while (r.ReadLine() != null) lineInFile++;
		}

		StreamReader reader = new StreamReader(mapFilePath);

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
	}

	/*
		public int chunkIndex;
		public int index;
		public Vector2 coordinates;
		public string tileType;

		public Country parentCountry;
		public City city;
		public Dictionary<string, int> resources;

		public Dictionary<int, UnitData.ArmoredUnit> landArmoredUnits;
		public Dictionary<string, int> landUnits;
		public Dictionary<string, int> airUnits;

		public Dictionary<string, int> tileBuildings;
	*/
}
