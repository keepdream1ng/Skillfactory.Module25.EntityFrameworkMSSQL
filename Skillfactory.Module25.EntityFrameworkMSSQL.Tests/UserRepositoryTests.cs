using Skillfactory.Module25.EntityFrameworkMSSQL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Tests
{
    [Collection("Database collection")]
    public class UserRepositoryTests
    {
        /// <summary>
        /// Class with global db tests setup and context for integration tests.
        /// </summary>
        DatabaseFixture fixture;

        public UserRepositoryTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void AddUserShouldAddToDatabase()
        {
            // Arrange.
            UserRepository repository = new UserRepository(fixture.DbContext);
            string testName = Guid.NewGuid().ToString();
            string testEmail = Guid.NewGuid().ToString();
            string expected = $"{testName} {testEmail}";

            // Act.
            repository.AddUser(testName, testEmail);
            string actual = repository.DbContext.Users.Where(u => u.Email == testEmail)
                .Select(u => $"{u.Name} {u.Email}")
                .First();

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetUserShouldReturnCorrectObject()
        {
            // Arrange.
            UserRepository repository = new UserRepository(fixture.DbContext);
            User expected = repository.DbContext.Users.OrderBy(u => u.Id).Last();

            // Act.
            User actual = repository.GetUser(expected.Id);

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllUsersShouldReturnAllObject()
        {
            // Arrange.
            UserRepository repository = new UserRepository(fixture.DbContext);
            List<User> expected = repository.DbContext.Users.ToList();

            // Act.
            List<User> actual = repository.GetAllUsers();

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ChangeUserNameShouldUpdateDatabase()
        {
            // Arrange.
            UserRepository repository = new UserRepository(fixture.DbContext);
            User userToUpdate = repository.DbContext.Users.OrderBy(u => u.Id).First();
            string expected = Guid.NewGuid().ToString();

            // Act.
            repository.ChangeUserName(userToUpdate.Id, expected);
            string actual = repository.DbContext.Users.Single(u => u.Id == userToUpdate.Id).Name;

            // Assert.
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AssignBookToUserShouldUpdateDatabase()
        {
            // Arrange.
            UserRepository repository = new UserRepository(fixture.DbContext);
            User userToUpdate = repository.DbContext.Users.OrderBy(u => u.Id).First();
            Book bookToUpdate = repository.DbContext.Books.Where(b => b.UserId == null).First();
            Book expected = bookToUpdate;

            // Act.
            repository.AssignBookToUser(userToUpdate.Id, bookToUpdate.Id);
            List<Book> actual = repository.DbContext.Users.Single(u => u.Id == userToUpdate.Id).BooksBorrowed;

            // Assert.
            Assert.Contains(expected, actual);
            Assert.Equal(expected.UserId, userToUpdate.Id);
        }

        [Fact]
        public void UnAssignBookFromUserShouldUpdateDatabase()
        {
            // Arrange.
            UserRepository repository = new UserRepository(fixture.DbContext);
            User userToUpdate = repository.DbContext.Users.OrderByDescending(u => u.BooksBorrowed.Count).First();
            Book bookToUpdate = userToUpdate.BooksBorrowed.First();
            Book expected = bookToUpdate;

            // Act.
            repository.UnAssignBookFromUser(userToUpdate.Id, bookToUpdate.Id);
            List<Book> actual = repository.DbContext.Users.Single(u => u.Id == userToUpdate.Id).BooksBorrowed;

            // Assert.
            Assert.DoesNotContain(expected, actual);
        }

        [Fact]
        public void DeleteUserShouldDropFromDatabase()
        {
            // Arrange.
            UserRepository repository = new UserRepository(fixture.DbContext);
            User expected = repository.DbContext.Users.OrderBy(u => u.Id).First();

            // Act.
            repository.DeleteUser(expected.Id);
            List<User> actual = repository.DbContext.Users.ToList();

            // Assert.
            Assert.DoesNotContain(expected, actual);
        }
    }
}
