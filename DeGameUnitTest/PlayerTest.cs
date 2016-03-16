using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeGame.Classes;

namespace DeGameUnitTest
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void TestPlayer()
        {
            DeGame.Player player = new DeGame.Player();

            Assert.AreEqual(DeGame.Enums.TypePowerUp.None, player.powerUp, "PowerUp set incorrectly");
            Assert.AreEqual(0, player.LocationX, "Location set incorrectly");
            Assert.AreEqual(0, player.LocationY, "Location set incorrectly");
        }
    }
}
