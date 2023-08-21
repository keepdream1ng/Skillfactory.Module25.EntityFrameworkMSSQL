using Skillfactory.Module25.EntityFrameworkMSSQL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Tests
{
    [Collection("Database collection")]
    public class CategoryRepositoryTests
    {
        /// <summary>
        /// Class with global db tests setup and context for integration tests.
        /// </summary>
        DatabaseFixture fixture;

        public CategoryRepositoryTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void CreateCategoryShouldAddToDatabase()
        {
            // Arrange.
            CategoryRepository repository = new CategoryRepository(fixture.DbContext);
            string newCategoryName = Guid.NewGuid().ToString();
            bool expected = true;

            // Act.
            repository.CreateCategory(newCategoryName);
            bool actual = repository.DbContext.Categories.Where(c => c.Name == newCategoryName).Any();


            // Assert.
            Assert.Equal(expected, actual);
        }

    }
}
