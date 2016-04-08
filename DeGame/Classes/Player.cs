using DeGame.Enums;

namespace DeGame.Classes
{
    public class Player : Entity
    {
        
        public int Hitpoints { get; private set; }
        public Enums.TypePowerUp PowerUp { get; set; }
        

        public Player()
        {
            Hitpoints = 1000;
            PowerUp = Enums.TypePowerUp.None;
        }

        public Enums.PlayerStatus RemoveHitpoints(int amount)
        {
            Hitpoints -= amount;

            if (Hitpoints < 1)
            {
                return PlayerStatus.Dead;
            }

            return PlayerStatus.Alive;
        }
    }
}