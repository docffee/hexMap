using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.HexImpl
{
    class HexTerrain : ITerrain
    {
        private TerrainType type;
        private static ITerrain[] prefabTerrains;

        public HexTerrain(TerrainType type)
        {
            this.type = type;
        }

        public string Name
        {
            get
            {
                return type.ToString();
            }
        }

        public bool Equals(ITerrain other)
        {
            if (other == null)
                return false;

            return Name.Equals(other.Name);
        }

        public static ITerrain GetTerrainFromChar(char c)
        {
            if (prefabTerrains == null)
                PopulatePrefabTerrain();

            switch (c)
            {
                case 'G':
                    return prefabTerrains[0];
                case 'W':
                    return prefabTerrains[1];
                case 'M':
                    return prefabTerrains[2];
                case 'S':
                    return prefabTerrains[3];
                case 'F':
                    return prefabTerrains[4];
                default:
                    return prefabTerrains[5];
            }
        }

        private static void PopulatePrefabTerrain()
        {
            prefabTerrains = new HexTerrain[6];

            prefabTerrains[0] = new HexTerrain(TerrainType.Grass);
            prefabTerrains[1] = new HexTerrain(TerrainType.Water);
            prefabTerrains[2] = new HexTerrain(TerrainType.Mountain);
            prefabTerrains[3] = new HexTerrain(TerrainType.Sand);
            prefabTerrains[4] = new HexTerrain(TerrainType.Forest);
            prefabTerrains[5] = new HexTerrain(TerrainType.Unpassable);
        }
    }

    enum TerrainType
    {
        Grass,
        Water,
        Mountain,
        Sand,
        Forest,
        Unpassable
    }
}
