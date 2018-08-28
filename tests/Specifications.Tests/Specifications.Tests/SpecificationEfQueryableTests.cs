using System.Linq;
using Specifications.Tests.HelpObjects;
using Xunit;

namespace Specifications.Tests
{
    public class SpecificationEfQueryableTests
    {
        [Fact]
        public void SingleSpecificationWorked()
        {
            string s = "Name";
            var spec = new SpecificationBase<SimpleTestObject>(t => t.Name == s);

            using (var db = EfFactory.CreateTestContext())
            {
                var test = db.SetOfTestObjects.Where(spec).Single();
                var testExpected = SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name();

                Assert.Equal(testExpected.Name, test.Name);
                Assert.Equal(testExpected.Count, test.Count);
            }
        }
    }
}
