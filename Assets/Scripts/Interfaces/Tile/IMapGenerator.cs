using GraphAlgorithms;

namespace Assets.Scripts.HexImpl
{
    public interface IMapGenerator
    {
        ITile[] GenerateTiles(int sizeX, int sizeY);
    }
}