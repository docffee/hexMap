using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphAlgorithms;

namespace Assets.Scripts.HexImpl
{
    class DummyUnit : IUnit<HexNode>
    {
        public int CurrentActionPoints
        {
            get
            {
                return 2;
            }
        }

        public int Direction
        {
            get
            {
                return 0;
            }
        }

        public int MaxActionPoints
        {
            get
            {
                return 2;
            }
        }

        public void Move(IEnumerable<IEdge<HexNode>> path)
        {
            // Do nothing
        }
    }
}
