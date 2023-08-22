using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository() : base() { }
        public UserRepository(AppContext appContext) : base(appContext) { }

        #region CRUD methods
        public int AddUser(string userName, string email)
        {
            DbContext.Users.Add(new User { Name = userName, Email = email });
            return DbContext.SaveChanges();
        }

        public User GetUser(int  id)
        {
            return DbContext.Users.SingleOrDefault(x => x.Id == id);
        }

        public List<User> GetAllUsers()
        {
            return DbContext.Users.ToList();
        }

        public int ChangeUserName(int userId, string userNewName)
        {
            User userToUpdate = GetUser(userId);
            userToUpdate.Name = userNewName;
            return DbContext.SaveChanges();
        }

        public int AssignBookToUser(int userId, int bookId)
        {
            User userToUpdate = GetUser(userId);
            Book bookToUpdate = DbContext.Books.SingleOrDefault(b => b.Id == bookId);
            userToUpdate.BooksBorrowed.Add(bookToUpdate);
            return DbContext.SaveChanges();
        }
        public int UnAssignBookFromUser(int userId, int bookId)
        {
            User userToUpdate = GetUser(userId);
            Book bookToUpdate = DbContext.Books.SingleOrDefault(b => b.Id == bookId);
            userToUpdate.BooksBorrowed.Remove(bookToUpdate);
            return DbContext.SaveChanges();
        }

        public int DeleteUser(int userId)
        {
            int result;
            using (var transactionContext = DbContext.Database.BeginTransaction())
            {
                User userToDelete = GetUser(userId);
                if (userToDelete.BooksBorrowed is not null)
                {
                    foreach (Book book in userToDelete.BooksBorrowed)
                    {
                        book.UserId = null;
                    }
                    userToDelete.BooksBorrowed.Clear();
                }
                DbContext.Users.Remove(userToDelete);
                result = DbContext.SaveChanges();
                transactionContext.Commit();
            }
            return result;
        }
        #endregion
    }
}
