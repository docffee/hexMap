using GraphAlgorithms;

namespace Assets.Scripts.HexImpl
{
    public interface IMapGenerator<N> where N : INode<N>
    {
        ITile<N>[] GenerateTiles(int sizeX, int sizeY);
    }
}