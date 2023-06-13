using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Data;

public class UI : MonoBehaviour
{
	Data data;
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

	public Country playerCountry = Data.nullCountry;

	public static UI Instance { get; private set; }
	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		data = Data.Instance;
		cam = CameraController.Instance;
	}


	void Start()
	{
		playerCountry = data.countries["USA"];
		UpdateResourceBar();

		leaderImage.sprite = null;
	}

	public void UpdateResourceBar()
	{
		politicalPower.text = playerCountry.leader.politicalPower.ToString();
	}

	public void PoliticalPanelButton()
	{
		if (politicalPanelOut && !displayingCountry.Equals(playerCountry))
		{
			UpdatePoliticalPanel(playerCountry);
		}
		else
		{
			UpdatePoliticalPanel(playerCountry);
			StartCoroutine(PoliticalPanelTransition());
		}
	}

	public void ResearchPanel1Button()
	{
		StartCoroutine(ResearchPanel1Transition());
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
		if (!country.Equals(Data.nullCountry) || !country.Equals(playerCountry))
		{
			displayingCountry = country;
			leaderName.text = country.leader.name;
			countryText.text = country.name;
			leaderImage.sprite = country.leader.leaderImage;
		}
		else
		{
			displayingCountry = playerCountry;
			leaderName.text = playerCountry.leader.name;
			countryText.text = playerCountry.name;
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

	public void DisplayCity(Data.City city)
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
			//cam.canZoom = true;
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
			//cam.canZoom = false;
			researchPanel1Sliding = false;
			StopCoroutine(ResearchPanel1Transition());
		}
	}
}