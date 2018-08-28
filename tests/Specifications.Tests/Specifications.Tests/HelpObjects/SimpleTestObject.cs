namespace Specifications.Tests.HelpObjects
{
    class SimpleTestObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public static SimpleTestObject Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name()
        {
            return new SimpleTestObject
            {
                Name = "Name",
                Count = 5,
            };
        }
    }
}