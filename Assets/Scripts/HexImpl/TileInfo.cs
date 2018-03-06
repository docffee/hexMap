using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.HexImpl
{
    class TileInfo : ITile<HexNode>
    {
        private ITerrain terrain;

        public TileInfo(ITerrain terrain)
        {
            this.terrain = terrain;
        }

        public int X
        {
            get { return 0; }
        }

        public int Z
        {
            get { return 0; }
        }

        public int Y
        {
            get { return 0; }
        }

        public ITerrain Terrain
        {
            get { return terrain; }
        }

        public IUnit<HexNode> UnitOnTile
        {
            get { return null; }
        }

        public float WorldPosX
        {
            get { return 0; }
        }

        public float WorldPosY
        {
            get { return 0; }
        }

        public float WorldPosZ
        {
            get { return 0; }
        }

        IUnit<HexNode> ITile<HexNode>.UnitOnTile
        {
            get
            {
                return null;
            }

            set
            {
                
            }
        }
    }
}
