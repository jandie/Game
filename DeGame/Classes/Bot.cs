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
        private Cel celCurrentlyIn;

        public int Strength { get { return strength; } }
        public Cel CelCurrentlyIn { get { return celCurrentlyIn; } }

        public Bot()
        {
            killed = false;
            LocationX = 0;
            LocationY = 0;
            PrevLocationX = 0;
            PrevLocationY = 0;
            strength = 100;
        }

        public void Move(Cel cel)
        {
            LocationX = cel.GetX();
            LocationY = cel.GetY();
            celCurrentlyIn = cel;
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
