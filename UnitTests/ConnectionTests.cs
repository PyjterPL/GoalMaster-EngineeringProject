using System;
using GoalMaster.Model;
using NUnit.Framework;
using System.Linq;
using GoalMaster.Helpers;

namespace UnitTests
{
    [TestFixture]
    public class ConnectionTests
    {
        [Test]
        public void ShouldReturnTwoGoalTypes()
        {           
            using (var db = new GoalMasterDatabaseContext())
            {
                var res = db.GoalTypes;
                Assert.AreEqual(2, res.Count());
            }
        }
        [Test]
        public void WhenInternetConnectionIsShouldPass()
        {
            //given
            //when
            bool isInternetOn = InternetAvailability.IsInternetAvailable();
            //then
            Assert.IsTrue(isInternetOn);
        }
    }
}
