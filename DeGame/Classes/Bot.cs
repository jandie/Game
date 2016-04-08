
namespace DeGame.Classes
{
    public class Bot : Entity
    {
        private bool _killed;
        private int _health;

        public int Strength { get; }
        public int Hitpoints => _health;

        public Bot()
        {
            _killed = false;
            _health = 200;
            Strength = 100;
        }

        public void Kill()
        {
            _killed = true;
        }

        public bool IsKilled()
        {
            return _killed;
        }

        public bool Damage(int strength)
        {
            _health -= strength;

            if (_health < 1)
            {
                Kill();
                return false;
            }

            return true;
        }
    }
}
