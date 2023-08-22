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

        [Fact]
        public void GetCategoryByIdShouldReturnCorrectObject()
        {
            // Arrange.
            CategoryRepository repository = new CategoryRepository(fixture.DbContext);
            var expected = repository.DbContext.Categories.OrderBy(c => c.Id).Last();

            // Act.
            var actual = repository.GetCategoryById(expected.Id);

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllCategoriesShouldReturnAllCategories()
        {
            // Arrange.
            CategoryRepository repository = new CategoryRepository(fixture.DbContext);
            var expected = repository.DbContext.Categories.Count();

            // Act.
            var actual = repository.GetAllCategories().Count;

            // Assert.
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void UpdateCategoryNameShouldUpdateDatabase()
        {
            // Arrange.
            CategoryRepository repository = new CategoryRepository(fixture.DbContext);
            int testObjectId  = repository.DbContext.Categories.OrderBy(c => c.Id).Last().Id;
            string expected = Guid.NewGuid().ToString();

            // Act.
            repository.UpdateCategoryName(testObjectId, expected);
            string actual = repository.DbContext.Categories.Single(c => c.Id == testObjectId).Name;

            // Assert.
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void DeleteCategoryShouldDropFromDatabase()
        {
            // Arrange.
            CategoryRepository repository = new CategoryRepository(fixture.DbContext);
            Category expected  = repository.DbContext.Categories.OrderBy(c => c.Id).First();

            // Act.
            repository.DeleteCategory(expected.Id);

            // Assert.
            Assert.DoesNotContain(expected, repository.DbContext.Categories.ToList());
        }
    }
}
