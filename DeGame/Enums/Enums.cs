using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeGame.Enums
{
    public enum Direction {Up, Down, Left, Right}
    public enum Object { Void, Destination, Grass, StartPoint, Wall }
    public enum TypePowerUp { MarioStar = 0, None }
    public enum PlayerStatus { Alive, Dead, Win}
}
