﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.HexImpl
{
    class TileInfo : ITile<HexNode>
    {
        private ITerrain terrain;
        private int elevation;

        public TileInfo(ITerrain terrain, int elevation)
        {
            this.terrain = terrain;
            this.elevation = elevation;
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
            get { return elevation; }
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

        public IUnit<HexNode> AirUnitOnTile
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
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
