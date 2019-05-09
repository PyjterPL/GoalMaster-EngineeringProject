using GoalMaster.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    class RegistrationTests
    {
        [TestCase("a@a.pl",ExpectedResult = true)]
        [TestCase("a @a.pl", ExpectedResult = false)]
        [TestCase("a@@a.pl", ExpectedResult = false)]
        [TestCase("a.a.pl", ExpectedResult = false)]
        public bool MailAdressTest(string mail)
        {
            //given
            //when
            var result = new RegexUtilities().IsValidEmail(mail);
            //then
            return result;
        }

    }
}
