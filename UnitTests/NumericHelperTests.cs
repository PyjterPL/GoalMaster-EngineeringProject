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
    class NumericHelperTests
    {
        [TestCase("1", ExpectedResult = true)]
        [TestCase("1.11", ExpectedResult = true)]
        [TestCase("1.1s1", ExpectedResult = false)]
        [TestCase("1.1.1", ExpectedResult = false)]
        [TestCase(".1", ExpectedResult = false)]
        [TestCase("", ExpectedResult = true)]
        [TestCase(null, ExpectedResult = true)]
        //[TestCase("11.", ExpectedResult = false)]
        public bool OnlyNumersInString(string number)
        {
            //given
            //when
            var result = NumericHelper.IsTextNumericOnly(number);
            //then
            return result;
        }
    }
}
