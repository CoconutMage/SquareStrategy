using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GM : MonoBehaviour
{
	public static GM Instance { get; private set; }
	public UnityEvent tickEvent = new();
	int tickCounter = 0;
	public int tps = 50;

	MapData data;
	CameraController cam;
	UI ui;

	public TMP_Text b0inCounter;
	public TMP_Text b0inCounter2;
	public TMP_Text l0anCounter;
	public TMP_Text dateText;

	public TMP_Text researchText;

	readonly string[] months = new string[]
	{
		"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
	};

	readonly int[] daysInMonth = new int[]
	{
		31,28,31,30,31,30,31,31,30,31,30,31
	};

	int monthDays = 0;
	int totalDays;
	int month;
	int year = 1950;

	/*-------SERIALIZE EVERYTHING PAST HERE-------*/
	//STATS

	public Dictionary<string, BigInteger> stats = new Dictionary<string, BigInteger>
	{
		{"b0ins", 0},
		{"researchPoints", 0},
		{"loan", 0},

		{"satellites", 0}
	};

	public Dictionary<string, bool> boolStats = new Dictionary<string, bool>
	{
		{"hasLoan", false}
	};


	/*
	public BigInteger b0ins;
	public BigInteger researchPoints;

	public BigInteger metal;
	public BigInteger advancedAlloys;

	public BigInteger liquidFuel;
	public BigInteger solidFuel;
	public BigInteger nuclearFuel;

	public BigInteger m;
	*/

	//Societal resources
	//Construction

	/*-------FUNCTIONS, NO NEED TO SERIALIZE-------*/

	void Start()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;
		tickEvent.AddListener(Tick);

		data = MapData.Instance;
		cam = CameraController.Instance;
		ui = UI.Instance;
	}

	void Update()
	{
		GlobalResourceManager();
		//PlanetResourceManager();
	}

	public void FixedUpdate()
	{
		tickCounter++;
		if (tickCounter >= tps)
		{
			tickEvent.Invoke();
			tickCounter = 0;
		}
	}

	public void Tick()
	{
		TickCalendar();
		dateText.text = months[month] + "/" + monthDays + "/" + year;
	}

	void GlobalResourceManager()
	{
		stats.TryGetValue("researchPoints", out BigInteger rp);
		stats.TryGetValue("b0ins", out BigInteger b0ins);
		stats.TryGetValue("loan", out BigInteger loan);

		researchText.text = "Research: " + rp;
		b0inCounter.text = "$" + b0ins;
		b0inCounter2.text = "$" + b0ins;
		l0anCounter.text = "$" + loan;
	}

	/*
	void PlanetResourceManager()
	{
		foreach (string key in data.SolarSystem.Keys)
		{
			CelestialBody body = data.SolarSystem[key];

			//Debug.Log(body.name + ": " + body.buildings["Factories"]);
			if (body.buildings["Metal Mines"] > 0)
			{
				body.resources["Metal"] += body.buildings["Metal Mines"];
			}
		}

		if (cam.planetLocked) ui.UpdatePlanetPanelText(cam.planetLocked.name);
	}
	*/

	void TickCalendar()
	{
		//Debug.Log(DateTime.Now.Millisecond);
		//Debug.Log(months[month] + "/" + monthDays + "/" + year);

		monthDays++;
		totalDays++;

		if (year % 4 == 0 && month == 1)
		{
			if (monthDays > daysInMonth[month] + 1)
			{
				FlipPage();
			}
		}
		else if (monthDays > daysInMonth[month])
		{
			FlipPage();
		}

		void FlipPage()
		{
			if (month == 11 && monthDays >= 31)
			{
				year++;
				month = 0;
				monthDays = 1;
				NewYear();
			}
			else
			{
				monthDays = 1;
				month++;
			}
		}
	}

	void NewYear()
	{

	}
}