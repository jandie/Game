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
            Player player = new Player();

            Assert.AreEqual(DeGame.Enums.TypePowerUp.None, player.PowerUp, "PowerUp set incorrectly");
            Assert.AreEqual(0, player.LocationX, "Location set incorrectly");
            Assert.AreEqual(0, player.LocationY, "Location set incorrectly");
        }
    }
}
