using Xunit;

namespace Specifications.Tests
{
    public class SpecificationBaseTests
    {
        [Fact]
        public void SingleSpecificationWorked()
        {
            var spec = new SpecificationBase<SimpleTestObject>(t => t.Name == "Name");
            Assert.True(spec.IsSatisfiedBy(SimpleTestObject.Create()));
        }

        [Theory]
        [InlineData("Name", 5)]
        public void SpecificationAndSpecificationTrueTest(string name, int count)
        {
            var spec1 = new SpecificationBase<SimpleTestObject>(t => t.Name == name);
            var spec2 = new SpecificationBase<SimpleTestObject>(t => t.Count == count);

            var spec = spec1 & spec2;

            Assert.True(spec.IsSatisfiedBy(SimpleTestObject.Create()));
            Assert.True(spec.And(spec2).IsSatisfiedBy(SimpleTestObject.Create()));
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

            Assert.False(spec.IsSatisfiedBy(SimpleTestObject.Create()));
            Assert.False(spec.And(spec2).IsSatisfiedBy(SimpleTestObject.Create()));
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

            Assert.True(spec.IsSatisfiedBy(SimpleTestObject.Create()));
            Assert.True(spec.Or(spec2).IsSatisfiedBy(SimpleTestObject.Create()));
        }

        [Theory]
        [InlineData("abc", 3)]
        public void SpecificationOrSpecificationFalseTest(string name, int count)
        {
            var spec1 = new SpecificationBase<SimpleTestObject>(t => t.Name == name);
            var spec2 = new SpecificationBase<SimpleTestObject>(t => t.Count == count);

            var spec = spec1 | spec2;

            Assert.False(spec.IsSatisfiedBy(SimpleTestObject.Create()));
            Assert.False(spec.Or(spec2).IsSatisfiedBy(SimpleTestObject.Create()));
        }

        [Theory]
        [InlineData("Name")]
        public void NegativeSpecificationTest(string name)
        {
            var trueSpec = new SpecificationBase<SimpleTestObject>(t => t.Name == name);

            var falseSpec = !trueSpec;

            Assert.True(trueSpec.IsSatisfiedBy(SimpleTestObject.Create()));
            Assert.False(falseSpec.IsSatisfiedBy(SimpleTestObject.Create()));

            Assert.True(trueSpec.IsSatisfiedBy(SimpleTestObject.Create()));
            Assert.False(trueSpec.Negative().IsSatisfiedBy(SimpleTestObject.Create()));
        }

        [Theory]
        [InlineData("Name")]
        public void DoubleNegativeSpecificationTrueTest(string name)
        {
            var spec1 = new SpecificationBase<SimpleTestObject>(t => t.Name == name);

            var spec = !!spec1;

            Assert.True(spec.IsSatisfiedBy(SimpleTestObject.Create()));
        }

        class SimpleTestObject
        {
            public string Name { get; set; } = "Name";

            public int Count { get; set; } = 5;

            public static SimpleTestObject Create()
            {
                return new SimpleTestObject();
            }
        }
    }
}
