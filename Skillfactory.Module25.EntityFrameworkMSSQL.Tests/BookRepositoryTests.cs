using Skillfactory.Module25.EntityFrameworkMSSQL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Tests
{
    [Collection("Database collection")]
    public class BookRepositoryTests
    {
        /// <summary>
        /// Class with global db tests setup and context for integration tests.
        /// </summary>
        DatabaseFixture fixture;

        public BookRepositoryTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        #region CRUD methods tests
        [Fact]
        public void AddBookShouldAddToDatabase()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            string testTitle = Guid.NewGuid().ToString();
            int testYear = 1111;
            string expected = $"{testTitle} {testYear}";

            // Act.
            repository.AddBook(testTitle, testYear);
            string actual = repository.DbContext.Books.Where(b => b.Title == testTitle)
                .Select(b => $"{b.Title} {b.YearOfPublishing}")
                .First();

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetBookShouldReturnCorrectObject()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            Book expected = repository.DbContext.Books.OrderBy(b => b.Id).First();

            // Act.
            Book actual = repository.GetBook(expected.Id);

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllBooksShouldReturnAllObjects()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            List<Book> expected = repository.DbContext.Books.ToList();

            // Act.
            List<Book> actual = repository.GetAllBooks();

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateBookPublishYearShouldUpdateDatabase()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            Book bookToUpdate = repository.DbContext.Books.OrderBy(b => b.YearOfPublishing).First();
            int expected = 1000;

            // Act.
            repository.UpdateBookPublishYear(bookToUpdate.Id, expected);
            int actual = repository.DbContext.Books.Single(b => b.Id == bookToUpdate.Id).YearOfPublishing;

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RemoveAuthorFromBookShouldUpdateDatabase()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            Book bookToUpdate = repository.DbContext.Books.OrderByDescending(b => b.Authors.Count).First();
            Author expected = bookToUpdate.Authors.First();

            // Act.
            repository.RemoveAuthorFromBook(bookToUpdate.Id, expected.Id);
            List<Author> actual = repository.DbContext.Books.Single(b => b.Id == bookToUpdate.Id).Authors;

            // Assert.
            Assert.DoesNotContain(expected, actual);
        }

        [Fact]
        public void DeleteBookShouldDropFromDatabase()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            Book expected = repository.DbContext.Books.OrderByDescending(b => b.Id).First();

            // Act.
            repository.DeleteBook(expected.Id);
            List<Book> actual = repository.DbContext.Books.ToList();

            // Assert.
            Assert.DoesNotContain(expected, actual);
        }
        #endregion
    }
}
