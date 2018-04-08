using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Front.Actions
{
    class AttackAction : IAction
    {
        private Unit unit;
        private bool hasControl;

        public AttackAction(Unit unit)
        {
            this.unit = unit;
            hasControl = false;
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

                                yield return new WaitForEndOfFrame(); // Stops the move action from executing right after //
                                hasControl = false;
                                UnitController.Singleton.ResetAction();
                                break;
                            }
                        }
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
