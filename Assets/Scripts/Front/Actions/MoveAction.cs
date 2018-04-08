using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using GraphAlgorithms;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.Actions
{
    class MoveAction : IAction, IReady
    {
        private Unit unit;
        private ITileControl<HexNode> hexControl;

        private bool hasControl;
        private bool performingAction;

        private List<GameObject> highlightedTiles;
        private List<GameObject> highlightedPath;
        Quaternion lastPathArrowRotation;
        private int hoverDirection;
        private ITile hoverOver;

        public MoveAction(Unit unit, ITileControl<HexNode> hexControl)
        {
            this.unit = unit;
            this.hexControl = hexControl;
            hasControl = false;
            performingAction = false;
            highlightedTiles = new List<GameObject>();
            highlightedPath = new List<GameObject>();
        }

        public void TakeControl()
        {
            hasControl = true;
            unit.StartCoroutine(Controller());

            IEnumerable<IPathNode<HexNode>> reachable = hexControl.GetReachable(unit, unit.Tile);
            HighlightTiles(reachable);
        }

        private IEnumerator Controller()
        {
            while (hasControl)
            {
                if (performingAction)
                {
                    yield return new WaitForFixedUpdate();
                    continue;
                }

                RaycastHit tileHit;
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
                    out tileHit, 200.0f, LayerMask.GetMask("Tile"));

                ITile hoverNow =
                    (
                        (tileHit.collider != null) ?
                        tileHit.collider.gameObject.GetComponent<ITile>() :
                        null
                    );

                int dir = GetTileDirection(tileHit);

                if (hoverNow != null && unit.IsTilePassable(hoverNow))
                {
                    if (!unit.GetTerrainWalkability(hoverNow.Terrain).Passable)
                    {
                        yield return new WaitForFixedUpdate();
                        continue;
                    }

                    if (!hoverNow.Equals(hoverOver) || hoverDirection != dir) // Calculates shortest path and shows it.
                    {
                        hoverDirection = dir;
                        hoverOver = hoverNow;
                        IEnumerable<IPathNode<HexNode>> path = hexControl.GetShortestPath(unit, unit.Tile, hoverNow, hoverDirection);
                        ClearGameObjectList(highlightedPath);
                        HighlightPath(path);

                        int pathCount = highlightedPath.Count;
                        if (dir == 6)
                            highlightedPath[pathCount - 1].transform.rotation = lastPathArrowRotation;
                        else
                            highlightedPath[pathCount - 1].transform.rotation =
                                Quaternion.Euler(90, HexUtil.DirectionRotation((HexDirection)dir) - 90, 0);
                    }
                }

                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && hoverNow != null)
                {
                    HexCell cell = tileHit.collider.gameObject.GetComponent<HexCell>();

                    IEnumerable<IPathNode<HexNode>> path = hexControl.GetShortestPath(unit, unit.Tile, cell, hoverDirection);
                    if (path != null)
                    {
                        performingAction = true;
                        unit.Move(path, this);
                        ClearGameObjectList(highlightedTiles);
                        ClearGameObjectList(highlightedPath);
                    }
                }

                yield return new WaitForFixedUpdate();
            }

            ClearGameObjectList(highlightedTiles);
            ClearGameObjectList(highlightedPath);
        }

        private void HighlightTiles(IEnumerable<IPathNode<HexNode>> reachable)
        {
            if (highlightedTiles.Count > 0)
                return;

            GameObject tileMovementPrefab = Resources.Load("Prefabs/tile_movement") as GameObject;
            GameObject currentTilePrefab = Resources.Load("Prefabs/current_tile") as GameObject;

            // Fix until selection of direction is implemented //
            Dictionary<ITile, bool> tiles = new Dictionary<ITile, bool>();

            foreach (IPathNode<HexNode> node in reachable)
            {
                ITile tile = node.GetNode().Tile;

                if (tiles.ContainsKey(tile))
                    continue;

                tiles.Add(tile, true);
                Vector3 position = new Vector3(tile.PosX, tile.PosY + 0.05f, tile.PosZ);

                GameObject highlight = Object.Instantiate(tileMovementPrefab, position, tileMovementPrefab.transform.rotation);
                highlightedTiles.Add(highlight);
            }

            Vector3 currentTilePosition = new Vector3(unit.Tile.PosX, unit.Tile.PosY + 0.10f, unit.Tile.PosZ);
            GameObject currentTile = Object.Instantiate(currentTilePrefab, currentTilePosition, currentTilePrefab.transform.rotation);
            highlightedTiles.Add(currentTile);
        }

        private void HighlightPath(IEnumerable<IPathNode<HexNode>> path)
        {
            if (path == null)
                return;

            GameObject pathArrowPrefab = Resources.Load("Prefabs/walk_arrow") as GameObject;
            Dictionary<ITile, bool> tiles = new Dictionary<ITile, bool>();

            IEnumerator<IPathNode<HexNode>> enumerator = path.GetEnumerator();
            enumerator.MoveNext(); // Skips first //
            tiles.Add(enumerator.Current.GetNode().Tile, true);

            while (enumerator.MoveNext())
            {
                IPathNode<HexNode> node = enumerator.Current;
                HexNode hexNode = node.GetNode();
                ITile tile = hexNode.Tile;

                if (tiles.ContainsKey(tile))
                    continue;
                tiles.Add(tile, true);

                Vector3 position = new Vector3(tile.PosX, tile.PosY + 0.05f, tile.PosZ);
                Quaternion rotation = Quaternion.Euler(90, hexNode.Direction.DirectionRotation() - 90, 0);

                GameObject highlight = Object.Instantiate(pathArrowPrefab, position, rotation);
                highlightedPath.Add(highlight);
                lastPathArrowRotation = rotation;
            }
        }

        private void ClearGameObjectList(List<GameObject> list)
        {
            foreach (GameObject obj in list)
                Object.Destroy(obj);

            list.Clear();
        }

        private int GetTileDirection(RaycastHit tileHit)
        {
            int dir = 6;
            if (tileHit.collider != null)
            {
                Vector3 hit = tileHit.point;
                Vector3 tile = tileHit.collider.gameObject.transform.position;
                float dist = Vector3.Distance(tile, hit);
                const float centerR = 3f;


                if (dist <= centerR)
                {
                    dir = 6;
                }
                else
                {
                    Vector3 temp = hit - tile;
                    Vector3 helperPos = new Vector3(0, 0, 1);
                    float angle =
                        (
                            (temp.x > 0) ?
                            Vector3.Angle(helperPos, temp) :
                            180 + (Mathf.Abs(Vector3.Angle(helperPos, temp) - 180))
                        );
                    dir = ((int)angle / 60);
                }
            }

            return dir;
        }

        public void Ready()
        {
            performingAction = false;
            IEnumerable<IPathNode<HexNode>> reachable = hexControl.GetReachable(unit, unit.Tile);
            HighlightTiles(reachable);
        }

        public string ActionName
        {
            get
            {
                return "Move";
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
    }
}
