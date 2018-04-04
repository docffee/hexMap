using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.HexImpl
{
    class TileInfo : ITile
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

        public IUnit UnitOnTile
        {
            get { return null; }
        }

        public float PosX
        {
            get { return 0; }
        }

        public float PosY
        {
            get { return 0; }
        }

        public float PosZ
        {
            get { return 0; }
        }

        public IUnit AirUnitOnTile
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

        IUnit ITile.UnitOnTile
        {
            get
            {
                return null;
            }

            set
            {
                
            }
        }

        public IResource resourceOnTile
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
