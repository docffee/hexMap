using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Front.Actions
{
    class Attack : IAction
    {
        private Unit unit;
        private bool hasControl;

        private ITileControl<HexNode> hexControl;
        private List<GameObject> highlightedTiles;

        public Attack(Unit unit, ITileControl<HexNode> hexControl)
        {
            this.unit = unit;
            this.hexControl = hexControl;
            hasControl = false;
            highlightedTiles = new List<GameObject>();
        }

        public string ActionName
        {
            get
            {
                return "Attack";
            }
        }

        public bool HasControl
        {
            get
            {
                return hasControl;
            }

            set
            {
                hasControl = value;
            }
        }

        public void TakeControl()
        {
            hasControl = true;
            UnitController.Singleton.SetAction(this);
            IEnumerable<IPathNode<HexNode>> reachable = hexControl.GetReachable(unit, unit.Tile);
            //HighlightTiles(reachable);
            unit.StartCoroutine(Controller());
        }

        private IEnumerator Controller()
        {
            while (hasControl)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit unitHit;

                    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
                        out unitHit, 200.0f, LayerMask.GetMask("Unit"));

                    if (unitHit.collider != null
                        && unitHit.collider.GetComponent<Unit>().Controller.Team != unit.Controller.Team)
                    {
                        if (unit.CurrentActionPoints >= unit.AttackActionPointCost)
                        {
                            Unit other = unitHit.collider.gameObject.GetComponent<Unit>();

                            int dist = HexHeuristic.MinDistTile(unit.Tile, other.Tile);
                            if (dist <= unit.Range)
                            {
                                IFireFight fireFight = new FireFightNoRetaliation();
                                fireFight.Fight(unit, other);
                                Debug.Log("Fighting!");

                                hasControl = false;
                                UnitController.Singleton.StopAction();
                                ClearGameObjectList(highlightedTiles);
                                break;
                            }
                        }
                    }
                }

                yield return new WaitForFixedUpdate();
            }

            ClearGameObjectList(highlightedTiles);
        }

        private void ClearGameObjectList(List<GameObject> list)
        {
            foreach (GameObject obj in list)
                Object.Destroy(obj);

            list.Clear();
        }

        private void HighlightTiles(IEnumerable<IPathNode<HexNode>> reachable)
        {
            if (highlightedTiles.Count > 0)
                return;

            GameObject attackRangePrefab = Resources.Load("Prefabs/current_tile") as GameObject;

            // Fix until selection of direction is implemented //
            Dictionary<ITile, bool> tiles = new Dictionary<ITile, bool>();

            foreach (IPathNode<HexNode> node in reachable)
            {
                ITile tile = node.GetNode().Tile;

                if (tiles.ContainsKey(tile))
                    continue;

                tiles.Add(tile, true);
                Vector3 position = new Vector3(tile.PosX, tile.PosY + 0.05f, tile.PosZ);

                GameObject highlight = Object.Instantiate(attackRangePrefab, position, attackRangePrefab.transform.rotation);
                highlightedTiles.Add(highlight);
            }
        }
    }
}
