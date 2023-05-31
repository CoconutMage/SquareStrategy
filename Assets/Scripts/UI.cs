using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
	public TMP_Text unitCounter;

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

	public void DisplayUnits(int units)
	{ 
		unitCounter.text = "Tile Units: " + units.ToString();
	}

}
