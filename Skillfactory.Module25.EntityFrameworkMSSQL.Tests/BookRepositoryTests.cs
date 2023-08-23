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

        #region Utility methods tests

        [Theory]
        [InlineData(1, int.MinValue, int.MaxValue)]
        [InlineData(null, int.MinValue, int.MaxValue)]
        [InlineData(1, null, int.MaxValue)]
        [InlineData(1, int.MinValue, null)]
        [InlineData(2, 2000, null)]
        [InlineData(2, null, 2000)]
        [InlineData(2, null, int.MinValue)]
        [InlineData(3, int.MaxValue, null)]
        [InlineData(int.MinValue, int.MaxValue, int.MinValue)]
        public void GetBooksByCategoryInYearIntervalFilterOptionallyShouldFilterCorrectly(int? categoryId, int? startYear, int? endYear)
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            bool expected = false;

            // Act.
            List<Book> filteredBookList = repository.GetBooksByCategoryInYearIntervalFilterOptionally(categoryId, startYear, endYear);
            bool categoryCheck = false;
            bool startYearCheck = false;
            bool endYearCheck = false;
            if (categoryId != null)
            {
                categoryCheck = filteredBookList.Any(c => c.CategoryId != categoryId);
            }
            if (startYear != null)
            {
                startYearCheck = filteredBookList.Any(c => c.YearOfPublishing < startYear);
            }
            if (endYear != null)
            {
                endYearCheck = filteredBookList.Any(c => c.YearOfPublishing > endYear);
            }
            bool actual = categoryCheck || startYearCheck || endYearCheck;

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetBookCountByCategoryShouldReturnCorrectValue()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            Category categoryToCheck = repository.DbContext.Categories.OrderByDescending(b => b.BooksInCategory.Count).First();
            int expected = categoryToCheck.BooksInCategory.Count;

            // Act.
            int actual = repository.GetBooksCountByCategory(categoryToCheck.Id);

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, "test", false)]
        [InlineData("test", "test", false)]
        [InlineData("test", null, false)]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        [InlineData(null, "", true)]
        [InlineData("", "", true)]
        [InlineData("", "test", false)]
        [InlineData("test", "", false)]
        public void CheckLibraryForBookByAuthorsLastnameAndTitleOptionallyShoudReturnCorrectly(string? authorLastname, string? bookTitle, bool expected)
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);

            // Act.
            if (authorLastname == "")
            {
                // Getting value from db for true assertion.
                authorLastname = repository.DbContext.Authors.OrderBy(a => a.Id).First().LastName;
            }
            if (bookTitle == "")
            {
                // Getting value from db for true assertion. Same ordering for testcase where non of arguments are null.
                bookTitle = repository.DbContext.Authors.OrderBy(a => a.Id).First().Books.First().Title;
            }
            bool actual = repository.CheckLibraryForBookByAuthorsLastnameAndTitleOptionally(authorLastname, bookTitle);

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsBookBorrowedShouldReturnCorrectValue()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            Book bookToCheck = repository.DbContext.Users.OrderByDescending(u => u.BooksBorrowed.Count).First().BooksBorrowed.First();
            bool expected = true;

            // Act.
            bool actual = repository.IsBookBorrowed(bookToCheck.Id);

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNewestBookShouldReturnCorrectObject()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            Book expected = new Book { Title = "Testing newest year", YearOfPublishing = 40000 };

            // Act.
            repository.DbContext.Books.Add(expected);
            repository.DbContext.SaveChanges();
            Book actual = repository.GetNewestBook();

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllBooksOrderedByTitleShouldReturnOrderedList()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            bool expected = true;

            // Act.
            List<Book> orderedList = repository.GetAllBooksOrderedByTitle();
            // Check every title pair individually, and change actual if its not alphabetical.
            bool actual = true;
            for (int i = 0; i < orderedList.Count - 1; i++)
            {
                if (StringComparer.Ordinal.Compare(orderedList[i].Title, orderedList[i + 1].Title) > 0)
                {
                    actual = false;
                    break;
                }
            }

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllBooksOrderedByYearFromNewestShouldReturnOrderedList()
        {
            // Arrange.
            BookRepository repository = new BookRepository(fixture.DbContext);
            bool expected = true;

            // Act.
            List<Book> orderedList = repository.GetAllBooksOrderedByYearFromNewest();
            bool actual = true;
            for (int i = 0; i < orderedList.Count - 1; i++)
            {
                if (orderedList[i].YearOfPublishing < orderedList[i + 1].YearOfPublishing)
                {
                    actual = false;
                    break;
                }
            }

            // Assert.
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
