using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeGame.Classes;
using DeGame.Enums;

namespace DeGame.Classes
{
    public class Bot : Entity
    {
        private int strength;
        private bool killed;

        public int Strength { get { return strength; } }

        public Bot()
        {
            //killed = true;
            killed = false;
            LocationX = 0;
            LocationY = 0;
            PrevLocationX = 0;
            PrevLocationY = 0;
            strength = 100;
        }

        public void Move(int x, int y)
        {
            PrevLocationX = LocationX;
            PrevLocationY = LocationY;
            LocationX = x;
            LocationY = y;

        }

        public void Kill()
        {
            killed = true;
        }

        public bool IsKilled()
        {
            return killed;
        }
    }
}
