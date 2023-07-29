using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Data;

public class UI : MonoBehaviour
{
	Data data;
	UnitData uData;
	CameraController cam;

	public TMP_Text landUnitsText;
	public TMP_Text airUnitsText;
	public TMP_Text waterUnitsText;
	public TMP_Text resourceText;

	public TMP_Text cityText;
	public TMP_Text countryText;
	public TMP_Text navyText;

	public RectTransform politicalPanel;
	public RectTransform researchPanel1;

	public TMP_Text politicalPower;
	public TMP_Text leaderName;

	public Image leaderImage;

	public Player playerOne = Data.nullPlayer;


	public static UI Instance { get; private set; }
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
		uData = UnitData.Instance;
		cam = CameraController.Instance;

		playerOne.country = data.countries["USA"];
		UpdateResourceBar();

		leaderImage.sprite = null;
	}

	public void UpdateResourceBar()
	{
		politicalPower.text = playerOne.politicalPower.ToString();
	}

	public void PoliticalPanelButton()
	{
		if (politicalPanelOut && !displayingCountry.Equals(playerOne.country))
		{
			UpdatePoliticalPanel(playerOne.country);
		}
		else
		{
			UpdatePoliticalPanel(playerOne.country);
			StartCoroutine(PoliticalPanelTransition());
		}
	}

	public void ResearchPanel1Button()
	{
		StartCoroutine(ResearchPanel1Transition());
	}


	public TMP_Text armor;
	public TMP_Text speed;
	public TMP_Text gun;
	public TMP_Text reliability;
	public TMP_Text bazinga;
	int a = 0;
	int s = 0;
	int g = 0;
	int r = 0;
	//This is a super work in progress/proof of concept combined with an experiment to see if I could make all the buttons in a menu use one function, which
	//doesn't cause any issues at the moment (even though it looks awful) but if it does or it's decided it needs to be changed, I will do so myself
	public void TankDesignerButtonHandler(int i)
	{
		switch (i)
		{
			case 0:
				if (a < 5)
				{
					a++;
					armor.text = a.ToString();
				}
				break;
			case 1:
				if (a > 0)
				{
					a--;
					armor.text = a.ToString();
				}
				break;

			case 2:
				if (s < 5)
				{
					s++;
					speed.text = s.ToString();
				}
				break;
			case 3:
				if (s > 0)
				{
					s--;
					speed.text = s.ToString();
				}
				break;

			case 4:
				if (g < 5)
				{
					g++;
					gun.text = g.ToString();
				}
				break;
			case 5:
				if (g > 0)
				{
					g--;
					gun.text = g.ToString();
				}
				break;

			case 6:
				if (r < 5)
				{
					r++;
					reliability.text = r.ToString();
				}
				break;
			case 7:
				if (r > 0)
				{
					r--;
					reliability.text = r.ToString();
				}
				break;
			
			case 8:
				uData.CreateArmoredUnit("Test", 0, 0.0f, a, s, g, r);
				break;
			case 9:
				Tile tile = data.map[0].tiles[0];
				tile.landArmoredUnits[0] = uData.armoredUnits[0];
				data.map[0].tiles[0] = tile;
				bazinga.text = tile.landArmoredUnits[0].armor + "\n" + tile.landArmoredUnits[0].speed + "\n" + tile.landArmoredUnits[0].gun + "\n" + tile.landArmoredUnits[0].reliability;
				GameObject tank = Instantiate(TankPrefab);
				tank.transform.parent = GameObject.Find("Map").transform;
				tank.transform.localPosition = data.map[0].tiles[0].coordinates;
				tank.GetComponent<Unit>().thisUnit = uData.armoredUnits[0];
				break;
		}
	}
	public GameObject TankPrefab;


	public RectTransform TankDesigner;
	bool td;
	public void DesignerButton()
	{
		td = !td;
		TankDesigner.gameObject.SetActive(td);
	}

	public void ClosePoliticalPanel()
	{
		if (politicalPanelOut)
		{
			StartCoroutine(PoliticalPanelTransition());
		}
	}

	public Country displayingCountry;

	void UpdatePoliticalPanel(Country country)
	{
		if (!country.Equals(Data.nullCountry) || !country.Equals(playerOne.country))
		{
			displayingCountry = country;
			leaderName.text = country.leader.name;
			countryText.text = country.name;
			leaderImage.sprite = country.leader.leaderImage;
		}
		else
		{
			displayingCountry = playerOne.country;
			leaderName.text = playerOne.country.leader.name;
			countryText.text = playerOne.country.name;
			leaderImage.sprite = country.leader.leaderImage;
		}
	}

	public void PoliticalPanelHandler(Country country)
	{

		if (politicalPanelOut)
		{
			displayingCountry = country;
			UpdatePoliticalPanel(country);
		}
		else
		{
			displayingCountry = country;
			UpdatePoliticalPanel(country);
			StartCoroutine(PoliticalPanelTransition());
		}
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

	public void DisplayCountryAndCityInfo(Tile tile)
	{
		cityText.text = "";
		countryText.text = "";

		if (!tile.city.Equals(nullCity))
		{
			cityText.text = tile.city.name;
		}
		if (!tile.parentCountry.Equals(nullCountry))
		{
			countryText.text = tile.parentCountry.name;
		}
	}

	public void DisplayCity(City city)
	{
		if (!city.Equals(Data.nullCity))
		{
			cityText.text = city.name;
			countryText.text = city.parentCountry.name ;

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
			Data.Country country = city.parentCountry;

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

	/* COROUTINES */
	public bool politicalPanelOut;
	bool politicalPanelSliding;
	Vector3 politicalPanelSlideSpeed = new(5, 0, 0);
	IEnumerator PoliticalPanelTransition()
	{
		if (politicalPanelOut && politicalPanelSliding != true)
		{
			politicalPanelSliding = true;
			for (int i = 0; i < 355 / politicalPanelSlideSpeed.x; i++)
			{
				politicalPanel.transform.position -= politicalPanelSlideSpeed;
				yield return null;
			}
			politicalPanelOut = false;
			politicalPanelSliding = false;
			StopCoroutine(PoliticalPanelTransition());
		}
		else if (!politicalPanelOut && politicalPanelSliding != true)
		{
			politicalPanelSliding = true;
			for (int i = 0; i < 355 / politicalPanelSlideSpeed.x; i++)
			{
				politicalPanel.transform.position += politicalPanelSlideSpeed;
				yield return null;
			}
			politicalPanelOut = true;
			politicalPanelSliding = false;
			StopCoroutine(PoliticalPanelTransition());
		}
	}

	public bool researchPanel1Out;
	bool researchPanel1Sliding;
	Vector3 researchPanel1SlideSpeed = new(0, 5, 0);
	int researchPanel1SlideAmount = 595;
	IEnumerator ResearchPanel1Transition()
	{
		if (researchPanel1Out && researchPanel1Sliding != true)
		{
			researchPanel1Sliding = true;
			for (int i = 0; i < researchPanel1SlideAmount / researchPanel1SlideSpeed.y; i++)
			{
				researchPanel1.transform.position -= researchPanel1SlideSpeed;
				yield return null;
			}
			researchPanel1Out = false;
			cam.canZoom = true;
			researchPanel1Sliding = false;
			StopCoroutine(ResearchPanel1Transition());
		}
		else if (!researchPanel1Out && researchPanel1Sliding != true)
		{
			researchPanel1Sliding = true;
			for (int i = 0; i < researchPanel1SlideAmount / researchPanel1SlideSpeed.y; i++)
			{
				researchPanel1.transform.position += researchPanel1SlideSpeed;
				yield return null;
			}
			researchPanel1Out = true;
			cam.canZoom = false;
			researchPanel1Sliding = false;
			StopCoroutine(ResearchPanel1Transition());
		}
	}
}