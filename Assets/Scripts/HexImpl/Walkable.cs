using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.HexImpl
{
    class Walkable : IWalkable
    {
        private float modifier;
        private bool walkable;

        public Walkable(float modifier, bool walkable)
        {
            this.modifier = modifier;
            this.walkable = walkable;
        }

        public float Modifier
        {
            get
            {
                return modifier;
            }
        }

        public bool Passable
        {
            get
            {
                return walkable;
            }
        }
    }
}
