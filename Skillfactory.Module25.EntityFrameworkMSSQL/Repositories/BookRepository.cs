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

        #region Utility methods

        // Number 1 in task 25.5.4.
        /// <summary>
        /// Method gives list of books from db according by inputed arguments filters, input null if you want to neglect filter.
        /// </summary>
        public List<Book> GetBooksByCategoryInYearIntervalFilterOptionally(int? categoryId = null, int? startYear = null, int? endYear = null)
        {
            var query = from book in DbContext.Books
                        where ((categoryId == null) || (book.CategoryId == categoryId))
                        && ((startYear == null) || (book.YearOfPublishing >= startYear))
                        && ((endYear == null) || (book.YearOfPublishing <= endYear))
                        select book;
            return query.ToList();
        }

        // Number 3 in task 25.5.4.
        public int GetBooksCountByCategory(int categoryId)
        {
            var query = from book in DbContext.Books
                        where book.CategoryId == categoryId
                        select book;
            return query.Count();
        }

        // Number 4 in task 25.5.4.
        /// <summary>
        /// Method checks batabase for books by authors lastname and or book title, input null to neglect filter.
        /// </summary>
        public bool CheckLibraryForBookByAuthorsLastnameAndTitleOptionally(string? authorsLastname = null, string? bookTitle = null)
        {
            var query = from book in DbContext.Books
                        where ((authorsLastname == null) || (book.Authors.Any(a => a.LastName == authorsLastname)))
                        && ((bookTitle == null) || (book.Title == bookTitle))
                        select book;
            return query.Any();
        }

        #endregion
    }
}
