using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeGame.Classes
{
    public abstract class Entity
    {
        private int locationX;
        private int locationY;
        private int prevLocationX;
        private int prevLocationY;
        public int LocationX { get { return locationX; } protected set { locationX = value; } }
        public int LocationY { get { return locationY; } protected set { locationY = value; } }
        public int PrevLocationX { get { return prevLocationX; } protected set { prevLocationX = value; } }
        public int PrevLocationY { get { return prevLocationY; } protected set { prevLocationY = value; } }
    }
}
