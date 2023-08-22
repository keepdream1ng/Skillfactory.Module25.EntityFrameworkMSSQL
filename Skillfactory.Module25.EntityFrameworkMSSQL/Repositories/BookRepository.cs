using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Repositories
{
    public class BookRepository : BaseRepository
    {
        public BookRepository() : base() { }
        public BookRepository(AppContext appContext) : base(appContext) { }

        #region CRUD methods
        public int AddBook(string title, int yearOfPublishing)
        {
            DbContext.Books.Add(new Book { Title = title, YearOfPublishing = yearOfPublishing });
            return DbContext.SaveChanges();
        }

        public Book GetBook(int bookId)
        {
            return DbContext.Books.SingleOrDefault(b => b.Id == bookId);
        }

        public List<Book> GetAllBooks()
        {
            return DbContext.Books.ToList();
        }

        public int UpdateBookPublishYear(int bookId, int bookNewYearOfPublish)
        {
            Book bookToUpdate = GetBook(bookId);
            bookToUpdate.YearOfPublishing = bookNewYearOfPublish;
            return DbContext.SaveChanges();
        }

        // Adding book to author credentials realized in AuthorsRepository.
        public int RemoveAuthorFromBook(int bookId, int authorId)
        {
            int result;
            using (var transactionContext = DbContext.Database.BeginTransaction())
            {
                Book bookToUpdate = GetBook(bookId);
                Author authorToUpdate = bookToUpdate.Authors.SingleOrDefault(a => a.Id == authorId);
                authorToUpdate.Books.Remove(bookToUpdate);
                bookToUpdate.Authors.Remove(authorToUpdate);
                result = DbContext.SaveChanges();
                transactionContext.Commit();
            }
            return result;
        }

        public int DeleteBook(int bookId)
        {
            int result;
            using (var transactionContext = DbContext.Database.BeginTransaction())
            {
                Book bookToDelete = GetBook(bookId);
                if (bookToDelete.UserId is not null)
                {
                    DbContext.Users.SingleOrDefault(u => u.Id == bookToDelete.UserId)
                        .BooksBorrowed.Remove(bookToDelete);
                }
                foreach (Author author in bookToDelete.Authors)
                {
                    author.Books.Remove(bookToDelete);
                }
                DbContext.Books.Remove(bookToDelete);
                result = DbContext.SaveChanges();
                transactionContext.Commit();
            }
            return result;
        }
        #endregion
    }
}
