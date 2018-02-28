using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections.Generic;
using UnityEngine;

class UnitController : MonoBehaviour
{
    IUnit<HexNode> selectedUnit = null;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Unit")))
            {
                Debug.Log("Unit selected");
                selectedUnit = hit.collider.gameObject.GetComponent<IUnit<HexNode>>();
            }
            else if (selectedUnit != null && !selectedUnit.PerformingAction() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Tile")))
            {
                HexCell cell = hit.collider.gameObject.GetComponent<HexCell>();

                Debug.Log("Hit cell at: " + cell.X + ", " + cell.Z);

                HexEngine engine = HexEngine.Singleton;
                IEnumerable<IPathNode<HexNode>> path = engine.MoveUnit(selectedUnit, selectedUnit.Tile, cell);
                selectedUnit.Move(path);
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            Debug.Log("Unit deselected");
            selectedUnit = null;
        }
    }
}
