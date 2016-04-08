using DeGame.Enums;

namespace DeGame.Classes
{
    public abstract class Entity
    {
        public int LocationX { get; protected set; }

        public int LocationY { get; protected set; }

        public int PrevLocationX { get; protected set; }

        public int PrevLocationY { get; protected set; }

        public Direction Direction { get; set; }

        protected Entity()
        {
            LocationX = 0;
            LocationY = 0;
            PrevLocationX = 0;
            PrevLocationY = 0;
        }

        public void Move(int x, int y)
        {
            PrevLocationX = LocationX;
            PrevLocationY = LocationY;
            LocationX = x;
            LocationY = y;
        }
    }
}
