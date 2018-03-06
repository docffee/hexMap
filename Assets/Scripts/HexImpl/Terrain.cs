using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.HexImpl
{
    class Terrain : ITerrain
    {
        private TerrainType type;
        private static ITerrain[] prefabTerrains;

        public Terrain(TerrainType type)
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
            prefabTerrains = new Terrain[6];

            prefabTerrains[0] = new Terrain(TerrainType.Grass);
            prefabTerrains[1] = new Terrain(TerrainType.Water);
            prefabTerrains[2] = new Terrain(TerrainType.Mountain);
            prefabTerrains[3] = new Terrain(TerrainType.Sand);
            prefabTerrains[4] = new Terrain(TerrainType.Forest);
            prefabTerrains[5] = new Terrain(TerrainType.Unpassable);
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
