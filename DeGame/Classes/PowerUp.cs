using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeGame.Classes;
using DeGame.Enums;

namespace DeGame.Classes
{
    public class PowerUp
    {
        public Enums.TypePowerUp TypePowerUp { get; set; }

        public bool PickedUp
        {
            get; set;
        }

        public PowerUp(Enums.TypePowerUp typePowerUp)
        {
            TypePowerUp = typePowerUp;
        }
    }
}
