using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeGame.Classes;
using DeGame.Enums;

namespace DeGame
{
    public class Player : Entity
    {
        private Direction direction;
        
        private int hitpoints;
        public Enums.TypePowerUp powerUp { get; set; }

        public Player()
        {
            hitpoints = 100;
            LocationX = 0;
            LocationY = 0;
            PrevLocationX = 0;
            PrevLocationY = 0;
            powerUp = Enums.TypePowerUp.None;
        }

        public void Move(int x, int y)
        {
            PrevLocationX = LocationX;
            PrevLocationY = LocationY;
            LocationX = x;
            LocationY = y;
        }

        public void PickUpPowerUp()
        {
            throw new System.NotImplementedException();
        }

        public Enums.PlayerStatus RemoveHitpoints(int amount)
        {
            hitpoints -= amount;
            if (hitpoints < 1)
            {
                return PlayerStatus.Dead;
            }
            return PlayerStatus.Alive;
        }
    }
}