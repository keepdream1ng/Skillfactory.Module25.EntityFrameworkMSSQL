using Skillfactory.Module25.EntityFrameworkMSSQL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Tests
{
    [Collection("Database collection")]
    public class AuthorsRepositoryTests
    {
        /// <summary>
        /// Class with global db tests setup and context for integration tests.
        /// </summary>
        DatabaseFixture fixture;

        public AuthorsRepositoryTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        #region CRUD methods tests
        [Fact]
        public void AddAuthorShouldAddToDatabase()
        {
            // Arrange.
            AuthorRepository repository = new AuthorRepository(fixture.DbContext);
            string testFirsname = Guid.NewGuid().ToString();
            string testLastname = Guid.NewGuid().ToString();
            string expected = $"{testFirsname} {testLastname}";

            // Act.
            repository.AddAuthor(testFirsname, testLastname);
            string actual = repository.DbContext.Authors.Where(a => a.FirstName == testFirsname)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .First();

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAuthorByIdShouldReturnCorrectObject()
        {
            // Arrange.
            AuthorRepository repository = new AuthorRepository(fixture.DbContext);
            Author expected = repository.DbContext.Authors.OrderBy(a => a.Id).Last();

            // Act.
            Author actual = repository.GetAuthorById(expected.Id);

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllAuthorsShouldReturnAllObjects()
        {
            // Arrange.
            AuthorRepository repository = new AuthorRepository(fixture.DbContext);
            List<Author> expected = repository.DbContext.Authors.ToList();

            // Act.
            List<Author> actual = repository.GetAllAuthors();

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateAuthorShouldUpdateDatabase()
        {
            // Arrange.
            AuthorRepository repository = new AuthorRepository(fixture.DbContext);
            string testFirsname = Guid.NewGuid().ToString();
            string testLastname = Guid.NewGuid().ToString();
            string expected = $"{testFirsname} {testLastname}";
            Author authorToUpdate = repository.DbContext.Authors.OrderBy(a => a.Id).First();

            // Act.
            repository.UpdateAuthor(authorToUpdate.Id, testFirsname, testLastname);
            string actual = repository.DbContext.Authors.Where(a => a.FirstName == testFirsname)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .First();

            // Assert.
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void AddBookToAuthorShouldUpdateDatabase()
        {
            // Arrange.
            AuthorRepository repository = new AuthorRepository(fixture.DbContext);
            Author authorToUpdate = repository.DbContext.Authors.OrderBy(a => a.Id).First();
            Book bookToUpdate = repository.DbContext.Books.OrderBy(b => b.Id).First();
            bool expected = true;

            // Act.
            repository.AddBookToAuthor(authorToUpdate.Id, bookToUpdate.Id);
            bookToUpdate = repository.DbContext.Books.Single(b => b.Id == bookToUpdate.Id);
            bool actual = repository.DbContext.Authors.Single(a => a.Id == authorToUpdate.Id)
                .Books.Contains(bookToUpdate);

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeleteAuthorShouldDropFromDatabase()
        {
            // Arrange.
            AuthorRepository repository = new AuthorRepository(fixture.DbContext);
            Author expected = repository.DbContext.Authors.OrderByDescending(a => a.Books.Count).First();

            // Act.
            repository.DeleteAuthor(expected.Id);

            // Assert.
            Assert.DoesNotContain(expected, repository.DbContext.Authors.ToList());
        }
        #endregion
    }
}
