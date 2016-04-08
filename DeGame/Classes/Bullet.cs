using DeGame.Enums;

namespace DeGame.Classes
{
    class Bullet : Entity
    {
        public int ImpactStrenth { get; private set; }
        public bool Destroyed { get; set; }

        public Bullet()
        {
            ImpactStrenth = 100;
        }

        public Bullet(Direction direction, int locationX, int locationY, int cellSize)
        {
            ImpactStrenth = 100;
            Direction = direction;

            LocationX = locationX;
            LocationY = locationY;

            switch (direction)
            {
                case Direction.Down:
                    LocationY = locationY + cellSize;
                    break;
                case Direction.Left:
                    LocationX = LocationX - cellSize;
                    break;
                case Direction.Right:
                    LocationX = locationX + cellSize;
                    break;
                default:
               case Direction.Up:
                    LocationY = locationY - cellSize;
                    break;
            }
        }

        public void Destroy()
        {
            Destroyed = true;
        }

        
    }
}
