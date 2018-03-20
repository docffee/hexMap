using UnityEngine;
using UnityEngine.UI;

public class EndTurnScript : MonoBehaviour
{
	public Button endTurnButton;

	private void Start () 
	{   
		Button endTurnBtn = endTurnButton.GetComponent<Button>();
		endTurnBtn.onClick.AddListener(EndTurn);
	}
	
	public void EndTurn()
	{
        GameObject[] gameObjectUnits = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject go in gameObjectUnits)
        {
            UnityUnit unit = go.GetComponent<UnityUnit>();
            unit.CurrentActionPoints = unit.MaxActionPoints;
        }

		Debug.Log("Endturn Button clicked!");
	}
}
