﻿using Assets.Scripts.HexImpl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreindalMap : IMapGenerator
{
    private const string tiles = "GGGGGGGGGGGGFFFGGGGGGGGGFFFFFFFFFFFFFFFFWWWWWWWGGGGGFFFGGGGGGFFFFFFFFF" +
        "FFFFFFFFFFGWWWWWWWGGGGFFFGGGGFFFFFFFFFFFFFFFFFFFFFGWWWWWWGGGGGFFFGFFFFFFFGGGFFF" +
        "FFFFFFFFFFFGGWWWWWWGGGGFFFFFFFFFGGGGGGGGGGFFFFFFFFFGWWWWWWGGGGFFFFFFFFGGGGGGGGG" +
        "GGGGGFFFFFFFGGWWWWWGGGGGFFFFFFGGGGGGGGGGGGGGGGGGFFGGGGWWWWGGGGGFFFFFGGGGGGGGGGG" +
        "SGGGGGGGGGGGGGGWWWWGGGGGFFFFFGGGGGGGGGGGSSSSSSSSGGGMGGGWWWGGGGGFFFFFGGGGGGSSSSS" +
        "SSSSSSSSSMMMMMGGGWWGGGGGFFFFGGGGGGSSSSSSSSSFWSSSSSMMMMGGGWGGGGGFFFFGGGGGGGGGGGS" +
        "SSSFFWSSSMMMMMMGGGWWGGGGFFFGGGGGGGGGGGGSSSSSSSSSSSGGGGGGGGWGGGGFFFFGGMMMGGGGGGG" +
        "SSSSSSSSSSSGGGGGGGGGWGGGFFFFGGGMMMMGGGGGGGGSSSSSSSGGGGGGGGGGGGGGFGGGGGGMMMMGGGG" +
        "GGGGGGGSSSGGGGGGGGGGGGGGGGGGGGGGMMMMMGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGMMMMMGG" +
        "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGMMMMMMGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGMMMMMMM" +
        "MGGGGGGGGGGGGGGGGGG";

    public ITile[] GenerateTiles(int sizeX, int sizeY)
    {
        sizeX = 40;
        sizeY = 20;
        char[] tileChars = tiles.ToCharArray();
        ITile[] generated = new TileInfo[sizeX * sizeY];
        for (int i = 0; i < tileChars.Length; i++)
        {
            char c = tileChars[i];

            int elevation = 0;
            if (c == 'M')
                elevation = 15;
            else if (c == 'F')
                elevation = 3;
            else if (c == 'W')
                elevation = -3;

            generated[i] = new TileInfo(HexTerrain.GetTerrainFromChar(c), elevation);
        }

        return generated;
    }
}
