using DeGame.Enums;

namespace DeGame.Classes
{
    public class Player : Entity
    {
        
        private int _hitpoints;

        public Enums.TypePowerUp PowerUp { get; set; }
        

        public Player()
        {
            _hitpoints = 100;
            PowerUp = Enums.TypePowerUp.None;
        }

        public Enums.PlayerStatus RemoveHitpoints(int amount)
        {
            _hitpoints -= amount;

            if (_hitpoints < 1)
            {
                return PlayerStatus.Dead;
            }

            return PlayerStatus.Alive;
        }
    }
}