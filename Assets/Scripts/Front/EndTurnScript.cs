using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnScript : MonoBehaviour {
	private UnityUnit unit1;
	private UnityUnit unit2;
	private UnityUnit unit3;
	public Button endTurnButton;
	// Use this for initialization
	private List<UnityUnit> units;
	private GameObject[] gameObjectUnits;
	void Start () 
	{
		units = new List<UnityUnit>();
		gameObjectUnits = GameObject.FindGameObjectsWithTag("Unit");

		foreach (GameObject unit in gameObjectUnits)
		{
			units.Add(unit.GetComponent<UnityUnit>());
		}

		Debug.Log("EndturnScript loaded!");
        

		Button endTurnBtn = endTurnButton.GetComponent<Button>();
		endTurnBtn.onClick.AddListener(EndTurn);
	}
	
	// Update is called once per frame
	public void EndTurn()
	{
		foreach (UnityUnit unit in units)
		{
			
			unit.CurrentMovePoints = unit.MaxMovePoints;

		}
		Debug.Log("Endturn Button clicked!");
		
	}
}
