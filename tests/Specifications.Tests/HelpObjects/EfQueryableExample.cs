using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Specifications.Tests.HelpObjects
{
    class EfQueryableExampleContext: DbContext
    {
        public DbSet<SimpleTestObject> SetOfTestObjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Sample");
        }
    }

    class EfFactory
    {
        public static EfQueryableExampleContext CreateTestContext()
        {
            var context = new EfQueryableExampleContext();
            context.Database.EnsureCreated();

            if (!context.SetOfTestObjects.Any())
            {
                context.SetOfTestObjects.Add(new SimpleTestObject() {Count = 17, Name = "ZZZZ"});
                context.SetOfTestObjects.Add(SimpleTestObject.Create_For_Test_Where_Count_Is_Five_And_Name_Is_Name());
                context.SetOfTestObjects.Add(new SimpleTestObject() {Count = 6, Name = "Aaa"});
            }

            context.SaveChanges();
            return context;
        }
    } 
}
