using Specifications.Tests.HelpObjects;
using Xunit;

namespace Specifications.Tests
{
    public class SpecificationBaseTests
    {
        [Fact]
        public void SingleSpecificationWorked()
        {
            var spec = new SpecificationBase<SimpleTestObject>(t => t.Name == "Name");
            Assert.True(spec.IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
        }

        [Theory]
        [InlineData("Name", 5)]
        public void SpecificationAndSpecificationTrueTest(string name, int count)
        {
            var spec1 = new SpecificationBase<SimpleTestObject>(t => t.Name == name);
            var spec2 = new SpecificationBase<SimpleTestObject>(t => t.Count == count);

            var spec = spec1 & spec2;

            Assert.True(spec.IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
            Assert.True(spec.And(spec2).IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
        }

        [Theory]
        [InlineData("Name", 3)]
        [InlineData("abc", 5)]
        [InlineData("abc", 3)]
        public void SpecificationAndSpecificationFalseTest(string name, int count)
        {
            var spec1 = new SpecificationBase<SimpleTestObject>(t => t.Name == name);
            var spec2 = new SpecificationBase<SimpleTestObject>(t => t.Count == count);

            var spec = spec1 & spec2;

            Assert.False(spec.IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
            Assert.False(spec.And(spec2).IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
        }


        [Theory]
        [InlineData("Name", 5)]
        [InlineData("Abc", 5)]
        [InlineData("Name", 3)]
        public void SpecificationOrSpecificationTrueTest(string name, int count)
        {
            var spec1 = new SpecificationBase<SimpleTestObject>(t => t.Name == name);
            var spec2 = new SpecificationBase<SimpleTestObject>(t => t.Count == count);

            var spec = spec1 | spec2;

            Assert.True(spec.IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
            Assert.True(spec.Or(spec2).IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
        }

        [Theory]
        [InlineData("abc", 3)]
        public void SpecificationOrSpecificationFalseTest(string name, int count)
        {
            var spec1 = new SpecificationBase<SimpleTestObject>(t => t.Name == name);
            var spec2 = new SpecificationBase<SimpleTestObject>(t => t.Count == count);

            var spec = spec1 | spec2;

            Assert.False(spec.IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
            Assert.False(spec.Or(spec2).IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
        }

        [Theory]
        [InlineData("Name")]
        public void NotSpecificationTest(string name)
        {
            var trueSpec = new SpecificationBase<SimpleTestObject>(t => t.Name == name);

            var falseSpec = !trueSpec;

            Assert.True(trueSpec.IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
            Assert.False(falseSpec.IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));

            Assert.True(trueSpec.IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
            Assert.False(trueSpec.Not().IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
        }

        [Theory]
        [InlineData("Name")]
        public void DoubleNotSpecificationTrueTest(string name)
        {
            var spec1 = new SpecificationBase<SimpleTestObject>(t => t.Name == name);

            var spec = !!spec1;

            Assert.True(spec.IsSatisfiedBy(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()));
        }
    }
}
