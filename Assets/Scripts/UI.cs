using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
	MapData data;

	public TMP_Text landUnitsText;
	public TMP_Text airUnitsText;
	public TMP_Text waterUnitsText;
	public TMP_Text resourceText;

	public TMP_Text cityText;
	public TMP_Text countryText;
	public TMP_Text navyText;

	public static UI Instance { get; private set; }
	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		data = MapData.Instance;
	}

	public void DisplayUnits(Dictionary<string, int> landUnits, Dictionary<string, int> airUnits)
	{
		string temp = "";
		int i = 0;

		foreach (var kvp in landUnits)
		{
			temp += kvp.Key;
			temp += ": ";
			temp += kvp.Value;
			i++;
			if (i != landUnits.Count)
			{
				temp += "\n";
			}
		}

		string temp2 = "";
		foreach (var kvp in airUnits)
		{
			temp2 += kvp.Key;
			temp2 += ": ";
			temp2 += kvp.Value;
			i++;
			if (i != airUnits.Count)
			{
				temp2 += "\n";
			}
		}

		landUnitsText.text = temp;
		airUnitsText.text = temp2;
	}

	public void DisplayResources(Dictionary<string, int> resources)
	{
		string temp = "";
		int i = 0;

		foreach (var kvp in resources)
		{
			temp += kvp.Key;
			temp += ": ";
			temp += kvp.Value;
			i++;
			if (i != resources.Count)
			{
				temp += "\n";
			}
		}
		resourceText.text = temp;
	}

	public void DisplayCountryStatistics(int countryID)
	{ 
		
	}

	public void DisplayCity(CountryData.City city)
	{
		if (!city.Equals(CountryData.nullCity))
		{
			cityText.text = city.name;
			countryText.text = city.parentCountry.name + "\n" + city.parentCountry.leader.name;

			Bazinga();
		}
		else
		{
			cityText.text = "";
			countryText.text = "";
			navyText.text = "";
		}

		void Bazinga()
		{
			string temp = "";
			CountryData.Country country = city.parentCountry;

			int i = 0;
			foreach (var navalUnit in country.Navy)
			{
				temp += navalUnit.Key;
				temp += ": ";
				temp += navalUnit.Value;
				i++;
				if (i != country.Navy.Count)
				{
					temp += "\n";
				}
			}

			navyText.text = temp;
		}
	}

}
