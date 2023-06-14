using System.Collections.Generic;
using UnityEngine;
using static Data;

public class UnitData : MonoBehaviour
{
	public static UnitData Instance { get; private set; }

	public Dictionary<int, ArmoredUnit> armoredUnits;

	private void Awake()
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
		armoredUnits = new Dictionary<int, ArmoredUnit>();
	}

	public struct InfantryUnit
	{
		public string name;

		public int manpower;

		public float supplies;

		public InfantryUnit(string aa, int ab, float ac)
		{
			name = aa;
			manpower = ab;
			supplies = ac;
		}
	}

	public void CreateArmoredUnit(string name, int manpower, float supplies, int a, int s, int g, int r)
	{
		int i = 0;
		armoredUnits[i] = new ArmoredUnit(name, manpower, supplies, a, s, g, r);
		i++;
	}

	public struct ArmoredUnit
	{
		public string name;

		public int manpower;

		public float supplies;

		public int armor;
		public int speed;
		public int gun;
		public int reliability;

		public ArmoredUnit(string aa, int ab, float ac, int ad, int ae, int af, int ag)
		{
			name = aa;
			manpower = ab;
			supplies = ac;

			armor = ad;
			speed = ae;
			gun = af;
			reliability = ag;
		}
	}
}