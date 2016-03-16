using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeGame.Classes;
using DeGame.Enums;

namespace DeGame
{
    public class Cel
    {
        private Enums.Object typeCel;
        private Classes.PowerUp powerUpInCell;
        private int locationX;
        private int locationY;
        
        public Cel (Enums.Object typeCel ,int x, int y)
        {
            this.typeCel = typeCel;
            powerUpInCell = null;
            locationX = x;
            locationY = y;
        }
        public void SetObject(Enums.Object typeCel)
        {
            this.typeCel = typeCel;
        }
        public int GetX()
        {
            return locationX;
        }
        public int GetY()
        {
            return locationY;
        }

        public Enums.Object GetTypeCel()
        {
            return typeCel;
        }

        public void SetPowerUp(PowerUp powerUp)
        {
            powerUpInCell = powerUp;
        }
        public PowerUp GetPowerUp()
        {
            return powerUpInCell;
        }
    }
}