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
	void Start () 
	{
		GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
		Debug.Log("EndturnScript loaded!");
        unit1 = units[0].GetComponent<UnityUnit>();
        unit2 = units[1].GetComponent<UnityUnit>();
        unit3 = units[2].GetComponent<UnityUnit>();

		Button endTurnBtn = endTurnButton.GetComponent<Button>();
		endTurnBtn.onClick.AddListener(EndTurn);
	}
	
	// Update is called once per frame
	public void EndTurn()
	{
		Debug.Log("Endturn Button clicked!");
		unit1.CurrentActionPoints = unit1.MaxActionPoints;
		unit2.CurrentActionPoints = unit2.MaxActionPoints;
		unit3.CurrentActionPoints = unit3.MaxActionPoints;
	}
}
