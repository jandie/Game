
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
