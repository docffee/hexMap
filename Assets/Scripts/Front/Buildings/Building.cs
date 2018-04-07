
using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Front.Buildings
{
    public abstract class Building : MonoBehaviour, IBuilding
    {
        private int maxHealth = 1;
        private int currentHealth = 1;

        protected ITile tile;
        protected IPlayer controller;
        protected ITileControl<HexNode> hexControl;
        protected GameController gameController;

        public void Initialize(IPlayer controller, ITileControl<HexNode> hexControl, GameController gameController)
        {
            this.controller = controller;
            this.hexControl = hexControl;
            this.gameController = gameController;
        }

        public int CurrentHealth
        {
            get
            {
                return currentHealth;
            }

            set
            {
                currentHealth = value;
            }
        }

        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
        }

        public ITile Tile
        {
            get
            {
                return tile;
            }

            set
            {
                tile = value;
            }
        }

        public float PosX
        {
            get
            {
                return transform.position.x;
            }
        }

        public float PosY
        {
            get
            {
                return transform.position.y;
            }
        }

        public float PosZ
        {
            get
            {
                return transform.position.z; ;
            }
        }
    }
}
