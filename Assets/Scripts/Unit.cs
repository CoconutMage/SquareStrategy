using UnityEngine;

public class Unit : MonoBehaviour
{
    //UnitData uData;
    public UnitData.ArmoredUnit thisUnit;

    void Start()
    {
       //uData = UnitData.Instance; 
    }

    void Update()
    {
        
    }

	void OnMouseDown()
	{
		if (Input.GetKey("mouse 0"))
		{
			print("Stats: " + thisUnit.armor + ", " + thisUnit.speed + ", " + thisUnit.gun + ", " + thisUnit.reliability);
		}
	}
}
