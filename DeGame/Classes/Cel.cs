namespace DeGame.Classes
{
    public class Cel
    {
        private Enums.Object _typeCel;
        private Classes.PowerUp _powerUpInCell;
        private int _locationX;
        private int _locationY;

        public Bot Bot{get; set;}
        
        public Cel (Enums.Object typeCel ,int x, int y)
        {
            this._typeCel = typeCel;
            _powerUpInCell = null;
            _locationX = x;
            _locationY = y;
        }

        public void SetObject(Enums.Object typeCel)
        {
            this._typeCel = typeCel;
        }

        public int GetX()
        {
            return _locationX;
        }

        public int GetY()
        {
            return _locationY;
        }

        public Enums.Object GetTypeCel()
        {
            return _typeCel;
        }

        public void SetPowerUp(PowerUp powerUp)
        {
            _powerUpInCell = powerUp;
        }

        public void ReSetPowerUp()
        {
            if (_powerUpInCell != null) { _powerUpInCell.TypePowerUp = Enums.TypePowerUp.None; }
        }

        public PowerUp GetPowerUp()
        {
            return _powerUpInCell;
        }
    }
}